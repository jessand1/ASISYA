using Microsoft.AspNetCore.Mvc;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Domain.Interfaces;

namespace OptimizerProvider.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HealthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --------------------------------------------------------------------
        // GET: api/health/ping
        // Endpoint rápido para load balancers / gateways
        // --------------------------------------------------------------------
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "pong" });
        }

        // --------------------------------------------------------------------
        // GET: api/health/status
        // Health check extendido con validación de base de datos
        // --------------------------------------------------------------------
        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var response = new Dictionary<string, object>();

            response["service"] = "ProviderOptimizerService";
            response["time"] = DateTime.UtcNow;
            response["environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";

            // ------------------------------------
            // Validación de conexión a la base de datos
            // ------------------------------------
            try
            {
                // "SELECT 1" equivalente usando UoW
                await _unitOfWork.TestConnectionAsync();
                response["database"] = "Healthy";
            }
            catch (Exception ex)
            {
                response["database"] = "Unhealthy";
                response["error"] = ex.Message;
                return StatusCode(503, response); // ServiceUnavailable
            }

            return Ok(response);
        }
    }
}
