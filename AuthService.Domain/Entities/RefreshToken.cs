using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class RefreshToken : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Refresh_token { get; set; }
        [Required]
        public Guid userId { get; set; }
        [Required]
        public DateTime Expires { get; set; }
    }
}
