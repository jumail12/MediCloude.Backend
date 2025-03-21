using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Domain.Entities
{
    public class Specialization_doctor : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Specialization Required")]
        public string Category { get; set; }
    }
}
