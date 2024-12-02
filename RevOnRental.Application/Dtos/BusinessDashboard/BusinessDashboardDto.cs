using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos.BusinessDashboard
{
    public class BusinessDashboardDto
    {
        public int TotalVehicles { get; set; }
        public int TotalAvailableVehicles { get; set; }
        public List<VehicleTypeSummaryDto> VehicleTypeSummaries { get; set; }
    }
}
