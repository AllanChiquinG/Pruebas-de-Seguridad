using Microsoft.AspNetCore.Mvc;
using VulnerableApi.Services;

namespace VulnerableApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly SecurityService _securityService;

    public SecurityController(SecurityService securityService)
    {
        _securityService = securityService;
    }

    [HttpGet("ldap")]
    public IActionResult LdapSearch([FromQuery] string filter = "(objectClass=*)")
    {
        var result = _securityService.LdapSearch(filter);
        return Ok(new { Results = result });
    }

    [HttpPost("image")]
    public IActionResult ProcessImage([FromBody] string imagePath)
    {
        _securityService.ProcessImage(imagePath);
        return Ok(new { Status = "Image processed", Path = imagePath + ".processed.png" });
    }

    [HttpPost("encrypt")]
    public IActionResult Encrypt([FromBody] EncryptRequest request)
    {
        var encrypted = _securityService.EncryptData(request.Data, request.Key);
        return Ok(new { Encrypted = encrypted });
    }

    [HttpPost("sign-xml")]
    public IActionResult SignXml([FromBody] string xmlContent)
    {
        var signed = _securityService.SignXmlDocument(xmlContent);
        return Ok(new { SignedXml = signed });
    }
}

public class EncryptRequest
{
    public string Data { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}
