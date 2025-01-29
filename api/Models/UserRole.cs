using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class UserRole
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; }  // Navigation property

        [Key]
        public int RoleId { get; set; }
        public Role Role { get; set; }  // Navigation property
    }
}