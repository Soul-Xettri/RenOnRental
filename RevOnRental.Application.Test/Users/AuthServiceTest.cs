//using MediatR;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Moq;
//using RevOnRental.Application.Dtos;
//using RevOnRental.Application.Interfaces;
//using RevOnRental.Application.Services.Users.Command;
//using RevOnRental.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RevOnRental.Application.Test.Users
//{
//    [TestFixture]
//    [TestFixture]
//    public class AuthServiceTests
//    {
//        private Mock<UserManager<User>> _userManagerMock;
//        private Mock<SignInManager<User>> _signInManagerMock;
//        private Mock<IAppDbContext> _appDbContextMock;
//        private AuthService _authService;
//        private Mock<IJwtService> _jwtServiceMock;
//        private Mock<IMediator> _mediatorMock;

//        [SetUp]
//        public void Setup()
//        {
//            // Mock the IUserStore<User> dependency required by UserManager<User>
//            var userStoreMock = new Mock<IUserStore<User>>();

//            //// Mock UserManager<User> and pass valid mocks for required dependencies
//            //_userManagerMock = new Mock<UserManager<User>>(
//            //    userStoreMock.Object,
//            //    Mock.Of<IOptions<IdentityOptions>>(),
//            //    Mock.Of<IPasswordHasher<User>>(),
//            //    Array.Empty<IUserValidator<User>>(),
//            //    Array.Empty<IPasswordValidator<User>>(),
//            //    Mock.Of<ILookupNormalizer>(),
//            //    Mock.Of<IdentityErrorDescriber>(),
//            //    Mock.Of<IServiceProvider>(),
//            //    Mock.Of<ILogger<UserManager<User>>>()
//            //);

//            //// Mock SignInManager<User> with valid dependencies
//            //_signInManagerMock = new Mock<SignInManager<User>>(
//            //    _userManagerMock.Object,
//            //    Mock.Of<IHttpContextAccessor>(),
//            //    Mock.Of<IUserClaimsPrincipalFactory<User>>(),
//            //    Mock.Of<IOptions<IdentityOptions>>(),
//            //    Mock.Of<ILogger<SignInManager<User>>>(),
//            //    Mock.Of<IAuthenticationSchemeProvider>(),
//            //    Mock.Of<IUserConfirmation<User>>()
//            //);

//            //// Mock the application database context
//            //_appDbContextMock = new Mock<IAppDbContext>();
//            //_jwtServiceMock = new Mock<IJwtService>();
//            //_mediatorMock= new Mock<IMediator>();
//            //// Instantiate AuthService with the mocked dependencies
//            //_authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object, _appDbContextMock.Object, _jwtServiceMock.Object, _mediatorMock.Object);
//        }

//        [Test]
//        public async Task RegisterUserAsync_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
//        {
//            // Arrange
//            var registerUserDto = new RegisterUserDto
//            {
//                Email = "testuser@test.com",
//                FirstName = "Subina",
//                LastName = "Khadka",
//                ContactNumber = "1234567890",
//                Address = "Pokhara",
//                Password = "Password@123!"
//            };

//            // Set up the mocked UserManager to return IdentityResult.Success
//            _userManagerMock
//                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
//                .ReturnsAsync(IdentityResult.Success);

//            _userManagerMock
//                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
//                .ReturnsAsync(IdentityResult.Success);

//            // Act
//            var result = await _authService.RegisterUserAsync(registerUserDto);

//            // Assert
//            Assert.IsTrue(result.Succeeded);
//            _userManagerMock.Verify(x => x.CreateAsync(It.Is<User>(u => u.Email == registerUserDto.Email), registerUserDto.Password), Times.Once);
//            _userManagerMock.Verify(x => x.AddToRoleAsync(It.Is<User>(u => u.Email == registerUserDto.Email), "User"), Times.Once);
//        }


//        // Test for failed registration due to an existing email
//        [Test]
//        public async Task RegisterUserAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
//        {
//            // Arrange
//            var registerUserDto = new RegisterUserDto
//            {
//                Email = "testuser@test.com",
//                FirstName = "Subina",
//                LastName = "Khadka",
//                ContactNumber = "1234567890",
//                Address = "Pokhara",
//                Password = "Password@123!"
//            };

//            var user = new User
//            {
//                UserName = registerUserDto.Email,
//                Email = registerUserDto.Email,
//                FirstName = registerUserDto.FirstName,
//                LastName = registerUserDto.LastName,
//                ContactNumber = registerUserDto.ContactNumber,
//                Address = registerUserDto.Address
//            };

//            // Simulate that a user with the same email already exists
//            _userManagerMock
//                .Setup(x => x.FindByEmailAsync(registerUserDto.Email))
//                .ReturnsAsync(user); // A user with this email already exists

//            // Act
//            var result = await _authService.RegisterUserAsync(registerUserDto);

//            // Assert
//            Assert.IsFalse(result.Succeeded, "Registration should fail if the email already exists.");
//            Assert.AreEqual("Email already exists", result.Errors.First().Description);
//            _userManagerMock.Verify(x => x.FindByEmailAsync(registerUserDto.Email), Times.Once); // Verify FindByEmailAsync was called
//            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never); // Creation should not be attempted
//        }
//    }
//}
