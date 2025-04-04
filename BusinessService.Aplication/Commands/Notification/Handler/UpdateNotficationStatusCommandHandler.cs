using BusinessService.Aplication.Interfaces.IServices;
using MediatR;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.Notification.Handler
{
    public class UpdateNotficationStatusCommandHandler : IRequestHandler<UpdateNotficationStatusCommand, string>
    {
        private readonly INotificationService _notservice;
        public UpdateNotficationStatusCommandHandler(INotificationService notservice)
        {
            _notservice = notservice;
        }

        public async Task<string> Handle(UpdateNotficationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _notservice.GetById(request.notId, request.authid, request.recip_type);
             
                if (notification == null)
                {
                    throw new Exception("Not exists");
                }
                notification.IsRead = true;
                await _notservice.SaveAsync();

                return "Status updated";
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
