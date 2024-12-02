using RevOnRental.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Interfaces
{
    public interface IJwtService
    {
        ClaimsIdentity GenerateClaimsIdentity(ClaimDto claimDto);

        Task<AuthenticationOutputDto> GenerateJwt(AppUserDto userDetails);
    }
}
