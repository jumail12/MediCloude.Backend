
using System.ComponentModel.DataAnnotations;
using static Contarcts.Common.CommonContarct;


namespace AuthService.Domain.Entities
{
    public class Doctor : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Enter your name!")]
        [MaxLength(25)]
        public string? Doctor_name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string? Qualification { get; set; }

        [Required(ErrorMessage = "Enter a strong password!")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
        public Guid? Specialization_id { get; set; }
        public string? Medical_license_number { get; set; }

        public bool Is_approved { get; set; } = false;
        public bool Is_blocked { get; set; } = false;

        public decimal? Drfee { get; set; } = 500;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }

        public string? About { get; set; }
        public string? Profile { get; set; }
        public GenderD?  Gender { get; set; }

        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public double? Field_experience { get; set; }

        public List<string>? SearchString { get; set; }
        
    }


}
