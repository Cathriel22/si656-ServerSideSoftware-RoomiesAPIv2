using NUnit.Framework;
using Moq;
using FluentAssertions;
using Roomies.API.Domain.Repositories;
using Roomies.API.Services;
using Roomies.API.Domain.Services.Communications;
using System.Threading.Tasks;
using System.Collections.Generic;
using Roomies.API.Domain.Models;

namespace Roomies.API.Test
{
    public class PostServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoPostReturnsEmptyCollection()
        {
            // Arrange

            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockPostRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Post>());

            var service = new PostService(mockPostRepository.Object, mockUnitOfWork.Object, mockFavouritePostRepository.Object);

            // Act

            List<Post> result = (List<Post>)await service.ListAsync();
            var postCount = result.Count;

            // Assert

            postCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByLandlordIdAsyncWhenNoPostReturnsEmptyCollection()
        {
            // Arrange

            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var landlordId = 1;

            mockPostRepository.Setup(r => r.ListByLandlordIdAsync(landlordId)).ReturnsAsync(new List<Post>());

            var service = new PostService(mockPostRepository.Object, mockUnitOfWork.Object, mockFavouritePostRepository.Object);

            // Act

            List<Post> result = (List<Post>)await service.ListByLandlordIdAsync(landlordId);
            var postCount = result.Count;

            // Assert

            postCount.Should().Equals(0);
        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsCategoryNotFoundResponse()
        {
            // Arrange
            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var postId = 1;
            Post post = new Post();
            mockPostRepository.Setup(r => r.FindById(postId)).Returns(Task.FromResult<Post>(null));
            var service = new PostService(mockPostRepository.Object, mockUnitOfWork.Object, mockFavouritePostRepository.Object);

            // Act
            PostResponse result = await service.GetByIdAsync(postId);
            var message = result.Message;

            // Assert
            message.Should().Be("Post inexistente");
        }

        [Test]
        public async Task SavePostReturnsSave()
        {
            // Arrange


            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockLandlordRepository = GetDefaultILandlordRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();          
            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();

            Post post = new Post
            {
                Id = 1

            };

            Landlord landlord = new Landlord
            {
                Id = 1

            };


            mockPostRepository.Setup(u => u.AddAsync(post)).Returns(Task.FromResult<Post>(post));
            mockPostRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Post>(post));

            mockLandlordRepository.Setup(u => u.AddAsync(landlord)).Returns(Task.FromResult<Landlord>(landlord));
            mockLandlordRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Landlord>(landlord));          

            var service = new PostService(mockPostRepository.Object, mockUnitOfWork.Object, mockFavouritePostRepository.Object, mockLandlordRepository.Object, mockReviewRepository.Object);


            // Act


            PostResponse result = await service.SaveAsync(post, 1);


            // Assert

            result.Resource.Should().Be(post);
        }

        [Test]
        public async Task UpdatePostReturnsUpdate()
        {
            // Arrange


            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockLandlordRepository = GetDefaultILandlordRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var mockFavouritePostRepository = GetDefaultIFavouritePostRepositoryInstance();
            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();


            Post post = new Post
            {
                Id = 1,
                Department = "Lima"
            };

            Landlord landlord = new Landlord
            {
                Id = 1

            };


            mockPostRepository.Setup(u => u.AddAsync(post)).Returns(Task.FromResult<Post>(post));
            mockPostRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Post>(post));

            mockLandlordRepository.Setup(u => u.AddAsync(landlord)).Returns(Task.FromResult<Landlord>(landlord));
            mockLandlordRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Landlord>(landlord));

            var service = new PostService(mockPostRepository.Object, mockUnitOfWork.Object, mockFavouritePostRepository.Object, mockLandlordRepository.Object, mockReviewRepository.Object);


            // Act


            PostResponse result = await service.UpdateAsync(1, post);


            // Assert

            result.Resource.Should().Be(post);
        }

        private static Mock<IPostRepository> GetDefaultIPostRepositoryInstance()
        {
            return new Mock<IPostRepository>();
        }

        private static Mock<IFavouritePostRepository> GetDefaultIFavouritePostRepositoryInstance()
        {
            return new Mock<IFavouritePostRepository>();
        }
        private static Mock<ILandlordRepository> GetDefaultILandlordRepositoryInstance()
        {
            return new Mock<ILandlordRepository>();
        }
        private static Mock<IReviewRepository> GetDefaultIReviewRepositoryInstance()
        {
            return new Mock<IReviewRepository>();
        }
        private static Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}