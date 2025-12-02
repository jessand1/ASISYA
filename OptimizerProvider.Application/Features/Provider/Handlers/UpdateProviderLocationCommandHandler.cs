using MediatR;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Provider.Commands;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Provider.Handlers
{
    public class UpdateProviderLocationCommandHandler
        : IRequestHandler<UpdateProviderLocationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProviderLocationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateProviderLocationCommand request,
            CancellationToken cancellationToken)
        {
            var providerRepo = _unitOfWork.Repository<ProviderOptimizer.Domain.Entities.Provider>();
            var locationRepo = _unitOfWork.Repository<ProviderLocation>();

            // 1. obtener provider
            var provider = await providerRepo.GetByIdAsync(request.ProviderId);

            if (provider is null)
                return false;

            // 2. obtener o crear location
            var location = await locationRepo.GetByIdAsync(request.ProviderId);

            if (location is null)
            {
                location = new ProviderLocation
                {
                    ProviderId = provider.ProviderId,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    UpdatedAt = DateTime.UtcNow
                };

                await locationRepo.AddAsync(location);
            }
            else
            {
                location.Latitude = request.Latitude;
                location.Longitude = request.Longitude;
                location.UpdatedAt = DateTime.UtcNow;

                locationRepo.Update(location);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
