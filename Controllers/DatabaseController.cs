using Microsoft.AspNetCore.Mvc;
using VulnerableApi.Services;

namespace VulnerableApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DatabaseService _dbService;

    public DatabaseController(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("user/{id}")]
    public IActionResult GetUser(string id)
    {
        var result = _dbService.GetUserById(id);
        return Ok(new { UserId = id, Data = result });
    }

    [HttpGet("ssl-check")]
    public IActionResult CheckSsl()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = _dbService.ValidateCertificate
        };
        return Ok(new { Status = "SSL validation disabled" });
    }
}
