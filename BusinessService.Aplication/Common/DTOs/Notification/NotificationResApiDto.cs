using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Notification
{
    public class NotificationResApiDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Sender_Name { get; set; }
        public string? Sender_Profile { get; set; }
        public Guid? Sender_id { get; set; }
        public DateTime Created_on { get; set; }
        public bool IsRead { get; set; }
    }
}
