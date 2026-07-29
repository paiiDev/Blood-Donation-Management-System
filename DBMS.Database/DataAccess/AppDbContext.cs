using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBMS.Database.DataAccess;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<BloodInventory> BloodInventories { get; set; }

    public virtual DbSet<BloodType> BloodTypes { get; set; }

    public virtual DbSet<DonationCenter> DonationCenters { get; set; }

    public virtual DbSet<DonationRecord> DonationRecords { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<SystemAdmin> SystemAdmins { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.BookingNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TimeSlot).HasMaxLength(50);

            entity.HasOne(d => d.Center).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CenterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Center_ID_Appointments_CenterId");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Appointment)
                .HasForeignKey<Appointment>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donors_Id_Appointments _DonorId");
        });

        modelBuilder.Entity<BloodInventory>(entity =>
        {
            entity.ToTable("BloodInventory");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.BoodGroup).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.BoodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BloodTypes_Id_BloodInventory_BloodGroupId");

            entity.HasOne(d => d.DonationRecord).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.DonationRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonationRecords_Id_BloodInventory_DonationRecordId");
        });

        modelBuilder.Entity<BloodType>(entity =>
        {
            entity.Property(e => e.GroupName).HasMaxLength(10);
        });

        modelBuilder.Entity<DonationCenter>(entity =>
        {
            entity.Property(e => e.CenterName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
        });

        modelBuilder.Entity<DonationRecord>(entity =>
        {
            entity.Property(e => e.DonationDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Appiontment).WithMany(p => p.DonationRecords)
                .HasForeignKey(d => d.AppiontmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Apoit_Id_DonationRecords_APoitId");

            entity.HasOne(d => d.BloodGroupTypeNavigation).WithMany(p => p.DonationRecords)
                .HasForeignKey(d => d.BloodGroupType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BloodTypes_Id_DonationRecords_BoodGroupType");

            entity.HasOne(d => d.Donor).WithMany(p => p.DonationRecords)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donors_Id_DonationRecords_DonorId");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Doners");

            entity.HasIndex(e => e.Id, "IX_Donors").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<SystemAdmin>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Center).WithMany(p => p.SystemAdmins)
                .HasForeignKey(d => d.CenterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonationCenters_Id_SystemAdmins_CenterId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
