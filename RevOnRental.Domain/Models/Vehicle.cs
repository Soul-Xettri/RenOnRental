using Microsoft.AspNetCore.Http;
using RevOnRental.Domain.Common;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class Vehicle : IBaseEntity
    {
        public int Id { get; set; }
        public int BusinessID { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int NumberOfVehicle { get; set; }
        public int NumberOfAvailableVehicle { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; } // Store file content as byte array
        public DateTime UploadedDate { get; set; }
        public bool AvailabilityStatus { get; set; } // True if available
        public Business Business { get; set; }
        public RentalCharge RentalCharges { get; set; }
        public ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
