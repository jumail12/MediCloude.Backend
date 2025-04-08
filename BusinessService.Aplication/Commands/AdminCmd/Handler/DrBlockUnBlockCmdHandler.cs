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

namespace BusinessService.Aplication.Commands.AdminCmd.Handler
{
    internal class DrBlockUnBlockCmdHandler : IRequestHandler<DrBlockUnBlockCmd, string>
    {
        private readonly IRequestClient<DrBlockUnBlockReq> _requestClient;
        public DrBlockUnBlockCmdHandler(IRequestClient<DrBlockUnBlockReq> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<string> Handle(DrBlockUnBlockCmd request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new Exception("Doctor id is not be null here");
                }

                var RabbitRes = await _requestClient.GetResponse<DrBlockUnBlockResponse>(new DrBlockUnBlockReq(request.drid));
                if (RabbitRes == null)
                {
                    throw new Exception("error making connection with rabbitmq");
                }

                var res = RabbitRes.Message.response;
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
