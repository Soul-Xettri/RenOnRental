using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RevOnRental.Application.Dtos.Auth;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain;
using RevOnRental.Domain.Constants;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Infrastructure.Identity.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public ClaimsIdentity GenerateClaimsIdentity(ClaimDto claimDto)
        {
            try
            {
                return new ClaimsIdentity(new GenericIdentity(claimDto.Email, "Token"), AddToClaimList(claimDto));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// serializes encoded token with expiry to json string
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AuthenticationOutputDto> GenerateJwt(AppUserDto userDetails)
        {
            try
            {
                
                var authToken = await GenerateEncodedToken(userDetails.ClaimsIdentity);
                return new AuthenticationOutputDto
                {
                    AccessToken = authToken,
                    ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GenerateEncodedToken(ClaimsIdentity identity)
        {
            string email = identity.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
            var claimList = new List<Claim>()
            {
                identity.FindFirst(AuthConstants.JwtId),
                identity.FindFirst(AuthConstants.Role),
                identity.FindFirst(AuthConstants.FullName),
                new Claim(JwtRegisteredClaimNames.Sub,email),
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Jti,await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,_jwtOptions.IssuedAt.ToString(), ClaimValueTypes.Integer64),
            };

            // Add BusinessId claim only if it exists
            var businessIdClaim = identity.FindFirst(AuthConstants.BusinessId);
            if (businessIdClaim != null)
            {
                claimList.Add(businessIdClaim);
            }

            //Create JWT security token and encode it
            var jwt = new JwtSecurityToken(
                 issuer: _jwtOptions.Issuer,
                 audience: _jwtOptions.Audience,
                 claims: claimList,
                 notBefore: _jwtOptions.NotBefore,
                 expires: _jwtOptions.Expiration,
                 signingCredentials: _jwtOptions.SigningCredentials
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private static IEnumerable<Claim> AddToClaimList(ClaimDto claimDto)
        {
            yield return new Claim(AuthConstants.JwtId, claimDto.Id.ToString());
            yield return new Claim(ClaimTypes.Email, claimDto.Email);
            //yield return new Claim(AuthConstants.IsAdmin, claimDto.IsAdmin.ToString().ToLower());
            yield return new Claim(AuthConstants.Role, claimDto.Role);
            if (claimDto.BusinessId.HasValue)
            {
                yield return new Claim(AuthConstants.BusinessId, claimDto.BusinessId.Value.ToString());
            }
            yield return new Claim(AuthConstants.FullName, claimDto.FullName);
        }

        /// <summary>
        /// Throws exceptions for invalid JwtIssuerOptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null) throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}
    
