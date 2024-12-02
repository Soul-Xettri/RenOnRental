using RevOnRental.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Queries
{
    public class RentalDto
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string RentalStatus { get; set; }
        public PhotoDto Photo { get; set; }
    }
}
