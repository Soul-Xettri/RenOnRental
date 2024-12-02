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
    public class CompleteRentalCommand : IRequest<NotificationDto>
    {
        public int RentalId { get; set; }
    }

    public class CompleteRentalHandler : IRequestHandler<CompleteRentalCommand, NotificationDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public CompleteRentalHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<NotificationDto> Handle(CompleteRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == request.RentalId, cancellationToken);

            if (rental == null)
                throw new ArgumentException("Rental not found.");


            if (rental.RentalStatus != RentalStatusType.Confirmed)
                throw new InvalidOperationException("Rental status is not set to completed.");

            var vehicle = rental.Vehicle;
            vehicle.NumberOfAvailableVehicle += 1;
            vehicle.AvailabilityStatus = true;
            _context.Vehicles.Update(vehicle);

            rental.RentalStatus = RentalStatusType.Completed;
            rental.UpdatedDate = DateTime.Now;
            _context.Rentals.Update(rental);

            await _context.SaveChangesAsync(cancellationToken);

            var notification = new CreateNotificationCommand
            {
                UserId = rental.UserID,
                VehicleId = rental.VehicleId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                Type = NotificationType.RatingBusiness
            };
           var res= await _mediator.Send(notification, cancellationToken);

            return res;
        }
    }

}
