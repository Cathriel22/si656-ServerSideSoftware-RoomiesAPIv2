using NUnit.Framework;
using Moq;
using FluentAssertions;
using Roomies.API.Domain.Repositories;
using Roomies.API.Services;
using Roomies.API.Domain.Services.Communications;
using System.Threading.Tasks;
using System.Collections.Generic;
using Roomies.API.Domain.Models;
using Roomies.API.Domain.Persistence.Repositories;
using System;

namespace Roomies.API.Test
{
    public class LeaseholderServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SaveLeaseholderWhenUserIsYoungerThanRequiredReturnsCantSave()
        {
            // Arrange

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultFavouritePostRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            String birthday = "1990-06-10";


            Domain.Models.Plan plan = new Domain.Models.Plan
            {
                Id = 1

            };

            User user = new User
            {
                Id = 1
            };

            Leaseholder leaseholder1 = new Leaseholder
            {
                UserId = 1,
                Birthday = birthday

            };


            List<Profile> profiles = new List<Profile>();
            //:(
            profiles.Add(leaseholder1);

            mockPlanRepository.Setup(u => u.AddAsync(plan)).Returns(Task.FromResult<Domain.Models.Plan>(plan));
            mockPlanRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Domain.Models.Plan>(plan));

            mockUserRepository.Setup(u => u.AddAsync(user)).Returns(Task.FromResult<User>(user));
            mockUserRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<User>(user));
          
            mockLeaseholderRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Leaseholder>(leaseholder1));

            var service = new LeaseholderService(mockLeaseholderRepository.Object, mockFavouritePostRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);


            // Act


            LeaseholderResponse result = await service.SaveAsync(leaseholder1, 1, 1);
            var message = result.Message;


            // Assert

            message.Should().Be("");
        }

        [Test]
        public async Task UpdateLeaseholderWhenUserIsYoungerThanRequiredReturnsCantSave()
        {
            // Arrange

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultFavouritePostRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            String birthday = "1990-06-10";


            Domain.Models.Plan plan = new Domain.Models.Plan
            {
                Id = 1

            };

            User user = new User
            {
                Id = 1
            };

            Leaseholder leaseholder1 = new Leaseholder
            {
                UserId = 1,
                Birthday = birthday,
                LastName = "Flores"
            };


            List<Profile> profiles = new List<Profile>();
            //:(
            profiles.Add(leaseholder1);

            mockPlanRepository.Setup(u => u.AddAsync(plan)).Returns(Task.FromResult<Domain.Models.Plan>(plan));
            mockPlanRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Domain.Models.Plan>(plan));

            mockUserRepository.Setup(u => u.AddAsync(user)).Returns(Task.FromResult<User>(user));
            mockUserRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<User>(user));

            mockLeaseholderRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Leaseholder>(leaseholder1));

            var service = new LeaseholderService(mockLeaseholderRepository.Object, mockFavouritePostRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);


            // Act


            LeaseholderResponse result = await service.UpdateAsync(1, leaseholder1);
            var message = result.Message;

            // Assert

            message.Should().Be("");
        }

        [Test]
        public async Task SaveLeaseholderWhenUserIsOlderThanEighteenReturnsSave()
        {
            // Arrange

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultFavouritePostRepositoryInstance();
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUserRepository = GetDefaultUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            String birthday = "1990-06-10";


            Domain.Models.Plan plan = new Domain.Models.Plan
            {
                Id = 1

            };

            Leaseholder leaseholder1 = new Leaseholder
            {
                UserId = 1,
                Birthday = birthday

            };

            User user = new User
            {
                Id = 1
            };

            List<Profile> profiles = new List<Profile>();

            mockPlanRepository.Setup(u => u.AddAsync(plan)).Returns(Task.FromResult<Domain.Models.Plan>(plan));
            mockPlanRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Domain.Models.Plan>(plan));

            mockUserRepository.Setup(u => u.AddAsync(user)).Returns(Task.FromResult<User>(user));
            mockUserRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<User>(user));

            mockLeaseholderRepository.Setup(u => u.AddAsync(leaseholder1)).Returns(Task.FromResult<Leaseholder>(null));
            mockLeaseholderRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Leaseholder>(leaseholder1));

            var service = new LeaseholderService(mockLeaseholderRepository.Object, mockFavouritePostRepository.Object, mockUnitOfWork.Object, mockPlanRepository.Object, mockUserRepository.Object);


            // Act


            LeaseholderResponse result = await service.SaveAsync(leaseholder1, 1, 1);


            // Assert

            result.Resource.Should().Be(leaseholder1);
        }


        [Test]
        public async Task GetAllAsyncWhenNoLeaseholderReturnsEmptyCollection()
        {
            // Arrange

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockFavouritePostRepository = GetDefaultFavouritePostRepositoryInstance();

            mockLeaseholderRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Leaseholder>());

            var service = new LeaseholderService(mockLeaseholderRepository.Object, mockFavouritePostRepository.Object, mockUnitOfWork.Object);

            // Act

            List<Leaseholder> result = (List<Leaseholder>)await service.ListAsync();
            var leaseholderCount = result.Count;

            // Assert

            leaseholderCount.Should().Equals(0);
        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsLeaseholderNotFoundResponse()
        {
            // Arrange
            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockFavouritePostRepository = GetDefaultFavouritePostRepositoryInstance();

            var leaseholderId = 1;
            Leaseholder leaseholder = new Leaseholder();
            mockLeaseholderRepository.Setup(r => r.FindById(leaseholderId)).Returns(Task.FromResult<Leaseholder>(null));
            var service = new LeaseholderService(mockLeaseholderRepository.Object, mockFavouritePostRepository.Object, mockUnitOfWork.Object);

            // Act
            LeaseholderResponse result = await service.GetByIdAsync(leaseholderId);
            var message = result.Message;

            // Assert
            message.Should().Be("Arrendatario inexistente");
        }

        private static Mock<ILeaseholderRepository> GetDefaultILeaseholderRepositoryInstance()
        {
            return new Mock<ILeaseholderRepository>();
        }
        private static Mock<IPlanRepository> GetDefaultIPlanRepositoryInstance()
        {
            return new Mock<IPlanRepository>();
        }

        private static Mock<IFavouritePostRepository> GetDefaultFavouritePostRepositoryInstance()
        {
            return new Mock<IFavouritePostRepository>();
        }

        private static Mock<IUserRepository> GetDefaultUserRepositoryInstance()
        {
            return new Mock<IUserRepository>();
        }

        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }


    }
}