using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Users.Queries
{
    public class GetUserDetailsQuery : IRequest<UserDto>
    {
        public int UserId { get; set; }
    }

    public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsQuery, UserDto>
    {
        private readonly IAppDbContext _context;

        public GetUserDetailsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                Address = user.Address,
            };
        }
    }
}
