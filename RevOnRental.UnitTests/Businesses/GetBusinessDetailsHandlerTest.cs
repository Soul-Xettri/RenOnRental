using Microsoft.EntityFrameworkCore;
using Moq;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Businesses.Queries;
using RevOnRental.Domain.Models;
using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.UnitTests.Businesses
{
    [TestFixture]
    public class GetBusinessDetailsHandlerTests
    {
        private Mock<IAppDbContext> _dbContextMock;
        private GetBusinessDetailsHandler _handler;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<IAppDbContext>();

            // Instantiate the handler with the mocked context
            _handler = new GetBusinessDetailsHandler(_dbContextMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnBusinessDetails_WhenBusinessExists()
        {
            // Arrange
            var businessId = 2;
            var businessData = new List<Business>
            {
                new Business
                {
                    Id = businessId,
                    BusinessName = "Test Business",
                    BusinessType = (BusinessType)1,
                    BusinessRegistrationNumber = "12345",
                    UserBusiness = new List<UserBusiness>
                    {
                        new UserBusiness
                        {
                            User = new User
                            {
                                ContactNumber = "1234567890"
                            }
                        }
                    }
                }
            }.AsQueryable();

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businessData.Provider);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businessData.Expression);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businessData.ElementType);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businessData.GetEnumerator());

            _dbContextMock.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);

            // Act
            var result = await _handler.Handle(new GetBusinessDetailsQuery { BusinessId = businessId }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(businessId, result.Id);
            Assert.AreEqual("Test Business", result.BusinessName);
            Assert.AreEqual("1234567890", result.PhoneNumber);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyDto_WhenBusinessDoesNotExist()
        {
            // Arrange
            var businessData = new List<Business>().AsQueryable();

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businessData.Provider);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businessData.Expression);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businessData.ElementType);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businessData.GetEnumerator());

            _dbContextMock.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);

            // Act
            var result = await _handler.Handle(new GetBusinessDetailsQuery { BusinessId = 1 }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id); // Assuming 0 is the default value for non-existent entities.
            Assert.IsEmpty(result.BusinessName);
        }
    }
}
