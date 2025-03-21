using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Domain.Entities
{
    public class AuditableEntity
    {
        public DateTime Created_on { get; set; }
        public DateTime? Updated_on { get; set; }
        public string? Created_by { get; set; }
        public string? Updated_by { get; set; }
        public bool Is_deleted { get; set; } = false;
    }
}
