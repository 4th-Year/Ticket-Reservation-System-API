using api.Models;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserServices _userServices;

        private readonly UserController _userController;

        public AuthenticationController(UserServices userServices , UserController userController)
        {
            _userServices = userServices;
            _userController = userController;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            var result = await LoginAsync(login);

            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        private async Task<LoginResponse> LoginAsync(Login login)
        {
            var user = await _userServices.GetAsyncByEmail(login.Email);

            if (user == null)
            {
                return new LoginResponse { Message = "Invalid Email", Success = false };
            } else
            {
                bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

                if (!isPasswordMatch)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid Password"
                    };
                }
                else { 

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                    var role = user.IsAdmin;
                    string roleName;

                    if (role)
                    {
                        roleName = "admin";
                    } else
                    {
                        roleName = "user";
                    }
                    var roleClaims = new Claim(ClaimTypes.Role, roleName.ToString());

                    claims.Add(roleClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("lswek3u4uop2u896a"));
                    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddMinutes(30);

                    var token = new JwtSecurityToken(
                        issuer: "https://localhost:3000",
                        audience: "https://localhost:3000",
                        claims: claims,
                        expires: expires,
                        signingCredentials: cred
                        );

                    return new LoginResponse
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Message = "Login Successfull",
                        Success = true,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserId = user.Id,
                        isAdmin = user.IsAdmin,
                        Email = user.Email,
                    };
                }
            }
        }
    }
}
