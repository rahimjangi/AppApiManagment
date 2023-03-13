using AppApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpPost("[action]")]
    public IActionResult Register()
    {
        return Ok();
    }

    [HttpPost("[action]")]
    public IActionResult Login()
    {
        return Ok();
    }
}
