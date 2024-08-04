using BasicAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BasicAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }
    // GET: api/<UsersController>
    [HttpGet]
    [AllowAnonymous]
    public string Get()
    {
        var x = _config.GetConnectionString("Default");
        return x;
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
   [Authorize(Policy = PolicyConstants.MustHaveEmployeeId)]
    public string Get(int id)
    {
        return "value";
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
