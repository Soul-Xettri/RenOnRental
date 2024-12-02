using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Notifications.Command;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Command
{
    public class ConfirmRentalCommand : IRequest<NotificationDto>
    {
        public int RentalId { get; set; }
        public PaymentType PaymentType { get; set; }
    }

    public class ConfirmRentalHandler : IRequestHandler<ConfirmRentalCommand, NotificationDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public ConfirmRentalHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<NotificationDto> Handle(ConfirmRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals
                .FirstOrDefaultAsync(r => r.Id == request.RentalId, cancellationToken);

            if (rental == null)
                throw new ArgumentException("Rental not found.");

            if (rental.RentalStatus != RentalStatusType.Accepted)
                throw new InvalidOperationException("Rental cannot be confirmed in its current state.");

            if (request.PaymentType == PaymentType.Online)
            {
                rental.RentalStatus = RentalStatusType.Confirmed;
                rental.UpdatedDate = DateTime.Now;
                _context.Rentals.Update(rental);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var notification = new CreateNotificationCommand
            {
                UserId = rental.UserID,
                VehicleId = rental.VehicleId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                Type = NotificationType.PaymentSuccessful
            };
            var result= await _mediator.Send(notification, cancellationToken);

            return result;
        }
    }

}
