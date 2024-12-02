using MediatR;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Notifications.Command
{
    public class ChangeIsNewCommand : IRequest<bool>
    {
        public int? UserId { get; set; }
        public int? BusinessId { get; set; }

    }

    public class ChangeIsNewCommandHandler : IRequestHandler<ChangeIsNewCommand, bool>
    {
        private readonly IAppDbContext _context;

        public ChangeIsNewCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ChangeIsNewCommand request, CancellationToken cancellationToken)
        {
            if (request.BusinessId == null && request.UserId == null)
                throw new ArgumentException("Either BusinessId or UserId must be provided.");

            var notificationsQuery = new List<Notification>();

            if (request.BusinessId.HasValue)
                notificationsQuery = _context.Notifications.Where(n => n.BusinessId == request.BusinessId.Value && n.IsNew).ToList();

            if (request.UserId.HasValue)
                notificationsQuery = _context.Notifications.Where(n => n.UserId == request.UserId.Value && n.IsNew).ToList(); ;

            var finalList= new List<Notification>();
            foreach (var item in notificationsQuery)
            {
                item.IsNew = false;
                finalList.Add(item);    
            }                

            if (finalList == null)
                throw new ArgumentException("Notification not found or does not belong to the specified UserId/BusinessId.");

           
            _context.Notifications.UpdateRange(finalList);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
