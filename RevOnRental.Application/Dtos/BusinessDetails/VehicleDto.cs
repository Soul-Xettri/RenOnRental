using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.BusinessDetails
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public bool AvailabilityStatus { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal HalfDayRate { get; set; }
        public decimal FullDayRate { get; set; }
        public PhotoDto Photo { get; set; }
    }
}
