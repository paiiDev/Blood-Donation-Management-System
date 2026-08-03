using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Shared.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = ("Username is required."))]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = ("Password is required."))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
