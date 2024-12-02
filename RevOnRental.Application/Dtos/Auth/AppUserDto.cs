using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.Auth
{
    public class AppUserDto : ClaimDto
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
