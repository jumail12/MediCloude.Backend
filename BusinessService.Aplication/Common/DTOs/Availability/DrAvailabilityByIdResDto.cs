using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Availability
{
    public class DrAvailabilityByIdResDto
    {
        public DateTime AppointmentDate { get; set; } // Store full date
        public List<DrAvailabiliyTimeSlotDto> AppointmentTimes { get; set; }
    }

    public class DrAvailabiliyTimeSlotDto
    {
        public Guid Id { get; set; }
        public string AppointmentTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
