using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.BusinessDetails
{
    public class BusinessDetailsDto
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public double AverageRating { get; set; }
        public List<VehicleDto> AvailableVehicles { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
