using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.Prescription
{
    public record PrescriptionAddCommand : IRequest<string>
    {
        [Required]
        public Guid AppId { get; set; }
        [Required]
        [MaxLength(1000)]
        public string PrescriptionText {  get; set; }
    }
}
