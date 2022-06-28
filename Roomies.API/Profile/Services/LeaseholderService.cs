using Roomies.API.Domain.Models;
using Roomies.API.Domain.Persistence.Repositories;
using Roomies.API.Domain.Repositories;
using Roomies.API.Domain.Services;
using Roomies.API.Domain.Services.Communications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roomies.API.Services
{
    public class LeaseholderService : ILeaseholderService
    {
        private readonly ILeaseholderRepository _leaseholderRepository;

        private readonly IPlanRepository _planRepository;
        private readonly IFavouritePostRepository _favouritePostRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public LeaseholderService(ILeaseholderRepository leaseholderRepository, IFavouritePostRepository favouritePostRepository, IUnitOfWork unitOfWork, IPlanRepository planRepository = null, IUserRepository userRepository = null)
        {
            _leaseholderRepository = leaseholderRepository;
            _favouritePostRepository = favouritePostRepository;
            _unitOfWork = unitOfWork;
            _planRepository = planRepository;
            _userRepository = userRepository;
        }

        public async Task<LeaseholderResponse> DeleteAsync(int id)
        {
            var existingLeaseholder = await _leaseholderRepository.FindById(id);

            if (existingLeaseholder == null)
                return new LeaseholderResponse("Arrendatario inexistente");

            try
            {
                if (existingLeaseholder.FavouritePosts != null)
                {
                    existingLeaseholder.FavouritePosts.ForEach(favouritePost=>
                {
                    _favouritePostRepository.Remove(favouritePost);
                });
                }

                _leaseholderRepository.Remove(existingLeaseholder);
                await _unitOfWork.CompleteAsync();

                return new LeaseholderResponse(existingLeaseholder);
            }
            catch (Exception ex)
            {
                return new LeaseholderResponse($"Un error ocurrió al eliminar el Arrendatario: {ex.Message}");
            }
        }

        public async Task<LeaseholderResponse> GetByIdAsync(int id)
        {
            var existingLeaseholder = await _leaseholderRepository.FindById(id);

            if (existingLeaseholder == null)
                return new LeaseholderResponse("Arrendatario inexistente");

            return new LeaseholderResponse(existingLeaseholder);
        }

        public async Task<IEnumerable<Leaseholder>> ListAsync()
        {
            return await _leaseholderRepository.ListAsync();
        }

        public async Task<IEnumerable<Leaseholder>> ListByPostIdAsync(int postId)
        {
            var favouritePost = await _favouritePostRepository.ListByPostIdAsync(postId);
            var leaseholders= favouritePost.Select(pt => pt.Leaseholder).ToList();
            return leaseholders;
        }

        public async Task<LeaseholderResponse> SaveAsync(Leaseholder landlord,int planId, int userId)
        {
            var existingPlan = await _planRepository.FindById(planId);

            if (existingPlan == null)
                return new LeaseholderResponse("Plan inexistente");

            var existingUser = await _userRepository.FindById(userId);


            if (existingUser == null)
                return new LeaseholderResponse("User inexistente");

            try
            {


                landlord.Plan = existingPlan;
                landlord.PlanId = planId;
                landlord.User = existingUser;
                landlord.UserId = userId;


                await _leaseholderRepository.AddAsync(landlord);
                await _unitOfWork.CompleteAsync();

                return new LeaseholderResponse(landlord);
                
            }

            catch (Exception ex)
            {
                return new LeaseholderResponse($"Un error ocurrió al guardar el arrendatario: {ex.Message}");
            }
        }

        public async Task<LeaseholderResponse> UpdateAsync(int id, Leaseholder landlord)
        {
            var existingLeaseholder= await _leaseholderRepository.FindById(id);

            if (existingLeaseholder == null)
                return new LeaseholderResponse("Arrendatario inexistente");

            existingLeaseholder.Name = landlord.Name;
            existingLeaseholder.Address = landlord.Address;
            existingLeaseholder.Birthday = landlord.Birthday;
            existingLeaseholder.Department = landlord.Department;
            existingLeaseholder.CellPhone = landlord.CellPhone;
            existingLeaseholder.District = landlord.District;
            existingLeaseholder.LastName = landlord.LastName;
            existingLeaseholder.Province = landlord.Province;
            existingLeaseholder.IdCard = landlord.IdCard;
            existingLeaseholder.Description = landlord.Description;
            existingLeaseholder.Verified = landlord.Verified;

            try
            {
                _leaseholderRepository.Update(existingLeaseholder);
                await _unitOfWork.CompleteAsync();

                return new LeaseholderResponse(existingLeaseholder);
            }
            catch (Exception ex)
            {
                return new LeaseholderResponse($"Un error ocurrió al actualizar el arrendador: {ex.Message}");
            }
        }
    }
}
