using FluentAssertions;
using Moq;
using NUnit.Framework;
using Roomies.API.Domain.Models;
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
    public class FavouritePostServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoFavouritePostReturnsEmptyCollection()
        {
            // Arrange

            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockFavouritePostRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<FavouritePost>());

            var service = new FavouritePostService(mockFavouritePostRepository.Object, mockUnitOfWork.Object);

            // Act

            List<FavouritePost> result = (List<FavouritePost>)await service.ListAsync();
            var favouritePostCount = result.Count;

            // Assert

            favouritePostCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByPostIdAsyncWhenNoFavouritePostReturnsEmptyCollection()
        {
            // Arrange

            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var postId = 1;

            mockFavouritePostRepository.Setup(r => r.ListByPostIdAsync(postId)).ReturnsAsync(new List<FavouritePost>());

            var service = new FavouritePostService(mockFavouritePostRepository.Object, mockUnitOfWork.Object);

            // Act

            List<FavouritePost> result = (List<FavouritePost>)await service.ListByPostIdAsync(postId);
            var favouritePostCount = result.Count;

            // Assert

            favouritePostCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByLeaseholderIdAsyncWhenNoFavouritePostReturnsEmptyCollection()
        {
            // Arrange

            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var leaseholderId = 1;

            mockFavouritePostRepository.Setup(r => r.ListByLeaseholderIdAsync(leaseholderId)).ReturnsAsync(new List<FavouritePost>());

            var service = new FavouritePostService(mockFavouritePostRepository.Object, mockUnitOfWork.Object);

            // Act

            List<FavouritePost> result = (List<FavouritePost>)await service.ListByLeaseholderIdAsync(leaseholderId);
            var favouritePostCount = result.Count;

            // Assert

            favouritePostCount.Should().Equals(0);
        }       

        private static Mock<IFavouritePostRepository> GetDefaultIFavouritePostRepositoryInstance()
        {
            return new Mock<IFavouritePostRepository>();
        }

        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}
