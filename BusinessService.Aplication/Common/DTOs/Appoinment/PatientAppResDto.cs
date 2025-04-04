using System;


namespace BusinessService.Aplication.Common.DTOs.Appoinment
{
    public class PatientAppResDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public Guid? RoomId { get; set; }
        public string? Doctor_name { get; set; }
        public string Email { get; set; }
        public string? Qualification { get; set; }
        public string? Phone { get; set; }
        public string? Profile { get; set; }
        public string? Gender { get; set; }
        public double? Field_experience { get; set; }
        public string? Specialization { get; set; }
    }
}
