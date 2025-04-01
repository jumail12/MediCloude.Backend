
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Availability
{
    public class AvailabilityByIdResDto
    {
        public Guid Id { get; set; }
        public string AppointmentTime { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime AppointmentDate { get; set; } // Store full date
    }
}
