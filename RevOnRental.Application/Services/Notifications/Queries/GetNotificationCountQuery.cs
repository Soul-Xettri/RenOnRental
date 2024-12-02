using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Notifications.Queries
{
    public class GetNotificationCountQuery : IRequest<int>
    {
        public int? BusinessId { get; set; }
        public int? UserId { get; set; }
    }

    public class GetNotificationCountHandler : IRequestHandler<GetNotificationCountQuery, int>
    {
        private readonly IAppDbContext _context;

        public GetNotificationCountHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetNotificationCountQuery request, CancellationToken cancellationToken)
        {
            if (request.BusinessId == null && request.UserId == null)
                throw new ArgumentException("Either BusinessId or UserId must be provided.");


            if (request.BusinessId.HasValue)
                return _context.Notifications.Where(n => n.BusinessId == request.BusinessId.Value && !n.IsNew).Count();

            if (request.UserId.HasValue)
                return _context.Notifications.Where(n => n.UserId == request.UserId.Value && !n.IsNew).Count();

            return 0;
        }
    }
}
