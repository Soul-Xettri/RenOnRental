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

namespace RevOnRental.Application.Services.Businesses.Queries
{
    public class SearchAvailableVehiclesQuery : IRequest<List<VehicleAvailabilityDto>>
    {
        public VehicleType VehicleType { get; set; }
        public string CurrentAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DestinationAddress { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        
    }


    public class SearchAvailableVehiclesHandler : IRequestHandler<SearchAvailableVehiclesQuery, List<VehicleAvailabilityDto>>
    {
        private readonly IAppDbContext _context;

        public SearchAvailableVehiclesHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleAvailabilityDto>> Handle(SearchAvailableVehiclesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Calculate rental duration (hourly, half-day, full-day)
                var rentalDuration = (request.EndDateTime - request.StartDateTime).TotalHours;
                var rentalType = rentalDuration < 6 ? "Hourly" :
                                 rentalDuration == 6 ? "HalfDay" : "FullDay";

                // Fetch available vehicles of the specified type
                var vehicles = await _context.Vehicles
                    .Include(v => v.Business)
                    .ThenInclude(x => x.UserBusiness).ThenInclude(x => x.User)
                    .Include(v => v.Business)
                    .ThenInclude(b => b.Reviews)
                    .Where(v => v.VehicleType == request.VehicleType && v.AvailabilityStatus)
                    
                    .ToListAsync(cancellationToken);

                if (vehicles.Count > 0)
                {
                    var groupedBusinesses = vehicles
                        .GroupBy(v => v.Business.Id)
                        .Select(g => g.First())
                        .ToList();
                    var result = groupedBusinesses.Select(v => new VehicleAvailabilityDto
                    {   
                        BusinessId = v.Business.Id,
                        BusinessName = v.Business.BusinessName,
                        Distance = v.Business != null ? DistanceCalculator.GetDistance(
                        request.Latitude, request.Longitude,
                        v.Business.UserBusiness.User.Latitude, v.Business.UserBusiness.User.Longitude) : 0,
                        Rating = (v.Business!=null && v.Business.Reviews!=null && v.Business.Reviews.Count>0) ? (int)Math.Round(v.Business.Reviews.Average(r => r.Rating)) : 0,
                        RentalTimeCategory=rentalType
                        
                    }).OrderBy(z => z.Distance).ToList();
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

       
    }
}
