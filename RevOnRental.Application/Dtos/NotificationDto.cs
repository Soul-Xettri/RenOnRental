using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public MiscDto Misc { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedDate { get; set; }
    }
}
