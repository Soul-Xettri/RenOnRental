using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class UserBusiness
    {
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public User User { get; set; }
        public Business Business { get; set; }

   
    }
}
