using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Surname { get; set; }

        public string ProfileImg { get; set; }
        public IFormFile file { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfrimPassword { get; set; }

        public string PhoneNumber { get; set; }

    }
}
