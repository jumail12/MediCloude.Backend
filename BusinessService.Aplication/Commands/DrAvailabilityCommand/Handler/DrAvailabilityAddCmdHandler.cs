using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.DrAvailabilityCommand.Handler
{
    public class DrAvailabilityAddCmdHandler : IRequestHandler<DrAvailabilityAddCmdWithDrId, string>
    {
        private readonly IDrAvailabilityRepo _repo;
        private readonly IRequestClient<DrByIdReq> _requestClient;

        public DrAvailabilityAddCmdHandler(IDrAvailabilityRepo repo, IRequestClient<DrByIdReq> requestClient)
        {
            _repo = repo;
            _requestClient = requestClient;
        }

        public async Task<string> Handle(DrAvailabilityAddCmdWithDrId request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(request.DrId));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with RabbitMQ.");
                }

                var doctor = RabbitMqRes.Message;

                if (doctor == null)
                {
                    throw new Exception("Doctor not found.");
                }

                if (!request.cmd.AppointmentDate.HasValue)
                {
                    throw new ValidationException("Appointment date is required.");
                }

                DateTime appointmentDate = request.cmd.AppointmentDate.Value;

                if (!DateTime.TryParseExact(request.cmd.AppointmentTime, "hh:mm tt",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    throw new ValidationException("Invalid time format. Use 'hh:mm AM/PM'.");
                }

                TimeSpan parsedTime = parsedDateTime.TimeOfDay;

                var isExists = await _repo.isExists(doctor.Id, appointmentDate, parsedTime);
                if (isExists)
                {
                    throw new ValidationException("This slot is already added.");
                }

                var newSlot = new DrAvailability
                {
                    DrId = doctor.Id,
                    AppointmentDate = appointmentDate,
                    AppointmentTime = parsedTime,
                    Created_by = doctor.Doctor_name,
                    Updated_by = doctor.Doctor_name,
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                };

                await _repo.Add(newSlot);
                return "New slot added successfully.";
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
