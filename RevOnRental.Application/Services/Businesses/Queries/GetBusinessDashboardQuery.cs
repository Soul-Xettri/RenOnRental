using MediatR;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Application.Dtos.BusinessDashboard;
using RevOnRental.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Services.Businesses.Queries
{
    public class GetBusinessDashboardQuery : IRequest<BusinessDashboardDto>
    {
        public int BusinessId { get; set; }

    }

    public class GetBusinessDashboardHandler : IRequestHandler<GetBusinessDashboardQuery, BusinessDashboardDto>
    {
        private readonly IAppDbContext _context;

        public GetBusinessDashboardHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessDashboardDto> Handle(GetBusinessDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all vehicles for the specified business
                var vehicles = await _context.Vehicles
                    .Where(v => v.BusinessID == request.BusinessId)
                    .ToListAsync(cancellationToken);

                if (vehicles == null || vehicles.Count == 0)
                {
                    return new BusinessDashboardDto
                    {
                        TotalVehicles = 0,
                        TotalAvailableVehicles = 0,
                        VehicleTypeSummaries = new List<VehicleTypeSummaryDto>()
                    };
                }

                // Calculate total vehicles and total available vehicles
                int totalVehicles = vehicles.Sum(v => v.NumberOfVehicle);
                int totalAvailableVehicles = vehicles
                    .Sum(v => v.NumberOfAvailableVehicle);

                // Group vehicles by type and calculate the total and available quantities
                var vehicleTypeSummaries = vehicles
                    .GroupBy(v => v.VehicleType.ToString())
                    .Select(group => new VehicleTypeSummaryDto
                    {
                        VehicleType = group.Key,
                        TotalQuantity = group.Sum(v => v.NumberOfVehicle),
                        AvailableQuantity = group.Sum(v => v.NumberOfAvailableVehicle)
                    })
                    .ToList();

                // Prepare the dashboard summary DTO
                var dashboardDto = new BusinessDashboardDto
                {
                    TotalVehicles = totalVehicles,
                    TotalAvailableVehicles = totalAvailableVehicles,
                    VehicleTypeSummaries = vehicleTypeSummaries
                };

                return dashboardDto;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
