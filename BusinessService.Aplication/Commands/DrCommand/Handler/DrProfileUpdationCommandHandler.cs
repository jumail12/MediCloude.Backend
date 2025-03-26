using BusinessService.Aplication.Interfaces.IServices;
using Contarcts.Common;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MassTransit.Middleware;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace BusinessService.Aplication.Commands.DrCommand.Handler
{
    public class DrProfileUpdationCommandHandler : IRequestHandler<DrProfileUpdationCommand, string>
    {
        private readonly IRequestClient<DrByIdReq> _requestClient;
        private readonly IRequestClient<DrProfileUpdationReq> _requestClient2;
        private readonly ICloudinaryService _cludinaryService;
        public DrProfileUpdationCommandHandler(IRequestClient<DrByIdReq> requestClient,ICloudinaryService cloudinaryService, IRequestClient<DrProfileUpdationReq> requestClient2)
        {
            _requestClient = requestClient;
            _cludinaryService = cloudinaryService;
            _requestClient2 = requestClient2;
        }

        public async Task<string> Handle(DrProfileUpdationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(request.drId.GetValueOrDefault(Guid.Empty)));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbitMqRes.Message;

                if(doctor == null)
                {
                    throw new Exception("dr not found");
                }


                if(request.Profile!=null && request.Profile.Length > 0)
                {
                    var resUrl = request.Profile != null ? await _cludinaryService.UploadProfileImageAsync(request.Profile) : null;
                    doctor.Profile= resUrl!=null ? resUrl : doctor.Profile;
                }

                var UpdationReq = new DrProfileUpdationReq
                {
                    drId = request.drId.GetValueOrDefault(Guid.Empty),
                    Phone = request.Phone != null ? request.Phone.ToString() : doctor.Phone,
                    About = request.About != null ? request.About.ToString() : doctor.About,
                    Gender=request.Gender.HasValue ? request.Gender.Value.ToString(): doctor.Gender,
                    Field_experience = request.Field_experience > 0 ? request.Field_experience : doctor.Field_experience,
                    Qualification= request.Qualification != null ? request.Qualification.ToString() : doctor.Qualification,
                    Profile=doctor.Profile
                };

                var RabbitMqUpdationResponse = await _requestClient2.GetResponse<DrProfileUpdationResponse>(UpdationReq);

                if (RabbitMqUpdationResponse.Message == null)
                {
                    throw new Exception("Error making connection with rabbit mq");
                }

                if (RabbitMqUpdationResponse.Message.Success)
                {
                    return "Profile updation success";
                }

                return "Profile updation failed, try again!";

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
