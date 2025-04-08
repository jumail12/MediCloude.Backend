using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Doctor;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Aplication.Quries.Admin.Handler
{
    public class AdminDoctorByIdQueryHandler : IRequestHandler<AdminDoctorByIdQuery, AdminDoctorByIdResDto>
    {
        private readonly IRequestClient<DrByIdReq> _requestClient;
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ISpecializationRepo _SpecializationRepo;
  

        public AdminDoctorByIdQueryHandler(IRequestClient<DrByIdReq> requestClient, IAppoinmentRepo appoinmentRepo, IPaymentRepo paymentRepo,ISpecializationRepo specializationRepo)
        {
            _requestClient = requestClient;
            _appoinmentRepo = appoinmentRepo;
            _paymentRepo = paymentRepo;
            _SpecializationRepo = specializationRepo;
        }

        public async Task<AdminDoctorByIdResDto> Handle(AdminDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(request.drid));
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbitMqRes.Message;

                var appoinmnets= await _appoinmentRepo.GetAppoinmentByDrId(doctor.Id);
                var payments= await _paymentRepo.GetAllPaymentDetailsByDrId(doctor.Id);
                var specialization = await _SpecializationRepo.GetAllSplAsync();
                var splDict=specialization.ToDictionary(a=>a.Id,b=>b);

                var toatalAppoinmnets = appoinmnets.Count();
                var appoinment_pending = appoinmnets.Where(a => a.Status == AppointmentStatus.Confirmed).Count();
                var appoinmnet_completed = appoinmnets.Where(a => a.Status == AppointmentStatus.Success).Count();
                var adminProfit = payments.Sum(a => a.AdminCommissionAmount);

                var res = new AdminDoctorByIdResDto()
                {
                    Id = doctor.Id,
                    IsBlocked = doctor.IsBlocked,
                    Doctor_name=doctor.Doctor_name,
                    Phone = doctor.Phone,
                    Profile = doctor.Profile,
                    Email = doctor.Email,
                    Field_experience=doctor.Field_experience,
                    Gender = doctor.Gender.ToString(),
                    Qualification = doctor.Qualification,
                    toatal_appoinments_pending=appoinment_pending,
                    toatal_appoinments_completed=appoinmnet_completed,
                    your_profit=adminProfit,
                    total_appoinments_taken=toatalAppoinmnets,
                    Specialization=splDict.TryGetValue((Guid)doctor.Specialization_id, out var spl)? spl.Category : null,
                };

                return res;

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
