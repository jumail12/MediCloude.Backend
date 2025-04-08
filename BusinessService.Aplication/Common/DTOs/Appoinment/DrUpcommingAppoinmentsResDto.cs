using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Appoinment
{
    public class DrUpcommingAppoinmentsResDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public Guid PatientId { get; set; }
        public string? Patient_name { get; set; }
        public string Email { get; set; }
        public Guid? RoomId { get; set; }
        public string? AppoinmentStatus { get; set; }
    }
}
