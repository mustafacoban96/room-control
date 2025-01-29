using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Dto.User
{
    public class RegisterDto
    {
        [Required]
        [Unicode(false)]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        //[Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public List<RoleDto> Roles { get; set; }

    }
}