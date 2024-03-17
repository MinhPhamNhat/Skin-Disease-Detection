using Microsoft.AspNetCore.Mvc;
using SkinDiseaseDetectionApi.Models;
using SkinDiseaseDetectionApi.Service;

namespace SkinDiseaseDetectionApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ILogger<DoctorsController> _logger;
    private readonly MongoDBService _mongoDBService;

    public DoctorsController(ILogger<DoctorsController> logger, MongoDBService mongoDBService)
    {
        _logger = logger;
        _mongoDBService = mongoDBService;
    }

    [HttpGet(Name = "GetDoctors")]
    public async Task<IEnumerable<Doctor>> Get()
    {
        return _mongoDBService.GetAsync<Doctor>().Result;
    }
}
