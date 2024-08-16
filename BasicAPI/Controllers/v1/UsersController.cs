using BasicAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BasicAPI.Controllers.v1;

[Route("api/v{Version:apiVersion}/[controller]")]    //= Get api/v0.3/Users
//[Route("api/[controller]")]    //= Get api/Users
[ApiController]
[ApiVersion("1.3", Deprecated = true)]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IConfiguration config, ILogger<UsersController> logger)
    {
        _config = config;
        _logger = logger;
    }

    // GET: api/<UsersController>
    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<string> Get()
    {
        var x = _config.GetConnectionString("Default");
        return new string[] { "Version 1 Value 1", "Version 1 Value 2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    // [Authorize(Policy = PolicyConstants.MustHaveEmployeeId)]
    public IActionResult Get(int id)
    {
        try
        {
            if (id <= 0 || id > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            _logger.LogInformation(@"ID {id} Is Valid Id", id);
            return Ok("id received: " + id);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, @"ID {id} Is Invalid Id", id);
            return BadRequest($"ID {id} Is Invalid Id");
        }
    }

    // POST api/<UsersController>
    [HttpPost]
    public string Post([FromBody] string value)
    {
        return "hello world333";
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
