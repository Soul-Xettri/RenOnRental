//using RevOnRental.Application.Interfaces;
//using RevOnRental.Application.Services.Businesses.Queries;
//using RevOnRental.Domain.Models;
//using RevOnRental.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RevOnRental.Application.Test.Businesses.Queries
//{

//    [TestFixture]
//    public class GetBusinessDetailsQueryTests
//    {
//            private readonly IAppDbContext _dbContext;
//        private QueryTestFixture _fixture;
//        public GetBusinessDetailsQueryTests()
//            {
//            _fixture = new QueryTestFixture();
//            _dbContext = _fixture.Context;
//            }

//            [Test]
//            public async Task Handle_ReturnsCorrectDto()
//            {
//                var query = new GetBusinessDetailsQuery
//                {
//                    BusinessId = 1
//                };

//                var handler = new GetBusinessDetailsHandler(_dbContext);
//                var result = await handler.Handle(query, CancellationToken.None);
               

//                Assert.IsNotNull(result);
//                Assert.AreEqual(query.BusinessId, result.Id);
//                Assert.That(result, Is.TypeOf<BusinessDto>());

//            }

//        [Test]
//        public async Task Handle_GivenInvalidId_ThrowsException()
//        {
//            var query = new GetBusinessDetailsQuery
//            {
//                BusinessId = 5
//            };

//            var handler = new GetBusinessDetailsHandler(_dbContext);
            
//            Assert.ThrowsAsync<NullReferenceException>(async () => await handler.Handle(query, CancellationToken.None));


//        }


//    }
//}
