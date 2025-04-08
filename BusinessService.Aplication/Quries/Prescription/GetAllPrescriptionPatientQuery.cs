using BusinessService.Aplication.Common.DTOs.Prescription;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Prescription
{
    public record GetAllPrescriptionPatientQuery(Guid patientid,int pageNumber,int pageSize) : IRequest<PatientPrescription_PaginationResDto>;
}
