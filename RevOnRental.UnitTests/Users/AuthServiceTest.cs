using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Application.Services.Users.Command;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.UnitTests.Users
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<IAppDbContext> _appDbContextMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            // Mock UserManager and SignInManager dependencies
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _appDbContextMock = new Mock<IAppDbContext>();

            // Instantiate AuthService with the mocked dependencies
            _authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object, _appDbContextMock.Object);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerUserDto = new RegisterUserDto
            {
                Email = "testuser@test.com",
                FirstName = "Subina",
                LastName = "Khadka",
                ContactNumber = "1234567890",
                Address = "Pokhara",
                Password = "Password@123!"
            };

            var user = new User
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                ContactNumber = registerUserDto.ContactNumber,
                Address = registerUserDto.Address
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterUserAsync(registerUserDto);

            // Assert
            Assert.IsTrue(result.Succeeded);
            _userManagerMock.Verify(x => x.CreateAsync(It.Is<User>(u => u.Email == registerUserDto.Email), registerUserDto.Password), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.Is<User>(u => u.Email == registerUserDto.Email), "User"), Times.Once);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldReturnFailure_WhenUserCreationFails()
        {
            // Arrange
            var registerUserDto = new RegisterUserDto
            {
                Email = "testuser@test.com",
                Password = "Password@123!"
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            // Act
            var result = await _authService.RegisterUserAsync(registerUserDto);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("User creation failed", result.Errors.First().Description);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }
    }
}
