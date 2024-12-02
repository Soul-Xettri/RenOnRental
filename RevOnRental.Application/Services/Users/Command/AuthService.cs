using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Dtos.Auth;
using RevOnRental.Application.Exceptions;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Businesses.Command;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Users.Command
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IAppDbContext appDbContext,
            IJwtService jwtService, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
            _jwtService = jwtService;
            _mediator = mediator;
        }

        // User Registration
        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);

                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Email already exists" });
                }
                var user = new User
                {
                    UserName = registerUserDto.Email,
                    Email = registerUserDto.Email,
                    FirstName = registerUserDto.FirstName,
                    LastName = registerUserDto.LastName,
                    ContactNumber = registerUserDto.ContactNumber,
                    Address = registerUserDto.Address,
                    Latitude=registerUserDto.Latitude,
                    Longitude=registerUserDto.Longitude,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerUserDto.Password);
               
                if (result.Succeeded)
                {
                  await _userManager.AddToRoleAsync(user, "User");
                }

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        // Business Registration
        public async Task<IdentityResult> RegisterBusinessAsync(RegisterBusinessDto registerBusinessDto)
        {
            var user = new User
            {
                UserName = registerBusinessDto.Email,
                Email = registerBusinessDto.Email,
                FirstName = registerBusinessDto.FirstName,
                LastName = registerBusinessDto.LastName,
                ContactNumber = registerBusinessDto.ContactNumber,
                Address = registerBusinessDto.Address,
                Latitude = registerBusinessDto.Latitude,
                Longitude = registerBusinessDto.Longitude
            };

            var result = await _userManager.CreateAsync(user, registerBusinessDto.Password);
            if (result.Succeeded)
            {
                var getUser=await _userManager.FindByEmailAsync(registerBusinessDto.Email);
                await _userManager.AddToRoleAsync(user, "Business");

                var business = new Business
                {
                    BusinessName = registerBusinessDto.BusinessName,
                    BusinessRegistrationNumber = registerBusinessDto.BusinessRegistrationNumber,
                    BusinessType = registerBusinessDto.BusinessType

                };
                var ent = await _appDbContext.Businesses.AddAsync(business);
                await _appDbContext.SaveChangesAsync();
                var documents = new AddBusinessDocumentCommand
                {
                    Bluebook = registerBusinessDto.Bluebook,
                    BusinessId = business.Id,
                    BusinessRegistrationDocument = registerBusinessDto.BusinessRegistrationDocument,
                    NationalIdBack = registerBusinessDto.NationalIdBack,
                    NationalIdFront = registerBusinessDto.NationalIdFront,
                };
                await _mediator.Send(documents);
                



                var userBusiness = new UserBusiness
                {
                     BusinessId=business.Id,
                     UserId= getUser.Id
                };
                await _appDbContext.UserBusiness.AddAsync(userBusiness);

                await _appDbContext.SaveChangesAsync();

            }

            return result;
        }

        // Login Method
        public async Task<AuthenticationOutputDto> LoginAsync(LoginDto loginDto)
        {
            try
            {

                var claimsIdentity = await GetClaimsIdentityAsync(loginDto.Email, loginDto.Password);
                var authenticationOutput = await _jwtService.GenerateJwt(claimsIdentity);

                return authenticationOutput;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Verifies user credentials and returns claim list asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AppUserDto> GetClaimsIdentityAsync(string email, string password)
        {
            try
            {
                User appUser = await _userManager.FindByEmailAsync(email);
                if (appUser is null)
                {
                    throw new CustomException($"No Accounts Registered with {email}.");
                }
                else
                {
                    return await VerifyUserNamePasswordAsync(password, appUser, new AppUserDto { Id = appUser.Id, Email = appUser.Email});
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifies username and password and returns claim list asynchronously.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userToVerify"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private async Task<AppUserDto> VerifyUserNamePasswordAsync(string password, User userToVerify, AppUserDto userDetails)
        {
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var userBusiness =  _appDbContext.UserBusiness
                                    .FirstOrDefault(ub => ub.UserId == userToVerify.Id);
               


                // Fetch roles for the user
                var roles = (await _userManager.GetRolesAsync(userToVerify)).FirstOrDefault();
                //var userRole =  _appDbContext.UserRoles.FirstOrDefault(x => x.UserId == userToVerify.Id);
                var claimDTO = new ClaimDto
                {
                    Id = userToVerify.Id,
                    Email = userToVerify.Email,
                    //IsAdmin = await _userQueryService.CheckUserAdminAsync(userToVerify.Id),
                    Role = roles,
                    FullName = userToVerify.FullName
                };

                if (userBusiness != null)
                {
                    claimDTO.BusinessId=userBusiness.BusinessId;    
                }
                userDetails.ClaimsIdentity = await Task.FromResult(_jwtService.GenerateClaimsIdentity(claimDTO));
                return userDetails;
            }

            throw new CustomException("Invalid email or password.");
        }


        public async Task<bool> CreateRole(RoleDto roleDto)
        {
            var role = new Role
            {
                Name = roleDto.Name,
            };
            await _appDbContext.Roles.AddAsync(role);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RoleDto>> GelAllRole()
        {
            
            var roles= _appDbContext.Roles.Select(x=>new RoleDto
            {
                 Id = x.Id,
                 Name=x.Name
            }).ToList();
            return roles;
        }
        public async Task<RoleDto> GetRoleById(int Id)
        {

            var role =await  _appDbContext.Roles.Where(x=>x.Id==Id).Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name
            }).FirstOrDefaultAsync();
            return role;
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update user information
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.ContactNumber = updateUserDto.ContactNumber ?? user.ContactNumber;
            user.Address = updateUserDto.Address ?? user.Address;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }

            return true;
        }

    }
}
