using BusinessService.Aplication.Interfaces.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.Notification.Handler
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, string>
    {
        private readonly INotificationService _notservice;
        public DeleteNotificationCommandHandler(INotificationService notservice)
        {
            _notservice = notservice;
        }
        public async Task<string> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _notservice.GetById(request.notId, request.authid, request.recip_type);

                if (notification == null)
                {
                    throw new Exception("Not exists");
                }
                notification.Is_deleted= true;
                await _notservice.SaveAsync();

                return "Notification deleted";
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
