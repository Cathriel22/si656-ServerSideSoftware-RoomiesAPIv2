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
    public class ReviewServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncWhenNoReviewReturnsEmptyCollection()
        {
            // Arrange

            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockReviewRepository.Setup(r => r.ListAsync()).ReturnsAsync(new List<Review>());

            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object);

            // Act

            List<Review> result = (List<Review>)await service.ListAsync();
            var reviewCount = result.Count;

            // Assert

            reviewCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByPostIdAsyncWhenNoReviewReturnsEmptyCollection()
        {
            // Arrange

            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var postId = 1;

            mockReviewRepository.Setup(r => r.ListByPostId(postId)).ReturnsAsync(new List<Review>());

            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object);

            // Act

            List<Review> result = (List<Review>)await service.ListByPostIdAsync(postId);
            var reviewCount = result.Count;

            // Assert

            reviewCount.Should().Equals(0);
        }

        [Test]
        public async Task GetAllByLeaseholderIdAsyncWhenNoReviewReturnsEmptyCollection()
        {
            // Arrange

            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var leaseholderId = 1;

            mockReviewRepository.Setup(r => r.ListByLeaseholderId(leaseholderId)).ReturnsAsync(new List<Review>());

            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object);

            // Act

            List<Review> result = (List<Review>)await service.ListByLeaseholderIdAsync(leaseholderId);
            var reviewCount = result.Count;

            // Assert

            reviewCount.Should().Equals(0);
        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsCategoryNotFoundResponse()
        {
            // Arrange
            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var reviewId = 1;
            Review review = new Review();
            mockReviewRepository.Setup(r => r.FindById(reviewId)).Returns(Task.FromResult<Review>(null));
            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object);

            // Act
            ReviewResponse result = await service.GetByIdAsync(reviewId);
            var message = result.Message;

            // Assert
            message.Should().Be("Review inexistente");
        }

        [Test]
        public async Task SaverReviewReturnsSave()
        {
            // Arrange


            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();

            Review review = new Review
            {
                Id = 1

            };

            Leaseholder leaseholder = new Leaseholder
            {
                Id = 1

            };

            Post post = new Post
            {
                Id = 1

            };


            mockReviewRepository.Setup(u => u.AddAsync(review)).Returns(Task.FromResult<Review>(review));
            mockReviewRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Review>(review));

            mockPostRepository.Setup(u => u.AddAsync(post)).Returns(Task.FromResult<Post>(post));
            mockPostRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Post>(post));

            mockLeaseholderRepository.Setup(u => u.AddAsync(leaseholder)).Returns(Task.FromResult<Leaseholder>(leaseholder));
            mockLeaseholderRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Leaseholder>(leaseholder));


            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object, mockLeaseholderRepository.Object, mockPostRepository.Object);


            // Act


            ReviewResponse result = await service.SaveAsync(review, 1, 1);


            // Assert

            result.Resource.Should().Be(review);
        }

        [Test]
        public async Task UpdateReviewReturnsUpdate()
        {
            // Arrange


            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            var mockLeaseholderRepository = GetDefaultILeaseholderRepositoryInstance();
            var mockReviewRepository = GetDefaultIReviewRepositoryInstance();

            Review review = new Review
            {
                Id = 1,
                Content = "My review :) "

            };

            Leaseholder leaseholder = new Leaseholder
            {
                Id = 1

            };

            Post post = new Post
            {
                Id = 1

            };


            mockReviewRepository.Setup(u => u.AddAsync(review)).Returns(Task.FromResult<Review>(review));
            mockReviewRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Review>(review));

            mockPostRepository.Setup(u => u.AddAsync(post)).Returns(Task.FromResult<Post>(post));
            mockPostRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Post>(post));

            mockLeaseholderRepository.Setup(u => u.AddAsync(leaseholder)).Returns(Task.FromResult<Leaseholder>(leaseholder));
            mockLeaseholderRepository.Setup(u => u.FindById(1)).Returns(Task.FromResult<Leaseholder>(leaseholder));


            var service = new ReviewService(mockReviewRepository.Object, mockUnitOfWork.Object, mockLeaseholderRepository.Object, mockPostRepository.Object);


            // Act


            ReviewResponse result = await service.UpdateAsync(1, review);


            // Assert

            result.Resource.Should().Be(review);
        }

        private static Mock<ILeaseholderRepository> GetDefaultILeaseholderRepositoryInstance()
        {
            return new Mock<ILeaseholderRepository>();
        }
        private static Mock<IPostRepository> GetDefaultIPostRepositoryInstance()
        {
            return new Mock<IPostRepository>();
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