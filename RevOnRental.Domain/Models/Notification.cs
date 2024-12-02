using RevOnRental.Domain.Common;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class Notification : IBaseEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? BusinessId { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public bool IsRead { get; set; } = false; // Whether the notification has been read
        public string Misc { get; set; }
        public NotificationType Type { get; set; } // Enum for categorizing notifications
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    
}
