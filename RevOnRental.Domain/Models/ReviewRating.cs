using RevOnRental.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class ReviewRating : IBaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }      // Reference to the User who gave the review
        public int BusinessId { get; set; }  // Reference to the Business being reviewed
        public int Rating { get; set; }      
        public string Review { get; set; }   
        public DateTime ReviewDate { get; set; }

        public User User { get; set; }       
        public Business Business { get; set; }
        public DateTime CreatedDate { get ; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
