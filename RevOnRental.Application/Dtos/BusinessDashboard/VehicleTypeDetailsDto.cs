using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.BusinessDashboard
{
    public class VehicleTypeDetailsDto
    {   public int id { get; set; }
        public int VehicleType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool AvailabilityStatus { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal HalfDayRate { get; set; }
        public decimal FullDayRate { get; set; }
        public PhotoDto Photo { get; set; }
    }
}
