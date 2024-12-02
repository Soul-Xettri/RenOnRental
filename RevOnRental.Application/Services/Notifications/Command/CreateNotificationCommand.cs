using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Rating.Command;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Notifications.Command
{
    public class CreateNotificationCommand : IRequest<NotificationDto>
    {
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public NotificationType Type { get; set; }
    }
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
    {
        private readonly IAppDbContext _context;

        public CreateNotificationHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var vehicleDet = await _context.Vehicles.Include(x => x.Business).Where(x => x.Id == request.VehicleId).Select(x => new MiscDto
            {
                Vehicle = new BaseDto { Id = x.Id, Name = x.Brand + " " + x.Model },
                Business = new BaseDto { Id = x.Business.Id, Name = x.Business.BusinessName },

            }).FirstOrDefaultAsync();
            var userDet = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            vehicleDet.User = new BaseDto { Id = userDet.Id, Name = userDet.FullName };

            var notification = new Notification
            {
                Type = request.Type,
                CreatedDate = DateTime.Now,
                IsRead = false,
                IsNew=true,
                Misc = JsonConvert.SerializeObject(vehicleDet)
            };

            if (request.Type == NotificationType.RentalRequest)
            {
                notification.BusinessId = vehicleDet.Business.Id;
                notification.Message = $"{vehicleDet.User.Name} is trying to rent {vehicleDet.Vehicle.Name} from {request.StartDate} to {request.EndDate}";
            }
            else if (request.Type == NotificationType.RentalRejected)
            {
                notification.UserId = vehicleDet.User.Id;
                notification.Message = $"Your request for renting {vehicleDet.Vehicle.Name} from {request.StartDate} to {request.EndDate} has been rejected";
            }
            else if (request.Type == NotificationType.PaymentRequest)
            {
                notification.UserId = vehicleDet.User.Id;
                notification.Message = $"Your request for renting {vehicleDet.Vehicle.Name} from {request.StartDate} to {request.EndDate} has been accepted. Please proceed for payment";
            }
            else if (request.Type == NotificationType.PaymentSuccessful)
            {
                notification.BusinessId = vehicleDet.Business.Id;
                notification.Message = $"{vehicleDet.Vehicle.Name}  has been paid for by user. ";
            }
            else if (request.Type == NotificationType.RatingBusiness)
            {
                notification.UserId = vehicleDet.User.Id;
                notification.Message = $"Please rate your experience with {vehicleDet.Business.Name} for your recent rental of {vehicleDet.Vehicle.Name}.";
            }



            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return new NotificationDto
            {
                Id = notification.Id,
                CreatedDate = notification.CreatedDate,
                Message = notification.Message,
                Misc = vehicleDet,
                NotificationType= notification.Type
            };
        }

       
    }

}

