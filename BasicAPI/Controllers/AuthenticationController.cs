using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace BasicAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    public record AuthenticationData(string UserName, string Password);
    public record UserData(int Id, string UserName, string Role, string EmployeeId);

    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("token")]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData authData)
    {
        var user = ValidateCredentials(authData);
        if (user is null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);
        return Ok(token);
    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));
        SigningCredentials? signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>();
        claims.Add(new (JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
        claims.Add(new("role", user.Role));
        claims.Add(new("employeeId", user.EmployeeId));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signinCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private UserData? ValidateCredentials(AuthenticationData authData)
    {
        if (CompareValues(authData.UserName, "admin") && CompareValues(authData.Password, "admin"))
        {
            return new UserData(1, authData.UserName, "Admin", "E011");
        }

        if (CompareValues(authData.UserName, "user") && CompareValues(authData.Password, "user"))
        {
            return new UserData(2, authData.UserName, "User", "E101");
        }

        return null;
    }

    private bool CompareValues(string value1, string value2)
    {
        if (value1 is not null)
        {
            if (value1.Equals(value2, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

}
