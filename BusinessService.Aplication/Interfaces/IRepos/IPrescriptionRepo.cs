using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface IPrescriptionRepo
    {
        Task<bool> AddPrescription(Prescription prescription);
        Task<List<Prescription>> GetPrescriptionList_Patient(Guid patientid);
    }
}
