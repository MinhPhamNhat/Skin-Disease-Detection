using Microsoft.AspNetCore.Mvc;
using SkinDiseaseDetectionApi.Dto;

namespace SkinDiseaseDetectionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticController : ControllerBase
    {
        private readonly ILogger<DiagnosticController> _logger;
        private readonly IDiagnoseService _diagnoseService;
        public DiagnosticController(ILogger<DiagnosticController> logger, IDiagnoseService diagnoseService)
        {
            _logger = logger;
            _diagnoseService = diagnoseService;
        }

        [HttpPost]
        public async Task<ActionResult<PredictionDto>> Get([FromForm] PredictRequest request, string modelType)
        {
            return Ok(await _diagnoseService.DiagnoseSkinDisease(modelType, request));
        }
    }
}