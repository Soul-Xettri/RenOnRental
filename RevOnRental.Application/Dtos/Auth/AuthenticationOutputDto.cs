using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.Auth
{
    public class AuthenticationOutputDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
