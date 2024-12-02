using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class VehicleAvailabilityDto
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public double? Rating { get; set; }
        public double? Distance { get; set; }
        public string? RentalTimeCategory { get; set; }


    }
}
