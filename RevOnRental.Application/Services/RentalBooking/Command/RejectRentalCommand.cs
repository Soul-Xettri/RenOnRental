using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Notifications.Command;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Command
{
    public class RejectRentalCommand : IRequest<NotificationDto>
    {
        public int RentalId { get; set; }
    }

    public class RejectRentalHandler : IRequestHandler<RejectRentalCommand, NotificationDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public RejectRentalHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<NotificationDto> Handle(RejectRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve rental from database
                var rental = await _context.Rentals
                    .Include(r => r.Vehicle)
                    .FirstOrDefaultAsync(r => r.Id == request.RentalId, cancellationToken);

                if (rental == null)
                    throw new ArgumentException("Rental not found.");

                if (rental.RentalStatus != RentalStatusType.Pending)
                    throw new InvalidOperationException("Rental cannot be rejected in its current state.");

                // Update rental status
                rental.RentalStatus = RentalStatusType.Cancelled;
                rental.UpdatedDate = DateTime.Now;
                _context.Rentals.Update(rental);
                await _context.SaveChangesAsync(cancellationToken);

                // Trigger a notification for rejection
                var notificationCommand = new CreateNotificationCommand
                {
                    UserId = rental.UserID,
                    VehicleId = rental.VehicleId,
                    StartDate = rental.StartDate,
                    EndDate = rental.EndDate,
                    Type = NotificationType.RentalRejected
                };
               var res= await _mediator.Send(notificationCommand, cancellationToken);

                // Optionally restore the vehicle's availability
                var vehicle = rental.Vehicle;
                vehicle.NumberOfAvailableVehicle += 1;
                vehicle.AvailabilityStatus = true;
                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync(cancellationToken);

                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
