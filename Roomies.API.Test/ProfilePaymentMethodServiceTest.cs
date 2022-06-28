using FluentAssertions;
using Moq;
using NUnit.Framework;
using Roomies.API.Domain.Models;
using Roomies.API.Domain.Repositories;
using Roomies.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomies.API.Test
{
    public class ProfilePaymentMethodServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoProfilePaymentMethodReturnsEmptyCollection()
        {
            // Arrange

            var mockProfilePaymentMethodRepository = GetDefaultIProfilePaymentMethodRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockProfilePaymentMethodRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<ProfilePaymentMethod>());

            var service = new ProfilePaymentMethodService(mockProfilePaymentMethodRepository.Object, mockUnitOfWork.Object);

            // Act

            List<ProfilePaymentMethod> result = (List<ProfilePaymentMethod>)await service.ListAsync();
            var profilePaymentMethodCount = result.Count;

            // Assert

            profilePaymentMethodCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByPaymentMethodIdAsyncWhenNoProfilePaymentMethodReturnsEmptyCollection()
        {
            // Arrange

            var mockProfilePaymentMethodRepository = GetDefaultIProfilePaymentMethodRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var paymentMethodId = 1;

            mockProfilePaymentMethodRepository.Setup(r => r.ListByPaymentMethodIdAsync(paymentMethodId)).ReturnsAsync(new List<ProfilePaymentMethod>());

            var service = new ProfilePaymentMethodService(mockProfilePaymentMethodRepository.Object, mockUnitOfWork.Object);

            // Act

            List<ProfilePaymentMethod> result = (List<ProfilePaymentMethod>)await service.ListByPaymentMethodIdAsync(paymentMethodId);
            var profilePaymentMethodCount = result.Count;

            // Assert

            profilePaymentMethodCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByProfileIdAsyncWhenNoProfilePaymentMethodReturnsEmptyCollection()
        {
            // Arrange

            var mockProfilePaymentMethodRepository = GetDefaultIProfilePaymentMethodRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var profileId = 1;

            mockProfilePaymentMethodRepository.Setup(r => r.ListByProfileIdAsync(profileId)).ReturnsAsync(new List<ProfilePaymentMethod>());

            var service = new ProfilePaymentMethodService(mockProfilePaymentMethodRepository.Object, mockUnitOfWork.Object);

            // Act

            List<ProfilePaymentMethod> result = (List<ProfilePaymentMethod>)await service.ListByProfileIdAsync(profileId);
            var profilePaymentMethodCount = result.Count;

            // Assert

            profilePaymentMethodCount.Should().Equals(0);
        }


        private static Mock<IProfilePaymentMethodRepository> GetDefaultIProfilePaymentMethodRepositoryInstance()
        {
            return new Mock<IProfilePaymentMethodRepository>();
        }


        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }

    }
}
