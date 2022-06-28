using Roomies.API.Domain.Models;
using Roomies.API.Domain.Repositories;
using Roomies.API.Domain.Services;
using Roomies.API.Domain.Services.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roomies.API.Services
{
    public class ProfilePaymentMethodService : IProfilePaymentMethodService
    {
        private readonly IProfilePaymentMethodRepository _profilePaymentMethodRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProfilePaymentMethodService(IProfilePaymentMethodRepository userPaymentMethodRepository, IUnitOfWork unitOfWork)
        {
            _profilePaymentMethodRepository = userPaymentMethodRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ProfilePaymentMethodResponse> AssignProfilePaymentMethodAsync(int profileId, int paymentMethodId)
        {
            try
            {
                await _profilePaymentMethodRepository.AssignProfilePaymentMethodAsync(profileId, paymentMethodId);
                await _unitOfWork.CompleteAsync();
                ProfilePaymentMethod userPaymentMethod = await _profilePaymentMethodRepository.FindByUserIdAndPaymentMethodId(profileId, paymentMethodId);
                return new ProfilePaymentMethodResponse(userPaymentMethod);

            }
            catch (Exception ex)
            {
                return new ProfilePaymentMethodResponse($"Un error ocurrió al asignar usuario y método de pago: {ex.Message}");
            }
        }


        public async Task<IEnumerable<ProfilePaymentMethod>> ListAsync()
        {
            return await _profilePaymentMethodRepository.ListAsync();
        }

        public async Task<IEnumerable<ProfilePaymentMethod>> ListByPaymentMethodIdAsync(int paymentMethodId)
        {
            return await _profilePaymentMethodRepository.ListByPaymentMethodIdAsync(paymentMethodId);
        }

        public async Task<IEnumerable<ProfilePaymentMethod>> ListByProfileIdAsync(int profileId)
        {
            return await _profilePaymentMethodRepository.ListByProfileIdAsync(profileId);
        }

        public async Task<ProfilePaymentMethodResponse> UnassignProfilePaymentMethodAsync(int profileId, int paymentMethodId)
        {
            try
            {
                ProfilePaymentMethod userPaymentMethod = await _profilePaymentMethodRepository.FindByUserIdAndPaymentMethodId(profileId, paymentMethodId);

                _profilePaymentMethodRepository.Remove(userPaymentMethod);
                await _unitOfWork.CompleteAsync();

                return new ProfilePaymentMethodResponse(userPaymentMethod);

            }
            catch (Exception ex)
            {
                return new ProfilePaymentMethodResponse($"Un error ocurrió al desasignar usuario y método de pago: {ex.Message}");
            }
        }
    }
}
