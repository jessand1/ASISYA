using MediatR;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Provider.Commands;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Provider.Handlers
{
    public class UpdateProviderAvailabilityCommandHandler
        : IRequestHandler<UpdateProviderAvailabilityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProviderAvailabilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateProviderAvailabilityCommand request,
            CancellationToken cancellationToken)
        {
            var providerRepo = _unitOfWork.Repository<ProviderOptimizer.Domain.Entities.Provider>();
            var availabilityRepo = _unitOfWork.Repository<ProviderAvailability>();

            // 1. validar provider
            var provider = await providerRepo.GetByIdAsync(request.ProviderId);
            if (provider is null)
                return false;

            // 2. obtener availability existente o crear una nueva
            var availability = await availabilityRepo.GetByIdAsync(request.ProviderId);

            if (availability is null)
            {
                availability = new ProviderAvailability
                {
                    ProviderId = request.ProviderId,
                    IsAvailable = request.IsAvailable,
                    LastUpdate = DateTime.UtcNow
                };

                // si se envía el status
                if (!string.IsNullOrWhiteSpace(request.Status))
                    availability.Status = request.Status;

                await availabilityRepo.AddAsync(availability);
            }
            else
            {
                availability.IsAvailable = request.IsAvailable;
                availability.LastUpdate = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(request.Status))
                    availability.Status = request.Status;

                availabilityRepo.Update(availability);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
