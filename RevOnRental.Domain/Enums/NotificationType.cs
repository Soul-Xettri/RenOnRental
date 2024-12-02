using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Enums
{
    public enum NotificationType
    {
        RentalRequest = 1,
        PaymentRequest = 2,
        RentalRejected = 3,
        PaymentSuccessful = 4,
        RatingBusiness = 5
    }
}
