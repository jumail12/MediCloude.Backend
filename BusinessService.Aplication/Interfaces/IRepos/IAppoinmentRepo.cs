using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface IAppoinmentRepo
    {
        Task<Guid> AddAppoinment(Appointment appointment);
        Task<List<Appointment>> GetAppoinmentListByDrId(Guid drid);
        Task<List<Appointment>> GetAppoinmentByDrId(Guid drid);
        Task<Appointment> GetBy_APId(Guid id);
        Task<List<Appointment>> GetByPatientID(Guid patientID);
        Task<bool> SaveAsync();
        Task<List<Appointment>> GetByPatientID_Prescription(Guid patientID);
        Task<List<Appointment>> GetAppointments_Patient(Guid pId);
        Task<List<Appointment>> GetAll();
    }
}
