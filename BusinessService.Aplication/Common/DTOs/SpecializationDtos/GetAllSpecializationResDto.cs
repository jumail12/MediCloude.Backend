using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.SpecializationDtos
{
    public class GetAllSpecializationResDto
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
    }
}
