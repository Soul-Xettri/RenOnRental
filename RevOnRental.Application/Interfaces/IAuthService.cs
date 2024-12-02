using Microsoft.AspNetCore.Identity;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<IdentityResult> RegisterBusinessAsync(RegisterBusinessDto registerBusinessDto);
        Task<AuthenticationOutputDto> LoginAsync(LoginDto loginDto);
        Task<bool> CreateRole(RoleDto roleDto);
        Task<List<RoleDto>> GelAllRole();
        Task<bool> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
        
    }
}
