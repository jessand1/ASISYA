using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProviderOptimizer.Application.Optimization.Commands;
using ProviderOptimizer.Application.Optimization.Queries;

namespace OptimizerProvider.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptimizerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OptimizerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --------------------------------------------------------------------
        // POST: api/optimizer/optimize
        // Ejecuta el proceso de optimización 
        // (Obligatorio según la prueba técnica)
        // --------------------------------------------------------------------
        [HttpPost("optimize")]
        public async Task<IActionResult> Optimize([FromBody] OptimizeRequestCommand command)
        {
            if (command is null)
                return BadRequest("Invalid request payload.");

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET: api/optimizer/{id}/results
        // Obtiene los resultados de una optimización previa (opcional)
        // --------------------------------------------------------------------
        [HttpGet("{requestId:guid}/results")]
        public async Task<IActionResult> GetResults(Guid requestId)
        {
            var query = new GetOptimizationResultsQuery(requestId);
            var result = await _mediator.Send(query);

            return result is not null ? Ok(result) : NotFound();
        }
    }
}
