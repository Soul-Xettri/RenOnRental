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
    public class Business : IBaseEntity
    {
        public int Id { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessRegistrationNumber { get; set; }
        public BusinessType BusinessType { get; set; }
        public bool IsApproved { get; set; }
        public string? VehiclePlateNumber  { get; set; }
        public int TotalRating { get; set; }
        public decimal AverageRating { get; set; }
        public virtual UserBusiness UserBusiness { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ReviewRating> Reviews { get; set; }
        public virtual ICollection<BusinessDocument> BusinessDocuments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
