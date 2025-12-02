using MediatR;
using Microsoft.AspNetCore.Mvc;
using OptimizerProvider.Application.Features.Provider.Queries;
using ProviderOptimizer.Application.Provider.Commands;
using ProviderOptimizer.Application.Provider.Queries;

namespace OptimizerProvider.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProviderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --------------------------------------------------------------------
        // GET: api/providers/available
        // Retorna proveedores disponibles — requerido por la prueba técnica
        // --------------------------------------------------------------------
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProviders()
        {
            var result = await _mediator.Send(new GetAvailableProvidersQuery());
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET: api/providers/{id}
        // Información general del proveedor
        // --------------------------------------------------------------------
        [HttpGet("{providerId:guid}")]
        public async Task<IActionResult> GetProviderById(Guid providerId)
        {
            var result = await _mediator.Send(new GetProviderByIdQuery(providerId));
            return result is not null ? Ok(result) : NotFound();
        }

        // --------------------------------------------------------------------
        // POST: api/providers/{id}/location
        // Actualiza la ubicación del proveedor
        // --------------------------------------------------------------------
        [HttpPost("{providerId:guid}/location")]
        public async Task<IActionResult> UpdateLocation(
            Guid providerId,
            [FromBody] UpdateProviderLocationCommand command)
        {
            command.ProviderId = providerId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // POST: api/providers/{id}/availability
        // Actualiza disponibilidad del proveedor (disponible/ocupado/offline)
        // --------------------------------------------------------------------
        [HttpPost("{providerId:guid}/availability")]
        public async Task<IActionResult> UpdateAvailability(
            Guid providerId,
            [FromBody] UpdateProviderAvailabilityCommand command)
        {
            command.ProviderId = providerId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET: api/providers/{id}/workload
        // Obtiene la carga actual del proveedor (casos activos)
        // --------------------------------------------------------------------
        [HttpGet("{providerId:guid}/workload")]
        public async Task<IActionResult> GetWorkload(Guid providerId)
        {
            var result = await _mediator.Send(new GetProviderWorkloadQuery(providerId));
            return Ok(result);
        }
    }
}
