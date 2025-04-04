using BusinessService.Aplication.Interfaces.IServices;
using BusinessService.Domain.Entities;
using BusinessService.Infrastructure.Notifications;
using BusinessService.Infrastructure.Persistance;
using Contarcts.Common.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace BusinessService.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BusinessDbContext _businessDbContext;
        private readonly IHubContext<NotHub> _hubContext;
        public NotificationService(BusinessDbContext businessDbContext,IHubContext<NotHub> hubContext)
        {
            _businessDbContext = businessDbContext;
            _hubContext = hubContext;
        }

        public async Task<bool> AppoinmentCreatedNotifyDr(Notification notification, NotificationResDto notificationResDto)
        {
            try
            {
                await _businessDbContext.Notifications.AddAsync(notification);
                await _businessDbContext.SaveChangesAsync();

                notificationResDto.Sender_Profile=notification.Sender_Profile;

                await _hubContext.Clients.Group(notification.Recipient_id.ToString())
                  .SendAsync("receivenotificationdr", notificationResDto);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<bool> AppoinmetStartPatientAlert(Notification notification, NotificationResDto notificationResDto)
        {
            try
            {
                await _businessDbContext.Notifications.AddAsync(notification);
                await _businessDbContext.SaveChangesAsync();

                await _hubContext.Clients.Group(notification.Recipient_id.ToString())
               .SendAsync("receivenotificationpatient", notificationResDto);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<List<Notification>> GetNotificationBYId(Guid id, string recip_type)
        {
            try
            {
                var res = await _businessDbContext.Notifications.Where(a => !a.Is_deleted && a.Recipient_id == id && a.Recipient_type == recip_type)
                    .ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<Notification> GetById(Guid id, Guid authid, string recip_type)
        {
            try
            {
                var res = await _businessDbContext.Notifications
                    .FirstOrDefaultAsync(a => !a.Is_deleted && a.Recipient_type == recip_type && a.Recipient_id == authid && a.Id == id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _businessDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
