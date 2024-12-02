using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class UserBookingHistoryDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int VehicleId { get; set; }
        public string Model { get; set;}
        public string Brand { get; set; }
        public string BusinessName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VehicleType VehicleType { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public PhotoDto Photo { get; set; }
    }
}
