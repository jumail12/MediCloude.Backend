
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Domain.Entities
{
    public  class Notification : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Message { get; set; }

        [Required]
        public string? Recipient_type { get; set; }  //Doctor,Patient,Admin

        [Required]
        public Guid? Recipient_id { get; set; }

        [Required]
        public Guid? Sender_id { get; set; }

        [Required]
        public string? Sender_Name { get; set; }

        public string? Sender_Profile { get; set; } = "https://img.freepik.com/free-psd/contact-icon-illustration-isolated_23-2151903337.jpg?t=st=1743520252~exp=1743523852~hmac=43887a21f00a05efff8d8f44a5d50cb7236688eeb67f5b39752ea20bdf96c029&w=826";

        public bool IsRead { get; set; } = false;
    }
}
