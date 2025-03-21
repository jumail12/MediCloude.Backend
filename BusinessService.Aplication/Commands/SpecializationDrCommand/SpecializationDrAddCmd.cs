using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.SpecializationDrCommand
{
    public record SpecializationDrAddCmd : IRequest<string>
    {
        [Required(ErrorMessage = "Specialization Required")]
        public string Category { get; set; }
    }
}
