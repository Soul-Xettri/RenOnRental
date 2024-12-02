using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Vehicles.Command
{
    public class UpdateVehicleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int NumberOfVehicle { get; set; }
        public RentalChargeDto RentalCharges { get; set; }

        public bool AvailabilityStatus { get; set; }
    }

    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
    {
        private readonly IAppDbContext _context;

        public UpdateVehicleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var existingVehicle = await _context.Vehicles.Include(x=>x.RentalCharges).FirstOrDefaultAsync(v => v.Id == request.Id);

            if (existingVehicle != null)
            {
                existingVehicle.Model = request.Model;
                existingVehicle.Brand = request.Brand;
                existingVehicle.VehicleType = request.VehicleType;
                existingVehicle.AvailabilityStatus = request.AvailabilityStatus;
                existingVehicle.NumberOfVehicle = request.NumberOfVehicle;
                if (request.RentalCharges != null)
                {
                    existingVehicle.RentalCharges.HourlyRate = request.RentalCharges.HourlyRate;
                    existingVehicle.RentalCharges.HalfDayRate = request.RentalCharges.HalfDayRate;
                    existingVehicle.RentalCharges.FullDayRate = request.RentalCharges.FullDayRate;
                }
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
