using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Notifications.Command
{
    public class MarkAsReadCommand : IRequest<bool>
    {
        public int NotificationId { get; set; }
    }

    public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
    {
        private readonly IAppDbContext _context;

        public MarkAsReadHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n =>
                    n.Id == request.NotificationId,
                    cancellationToken);

            if (notification == null)
                throw new ArgumentException("Notification not found or does not belong to the specified UserId/BusinessId.");

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }


    //public class MarkAllAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
    //{
    //    private readonly IAppDbContext _context;

    //    public MarkAllAsReadHandler(IAppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    //    {
    //        if (request.UserId == null && request.BusinessId == null)
    //            throw new ArgumentException("Either UserId or BusinessId must be provided.");

    //        var notificationsQuery = _context.Notifications.AsQueryable();

    //        if (request.UserId.HasValue)
    //        {
    //            notificationsQuery = notificationsQuery.Where(n => n.UserId == request.UserId.Value && !n.IsRead);
    //        }
    //        else if (request.BusinessId.HasValue)
    //        {
    //            notificationsQuery = notificationsQuery.Where(n => n.BusinessId == request.BusinessId.Value && !n.IsRead);
    //        }

    //        var unreadNotifications = await notificationsQuery.ToListAsync(cancellationToken);

    //        if (unreadNotifications.Count == 0)
    //            return false; // No unread notifications to mark as read

    //        foreach (var notification in unreadNotifications)
    //        {
    //            notification.IsRead = true;
    //        }

    //        _context.Notifications.UpdateRange(unreadNotifications);
    //        await _context.SaveChangesAsync(cancellationToken);

    //        return true;
    //    }
    //}

}