using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Notifications.Command;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.RentalBooking.Command
{
    public class CreateRentalCommand : IRequest<NotificationDto> // Return the rental ID upon success
    {
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        
    }
    public class CreateRentalHandler : IRequestHandler<CreateRentalCommand, NotificationDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;

        public CreateRentalHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;   
        }

        public async Task<NotificationDto> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the vehicle availability
                var vehicle = await _context.Vehicles
                    .Include(v => v.Rentals).Include(x => x.RentalCharges)
                    .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken);

                if (vehicle == null)
                {
                    throw new ArgumentException("Vehicle not found.");
                }

                if (!vehicle.AvailabilityStatus)
                {
                    throw new InvalidOperationException("Vehicle is not available for rent.");
                }

                //// Check if the vehicle is already rented during the specified time period
                //bool isVehicleBooked = vehicle.Rentals.Any(r =>
                //    r.StartDate < request.EndDateTime && r.EndDate > request.StartDateTime);

                //if (isVehicleBooked)
                //{
                //    throw new InvalidOperationException("Vehicle is already booked for the selected time period.");
                //}

                // Calculate rental duration and price
                var totalDays = request.EndDateTime - request.StartDateTime;
                TimeSpan rentalDuration = request.EndDateTime - request.StartDateTime;
                decimal totalPrice;

                // Example pricing logic based on duration
                if (rentalDuration.TotalHours < 6)
                {
                    totalPrice = (decimal)vehicle.RentalCharges.HourlyRate * (decimal)rentalDuration.TotalHours;
                }
                else if (rentalDuration.TotalHours == 6)
                {
                    totalPrice = (decimal)vehicle.RentalCharges.HalfDayRate;
                }
                else
                {
                    totalPrice = (decimal)vehicle.RentalCharges.FullDayRate * (decimal)rentalDuration.TotalDays;
                }

                // Create the rental record
                var rental = new Rental
                {
                    UserID = request.UserId,
                    VehicleId = request.VehicleId,
                    StartDate = request.StartDateTime,
                    EndDate = request.EndDateTime,
                    TotalPrice = totalPrice,
                    RentalStatus = RentalStatusType.Pending, // Default status can be changed to "Active" after confirmation
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                // Save the rental record to the database
                await _context.Rentals.AddAsync(rental, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // Optionally update the vehicle's availability status
                vehicle.NumberOfAvailableVehicle = vehicle.NumberOfAvailableVehicle - 1;
                vehicle.AvailabilityStatus = vehicle.NumberOfAvailableVehicle == 0 ? false : true;
                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync(cancellationToken);

                //call create notification command to save notification
                var notification = new CreateNotificationCommand
                {
                    UserId = request.UserId,
                    VehicleId = request.VehicleId,
                    StartDate = request.StartDateTime,
                    EndDate = request.EndDateTime,
                    Type = NotificationType.RentalRequest
                };
                var res=await _mediator.Send(notification);

                // Return the rental ID
                return res;
            }
            catch (Exception ex)
            {

                throw;
            } 

           
        }
    }
}
