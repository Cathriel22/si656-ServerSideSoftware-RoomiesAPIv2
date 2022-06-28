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

namespace Roomies.API.Test
{
    public class PlanServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoPlanReturnsEmptyCollection()
        {
            // Arrange

            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();

            mockPlanRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Domain.Models.Plan>());

            var service = new PlanService(mockPlanRepository.Object, mockUnitOfWork.Object, mockProfileRepository.Object);

            // Act

            List<Domain.Models.Plan> result = (List<Domain.Models.Plan>)await service.ListAsync();
            var planCount = result.Count;

            // Assert

            planCount.Should().Equals(0);

        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsCategoryNotFoundResponse()
        {
            // Arrange
            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockUserRepository = GetDefaultIProfileRepositoryInstance();
            var planId = 1;
            Domain.Models.Plan plan = new Domain.Models.Plan();
            mockPlanRepository.Setup(r => r.FindById(planId)).Returns(Task.FromResult<Domain.Models.Plan>(null));
            var service = new PlanService(mockPlanRepository.Object, mockUnitOfWork.Object,mockUserRepository.Object);

            // Act
            PlanResponse result = await service.GetByIdAsync(planId);
            var message = result.Message;

            // Assert
            message.Should().Be("Plan inexistente");
        }

        [Test]
        public async Task SavePlanReturnsSave()
        {
            // Arrange

            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();

            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();


            Domain.Models.Plan plan = new Domain.Models.Plan
            {
                Id = 1

            };

            mockPlanRepository.Setup(u => u.AddAsync(plan)).Returns(Task.FromResult<Domain.Models.Plan>(plan));
            mockPlanRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Domain.Models.Plan>(plan));

            var service = new PlanService(mockPlanRepository.Object, mockUnitOfWork.Object, mockProfileRepository.Object);

            // Act

            PlanResponse result = await service.SaveAsync(plan);

            // Assert

            result.Resource.Should().Be(plan);
        }

        [Test]
        public async Task UpdatePlanReturnsUpdate()
        {
            // Arrange

            var mockPlanRepository = GetDefaultIPlanRepositoryInstance();

            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockProfileRepository = GetDefaultIProfileRepositoryInstance();


            Domain.Models.Plan plan = new Domain.Models.Plan
            {
                Id = 1,
                Name =  "Leaseholder Plan"

            };

            mockPlanRepository.Setup(u => u.AddAsync(plan)).Returns(Task.FromResult<Domain.Models.Plan>(plan));
            mockPlanRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Domain.Models.Plan>(plan));

            var service = new PlanService(mockPlanRepository.Object, mockUnitOfWork.Object, mockProfileRepository.Object);

            // Act

            PlanResponse result = await service.UpdateAsync(1, plan);

            // Assert

            result.Resource.Should().Be(plan);
        }

        private static Mock<IPlanRepository> GetDefaultIPlanRepositoryInstance()
        {
            return new Mock<IPlanRepository>();
        }
        private static Mock<IProfileRepository> GetDefaultIProfileRepositoryInstance()
        {
            return new Mock<IProfileRepository>();
        }

        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}