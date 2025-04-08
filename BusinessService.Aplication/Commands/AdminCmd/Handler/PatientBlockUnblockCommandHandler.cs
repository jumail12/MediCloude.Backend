using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.AdminCmd.Handler
{
    public class PatientBlockUnblockCommandHandler : IRequestHandler<PatientBlockUnblockCommand, string>
    {
        private readonly IRequestClient<PatientBlockUnblockReq> _requestClient;
        public PatientBlockUnblockCommandHandler(IRequestClient<PatientBlockUnblockReq> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<string> Handle(PatientBlockUnblockCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request == null)
                {
                    throw new Exception("Patient id is not be null here");
                }

                var RabbitRes = await _requestClient.GetResponse<PatientBlockUnblockResponse>(new PatientBlockUnblockReq(request.id));
                if(RabbitRes == null)
                {
                    throw new Exception("error making connection with rabbitmq");
                }

                var res= RabbitRes.Message.response;
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
