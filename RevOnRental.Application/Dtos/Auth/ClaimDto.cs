using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.Auth
{
    public class ClaimDto
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }
        public string FullName { get; set; }
    }
}
