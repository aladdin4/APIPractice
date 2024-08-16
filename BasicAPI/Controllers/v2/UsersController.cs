using BasicAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BasicAPI.Controllers.v2;

[Route("api/v{Version:apiVersion}/[controller]")]    //= Get api/v0.3/Users
//[Route("api/[controller]")]    //= Get api/Users
[ApiController]
[ApiVersion("2.6")]
[ApiVersion("3.9")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    [MapToApiVersion("2.6")]
    public IEnumerable<string> Get()
    {
        var x = _config.GetConnectionString("Default");
        return new string[] { "Version 2 Value 1", "Version 2 Value 2"};
    }

    [HttpGet]
    [MapToApiVersion("3.9")]
    public IEnumerable<string> GetV3()
    {
        var x = _config.GetConnectionString("Default");
        return new string[] { "Version 3 Value 1", "Version 3 Value 2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    [Authorize(Policy = PolicyConstants.MustHaveEmployeeId)]
    public string Get(int id)
    {
        return "id received: " + id;
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
