using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Vehicles.Queries
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int NumberOfVehicle { get; set; }

        public bool AvailabilityStatus { get; set; }
    }
}
