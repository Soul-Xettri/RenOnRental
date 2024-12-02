using Microsoft.AspNetCore.Identity;
using RevOnRental.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class Role : IdentityRole<int>, IBaseEntity
    {
        public ICollection<UserRole> UserRoles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
