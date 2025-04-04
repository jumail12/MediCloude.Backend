using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contarcts.Common.DTOs
{
    public class NotificationResDto
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public Guid? Recipient_id { get; set; }
        public string? Sender_Name { get; set; }
        public string? Sender_Profile { get; set; }
    }
}
