using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Roomies.API.Domain.Models;
using Roomies.API.Domain.Persistence.Repositories;
using Roomies.API.Domain.Repositories;
using Roomies.API.Domain.Services.Communications;
using Roomies.API.Services;
using Roomies.API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomies.API.Test
{
    public class UserServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoUserReturnsEmptyCollection()
        {
            // Arrange

            var mockAppSettingsRepository = GetDefaultIAppSettingsInstance();
            var mockUserRepository = GetDefaultIUserRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockMapperRepository = GetDefaultIMapperInstance();

            mockUserRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<User>());

            var service = new UserService(mockAppSettingsRepository.Object, mockMapperRepository.Object, mockUserRepository.Object, mockUnitOfWork.Object);

            // Act

            List<User> result = (List<User>)await service.GetAll();
            var userCount = result.Count;

            // Assert

            userCount.Should().Equals(0);
        }

        private static Mock<IOptions<AppSettings>> GetDefaultIAppSettingsInstance()
        {
            return new Mock<IOptions<AppSettings>>();
        }

        private static Mock<IUserRepository> GetDefaultIUserRepositoryInstance()
        {
            return new Mock<IUserRepository>();
        }
        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
        private static Mock<IMapper> GetDefaultIMapperInstance()
        {
            return new Mock<IMapper>();
        }
    }
}
