using Microsoft.AspNetCore.Identity;
using RevOnRental.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class User : IdentityUser<int>, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        { get { return (this.FirstName + " " + this.LastName).Trim(); } }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual UserBusiness UserBusiness { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } 
        public virtual ICollection<ReviewRating> Reviews { get; set; }
        public DateTime CreatedDate { get ; set ; }
        public DateTime UpdatedDate { get; set; }
    }
}
