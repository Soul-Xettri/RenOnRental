using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Notifications.Queries
{
    public class GetAllNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public int? BusinessId { get; set; }
        public int? UserId { get; set; }
    }

    public class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsQuery, List<NotificationDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllNotificationsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            if (request.BusinessId == null && request.UserId == null)
                throw new ArgumentException("Either BusinessId or UserId must be provided.");

            var notificationsQuery = new List<Notification>();

            if (request.BusinessId.HasValue)
                notificationsQuery = _context.Notifications.Where(n => n.BusinessId == request.BusinessId.Value).ToList();

            if (request.UserId.HasValue)
                notificationsQuery = _context.Notifications.Where(n => n.UserId == request.UserId.Value).ToList(); ;

            var notifications =  notificationsQuery
                .OrderByDescending(n => n.CreatedDate) // Sort by date (latest first)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    CreatedDate = n.CreatedDate,
                    Message = n.Message,
                    Misc = JsonConvert.DeserializeObject<MiscDto>(n.Misc),
                    NotificationType = n.Type,
                    IsRead = n.IsRead
                }).ToList();

            return notifications;
        }
    }
}
