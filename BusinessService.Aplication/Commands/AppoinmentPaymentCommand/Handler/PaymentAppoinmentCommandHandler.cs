using BusinessService.Aplication.Common.DTOs.AppoinmentPaymentDtos;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using Razorpay.Api;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage; 

namespace BusinessService.Aplication.Commands.AppoinmentPaymentCommand.Handler
{
    public class PaymentAppoinmentCommandHandler : IRequestHandler<PaymentAppoinmentCommand, string>
    {
        private readonly IPaymentAppoinmentRepo _repo;
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;

        public PaymentAppoinmentCommandHandler(IRequestClient<PatientByIdRequest> requestClient, IDrAvailabilityRepo rAvailabilityRepo, IPaymentAppoinmentRepo repo)
        {
            _requestClient = requestClient;
            _rAvailabilityRepo = rAvailabilityRepo;
            _repo = repo;
        }

        public async Task<string> Handle(PaymentAppoinmentCommand request, CancellationToken cancellationToken)
        {
                try
                {
                var RabbitMqRes = await _requestClient.GetResponse<PatientByIdResponse>(new PatientByIdRequest(request.PatientId));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with RabbitMQ.");
                }

                var patient = RabbitMqRes.Message;

                if (patient == null)
                {
                    throw new Exception("Patient not found.");
                }

                var slot=await _rAvailabilityRepo.GetBySlotId(request.DrAvailabilityId);
                if(!slot.IsAvailable)
                {
                    throw new ValidationException("This slot is sold out");
                }

                var newAppoinment = new Appointment
                {
                    DrId = slot.DrId,
                    DrAvailabilityId=slot.Id,
                    PatientId= patient.Patient_id,
                    Status=AppointmentStatus.Confirmed,
                    RoomId=RoomIdForVideocall(),
                    Created_by=patient.Patient_name,
                    Updated_by=patient.Patient_name,
                    Created_on=DateTime.UtcNow,
                    Updated_on=DateTime.UtcNow,
                };

               

                var paymentData = new Domain.Entities.Payment
                {
                    DoctorId=slot.DrId,
                    PatientId = patient.Patient_id,
                    TransactionId=request.TrasactionId,
                    PaymentMethod=request.PaymentMethod,
                    PaymentStatus=Status.Success,
                    DoctorAmount = request.Amount * 0.7m,
                    AdminCommissionAmount = request.Amount * 0.3m,
                    Created_by = patient.Patient_name,
                    Updated_by = patient.Patient_name,
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                };

               
                await _repo.PaymentAppoinmentGateway(newAppoinment, paymentData,request.RazorPaymentCredential);


                return "Your appoinment is booked successfully..!";

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

     
        private Guid RoomIdForVideocall()
        {
            return Guid.NewGuid();
        }

    

    }
}
