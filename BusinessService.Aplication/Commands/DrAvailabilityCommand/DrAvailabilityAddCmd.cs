using BusinessService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace BusinessService.Aplication.Commands.DrAvailabilityCommand
{
    public record DrAvailabilityAddCmd() : IRequest<string>
    {

        [EnumDataType(typeof(Day), ErrorMessage = "Invalid day selection.")]
        public Day? AppointmentDay { get; set; }

        [Required(ErrorMessage = "Appointment time is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$", ErrorMessage = "Invalid time format. Use 'hh:mm AM/PM'")]
        public string AppointmentTime { get; set; }  
    }

     public record DrAvailabilityAddCmdWithDrId(DrAvailabilityAddCmd cmd,Guid DrId) : IRequest<string>;
}
