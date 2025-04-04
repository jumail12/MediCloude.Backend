using BusinessService.Domain.Entities;
using Contarcts.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IServices
{
    public interface INotificationService
    {
        Task<bool> AppoinmentCreatedNotifyDr(Notification notification,NotificationResDto notificationResDto);
        Task<List<Notification>> GetNotificationBYId(Guid id,string recip_type);
        Task<Notification> GetById(Guid id, Guid authid,string recip_type);
        Task<bool> SaveAsync();

        Task<bool> AppoinmetStartPatientAlert(Notification notification,NotificationResDto notificationResDto);
    }
}
