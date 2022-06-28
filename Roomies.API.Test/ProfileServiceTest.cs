using FluentAssertions;
using Moq;
using NUnit.Framework;
using Roomies.API.Domain.Models;
using Roomies.API.Domain.Persistence.Repositories;
using Roomies.API.Domain.Repositories;
using Roomies.API.Domain.Services.Communications;
using Roomies.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomies.API.Test
{
    public class ProfileServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoProfileReturnsEmptyCollection()
        {
            // Arrange

            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultIUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockProfileRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Profile>());

            var service = new ProfileService(mockProfileRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);

            // Act

            List<Profile> result = (List<Profile>)await service.ListAsync();
            var profileCount = result.Count;

            // Assert

            profileCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByPlanIdAsyncWhenNoProfileReturnsEmptyCollection()
        {
            // Arrange

            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultIUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var planId = 1;

            mockProfileRepository.Setup(r => r.ListByPlanId(planId)).ReturnsAsync(new List<Profile>());

            var service = new ProfileService(mockProfileRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);

            // Act

            List<Profile> result = (List<Profile>)await service.ListByPlanIdAsync(planId);
            var profileCount = result.Count;

            // Assert

            profileCount.Should().Equals(0);
        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsCategoryNotFoundResponse()
        {
            // Arrange
            
            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultIUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            
            var profileId = 1;
            
            Profile profile = new Profile();
            mockProfileRepository.Setup(r => r.FindById(profileId)).Returns(Task.FromResult<Profile>(null));
            var service = new ProfileService(mockProfileRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);

            // Act
            ProfileResponse result = await service.GetByIdAsync(profileId);
            var message = result.Message;

            // Assert
            message.Should().Be("Usuario inexistente");
        }

        [Test]
        public async Task GetByUserIdAsyncWhenInvalidIdReturnsCategoryNotFoundResponse()
        {
            // Arrange

            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultIUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            var userId = 1;

            Profile profile = new Profile();
            mockProfileRepository.Setup(r => r.FindByUserId(userId)).Returns(Task.FromResult<Profile>(null));
            var service = new ProfileService(mockProfileRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);

            // Act
            ProfileResponse result = await service.GetByUserIdAsync(userId);
            var message = result.Message;

            // Assert
            message.Should().Be("Usuario inexistente");
        }

        private static Mock<IProfileRepository> GetDefaultIProfileRepositoryInstance()
        {
            return new Mock<IProfileRepository>();
        }

        private static Mock<IPlanRepository> GetDefaultIPlanRepositoryInstance()
        {
            return new Mock<IPlanRepository>();
        }
        private static Mock<IUserRepository> GetDefaultIUserRepositoryInstance()
        {
            return new Mock<IUserRepository>();
        }
        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}
