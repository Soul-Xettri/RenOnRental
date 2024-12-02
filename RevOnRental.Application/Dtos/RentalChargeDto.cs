using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class RentalChargeDto
    {
        public float HourlyRate { get; set; }
        public float HalfDayRate { get; set; }
        public float FullDayRate { get; set; }
    }
}
