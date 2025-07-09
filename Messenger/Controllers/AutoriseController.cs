using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messenger.Dtos;
using Messenger.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Messenger.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Controllers;


[Route("api/authorise")]
[ApiController]
public class AuthorisationController(IConfiguration configuration) : Controller
{
    public UserAuthModel user = new ();
    [HttpPost("register")]
    public ActionResult Register([FromBody]UserAuthDto request)
    {
        var hashedPassword = new PasswordHasher<UserAuthModel>()
            .HashPassword(user,  request.Password);
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody]UserAuthDto request)
    {
        

        string token = CreateToken(user);
        return Ok(token);
    }

    private string CreateToken(UserAuthModel user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public ActionResult AddUser()
    {
        return View();
    }
}