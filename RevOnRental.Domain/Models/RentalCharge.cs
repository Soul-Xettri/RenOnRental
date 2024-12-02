using RevOnRental.Domain.Common;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class RentalCharge : IBaseEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public float HourlyRate { get; set; }
        public float HalfDayRate { get; set; }
        public float FullDayRate { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime CreatedDate { get ; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
