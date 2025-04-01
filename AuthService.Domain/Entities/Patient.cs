using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace AuthService.Domain.Entities
{
    public class Patient : AuditableEntity
    {
        [Key]
        public Guid Patient_id { get; set; }

        [Required(ErrorMessage = "Enter your name!")]
        [MaxLength(25)]
        public string? Patient_name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a strong password!")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        public DateTime? Dob { get; set; }
        [Phone]
        public string? Phone { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        public bool Is_blocked { get; set; } = false;

        public GenderP? Gender { get; set; }
    }

   
}

