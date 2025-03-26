using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


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
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbitMqRes.Message;

                if (doctor == null)
                {
                    throw new Exception("dr not found");
                }


                if (!DateTime.TryParseExact(request.cmd.AppointmentTime, "hh:mm tt",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime parsedDateTime))
                {
                    throw new ValidationException("Invalid time format. Use 'hh:mm AM/PM'");
                }

                TimeSpan parsedTime = parsedDateTime.TimeOfDay;

                var dayString = request.cmd.AppointmentDay.HasValue ? request.cmd.AppointmentDay.Value.ToString() : null;

                if (string.IsNullOrEmpty(dayString) || !Enum.TryParse<Day>(dayString, true, out var parsedDay))
                {
                    throw new ValidationException("Invalid appointment day. Use values like 'Monday', 'Tuesday', etc.");
                }

                var isExits= await _repo.isExists(doctor.Id,parsedDay,parsedTime);
                if (isExits)
                {
                    throw new ValidationException("This slot is already added");
                }

                var newSlot = new DrAvailability
                {
                    DrId = doctor.Id,
                    AppointmentDay =parsedDay,
                    AppointmentTime=parsedTime,
                    Created_by=doctor.Doctor_name,
                    Updated_by=doctor.Doctor_name,
                    Created_on=DateTime.UtcNow,
                    Updated_on=DateTime.UtcNow,
                };

                await _repo.Add(newSlot);
                return "New slot added successfully";
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
