using Microsoft.AspNetCore.Http;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class RegisterBusinessDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessRegistrationNumber { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public IFormFile? NationalIdFront { get; set; }
        public IFormFile? NationalIdBack { get; set; }
        public IFormFile? Bluebook { get; set; }
        public IFormFile? BusinessRegistrationDocument { get; set; }
        public BusinessType BusinessType { get; set; }
    }
}
