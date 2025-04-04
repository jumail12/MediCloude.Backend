

namespace BusinessService.Aplication.Common.DTOs.Appoinment
{
    public record AppoinmentPatientPaginationResDto
    {
        public int total_pages { get; set; }
        public List<PatientAppResDto>? items { get; set; }
    }
}
