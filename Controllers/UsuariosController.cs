using Microsoft.AspNetCore.Mvc;
using VulnerableApi.Models;
using VulnerableApi.Services;

namespace VulnerableApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly JsonService _jsonService;

    public UsuariosController(JsonService jsonService)
    {
        _jsonService = jsonService;
    }

    [HttpGet]
    public IActionResult GetUsuarios()
    {
        var usuarios = new List<Usuario>
        {
            new() { Id = 1, Nombre = "Juan Perez", Email = "juan@test.com" },
            new() { Id = 2, Nombre = "Maria Lopez", Email = "maria@test.com" },
            new() { Id = 3, Nombre = "Carlos Garcia", Email = "carlos@test.com" }
        };
        return Ok(usuarios);
    }

    [HttpPost("parse")]
    public IActionResult ParseJson([FromBody] string json)
    {
        var result = _jsonService.DeserializeObject<Usuario>(json);
        return Ok(result);
    }

    [HttpPost("validate")]
    public IActionResult ValidateInput([FromBody] string input)
    {
        var isValid = _jsonService.ValidateWithRegex(input);
        return Ok(new { Input = input, IsValid = isValid });
    }
}
