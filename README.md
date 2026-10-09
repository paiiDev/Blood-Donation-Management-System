# Blood Donation Management System (BDMS)

A web-based blood donation management system built with ASP.NET Core (MVC) and Entity Framework Core. It lets the public book blood donation appointments online at registered donation centers and provides an authenticated admin dashboard for managing bookings and blood inventory.

The user-facing UI is in Myanmar (Burmese); the admin panel uses English.

## Features

### Public booking flow
- Multi-step booking modal on the landing page:
  - **Step 1** – pick donation center, appointment date, and time slot
  - **Step 2** – enter donor details (name, blood group, phone, email)
- Live availability check per center/date via a stored procedure (`CheckDailyAvailability`)
- Past dates are rejected; date picker minimum is set client-side and validated server-side
- Myanmar phone-number validation with `^(09|\+?959)\d{7,9}$`
- Blocks repeat bookings while a previous booking is still `Pending` (checked by phone or email)
- Unique booking number generated as `BK-yyyyMMdd-XXXX` (e.g. `BK-20260802-A1B2`)
- Donor + appointment saved atomically in a single DB transaction
- SweetAlert2 feedback and live client-side validation throughout

### Admin authentication
- Cookie-based authentication (`Microsoft.AspNetCore.Authentication.Cookies`)
- Passwords hashed with BCrypt (`BCrypt.Net-Next`)
- Role-based claims (`NameIdentifier`, `Name`, `Role`, optional `CenterId` in the claim)
- Dedicated `Auth` controllers/views with an access-denied page

### Admin dashboard
- `[Authorize]`-protected dashboard (protected routes redirect to the login page)
- Stat cards (total donors, appointments, pending bookings, blood units) and a recent-bookings table
- Blood stock progress bars grouped by blood type

## Tech Stack

| Area       | Technology |
|------------|------------|
| Framework  | ASP.NET Core 8.0 (MVC) |
| ORM        | Entity Framework Core 8 |
| Data access| Dapper (for stored procedures) |
| Database   | SQL Server |
| Auth       | Cookie authentication + BCrypt password hashing |
| Frontend   | Bootstrap 5, jQuery Validation, Font Awesome, AOS, SweetAlert2 |

## Solution Structure

The solution is split into four projects following a layered architecture.

```
Blood-Donation-Management-System.sln
├─ Blood-Donation-Management-System   (BDMS.App)  – ASP.NET Core MVC web app
├─ BDMS.Domain                                    – Business logic (services, interfaces, helpers)
│   ├─ Services/         BookingService, AuthService
│   ├─ Interfaces/       IBookingService, IAuthService
│   └─ Helpers/          TimeSlotHelper
├─ BDMS.Shared                                     – Cross-cutting contracts
│   ├─ DTOs/            Auth, Booking, Result
│   └─ Enums/           TimeSlotEnum
└─ DBMS.Database                                   – Data access layer
    ├─ DataAccess/      AppDbContext + entities
    ├─ Repositories/    BookingRepository, AuthRepository
    └─ Interfaces/      IBookingRepository, IAuthRepository
```

### Request flow

```
Browser → Controller → Service (BDMS.Domain) → Repository (DBMS.Database) → AppDbContext / Dapper → SQL Server
   ← Json / View       ← Result<T>             ← domain models / DTOs
```

- **BDMS.Shared** – DTOs (`LoginDto`, `CreateBookingDto`, `DonationCenterDto`, `AvailabilitySPResult`), the shared `Result<T>` wrapper (`IsSuccess`, `Data`, `ErrorMessage`), and the `TimeSlotEnum`.
- **BDMS.Domain** – service layer implementing business rules (past-date rejection, pending-donor blocking, booking-number generation, availability checks).
- **DBMS.Database** – EF Core `AppDbContext` with entities, repositories, and a Dapper call for the `CheckDailyAvailability` stored procedure.
- **BDMS.App** – MVC presentation layer (controllers, Razor views, static assets).

## Database Schema

Entities are scaffolded/database-first style partial classes under `DBMS.Database/DataAccess`:

| Entity          | Purpose                                             |
|-----------------|-----------------------------------------------------|
| `SystemAdmin`   | Admin user accounts (username, BCrypt password hash, role, optional center) |
| `DonationCenter`| Donation centers with a daily maximum capacity      |
| `Donor`         | Donor records (name, email, unique phone, blood group) |
| `Appointment`   | Booking appointments (unique booking number, date, time slot, status) |
| `BloodType`     | Blood groups (A+, O-, AB+, etc.)                    |
| `DonationRecord`| Completed donations tied to appointments            |
| `BloodInventory`| Stored blood units (expiry date, status, blood group, source donation) |

Global query filters exclude soft-deleted rows (`IsDeleted`) for donors, admins, and centers.

### Required stored procedure

Availability checking relies on a stored procedure named `CheckDailyAvailability` that takes `@CenterId` and `@AppointmentDate` and returns a row with `IsAvailable` (bit) and `Message` (nvarchar). Example:

```sql
CREATE PROC dbo.CheckDailyAvailability
    @CenterId INT,
    @AppointmentDate DATE
AS
BEGIN
    DECLARE @Capacity INT, @Booked INT;

    SELECT @Capacity = MaxCapacityPerDay
    FROM dbo.DonationCenters
    WHERE Id = @CenterId;

    SELECT @Booked = COUNT(*)
    FROM dbo.Appointments
    WHERE CenterId = @CenterId
      AND AppointmentDate = @AppointmentDate
      AND Status IN ('Pending', 'Approved');

    SELECT
        IsAvailable = CASE WHEN @Booked < @Capacity THEN 1 ELSE 0 END,
        Message     = CASE WHEN @Booked < @Capacity
                           THEN N'Available'
                           ELSE N'This center is fully booked for the selected date.'
                      END;
END;
```

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local or remote)
- Visual Studio 2022 (any edition) or your preferred editor

### Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd Blood-Donation-Management-System
   ```

2. Create the database and tables, and create the `CheckDailyAvailability` stored procedure shown above.

3. Configure the connection string. It is not committed to `appsettings.json`; configure it via user secrets:
   ```bash
   dotnet user-secrets init --project Blood-Donation-Management-System
   dotnet user-secrets set --project Blood-Donation-Management-System "ConnectionStrings:DefaultConnection" "Server=.;Database=BDMS;Trusted_Connection=True;TrustServerCertificate=True"
   ```
   (Alternatively, add a `ConnectionStrings:DefaultConnection` entry to `appsettings.json`.)

4. Seed a `SystemAdmin` row before logging into the admin panel (password must be a BCrypt hash, e.g. generated with `BCrypt.Net.BCrypt.HashPassword("yourpassword")`).

5. Run the app:
   ```bash
   dotnet run --project Blood-Donation-Management-System
   ```

### Default routes

| URL                  | Purpose                            |
|----------------------|------------------------------------|
| `/`                  | Landing page + booking modal       |
| `/Auth/Login`        | Admin login                        |
| `/Auth/AccessDenied` | Access-denied page                 |
| `/Dashboard`         | Admin dashboard (requires login)   |

### Launch profiles

| Profile       | URL                                   |
|---------------|---------------------------------------|
| `http`        | `http://localhost:5196`               |
| `https`       | `https://localhost:7126` / `http://localhost:5196` |
| `IIS Express` | `http://localhost:29556` (SSL `44343`)|

## Notes

- The dashboard currently renders demo/hard-coded statistics; wiring them to the database is a planned next step (`DashboardController` is minimal and the onboarding controller `BookingController`'s `Index` view is empty).
- `BookingController` exposes `GET /Booking/CheckAvailability` and `POST /Booking/CreateBooking`, which the booking modal consumes via `wwwroot/js/Booking.js`.