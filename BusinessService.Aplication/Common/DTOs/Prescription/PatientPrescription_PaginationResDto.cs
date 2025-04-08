using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Prescription
{
    public class PatientPrescription_PaginationResDto
    {
        public int total_pages { get; set; }
        public List<PatientPrescriptionResDto> items {  get; set; }

    }
}
