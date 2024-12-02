using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; } // Or Username if your system uses that
        public string Password { get; set; }
    }
}
