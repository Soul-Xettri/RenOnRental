using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Constants
{
    public static class AuthConstants
    {
        public const string JwtId = "id", AuthKey = "JWT_PRIVATE_KEY", IsAdmin = "isAdmin";
        public const string UserName = "username", FullName = "fullname",Role="role",BusinessId = "businessId";

    }
}
