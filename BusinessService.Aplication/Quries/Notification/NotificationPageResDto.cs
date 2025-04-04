using BusinessService.Aplication.Common.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Notification
{
    public class NotificationPageResDto
    {
        public int total_pages { get; set; }
        public List<NotificationResApiDto> items { get; set; }
    }
}
