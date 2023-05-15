
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Dtos;
using Railway.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Railway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
     
       private readonly IConfiguration _config;
       private readonly IUser _userRep;
       private readonly ISupervisor _supervisorRep;
       private readonly DataContext _context;

       public AuthController(IConfiguration config, IUser userRep, ISupervisor supervisorRep ,DataContext context)
            {
                _config = config;
                _userRep = userRep;
               _supervisorRep = supervisorRep;
            _context = context;
            }

        [HttpPost("Admin/Register")]
        public ActionResult<Admin> Register(UserDto request)
            {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                if (_userRep.DoesItExist(request.Email))
                    return BadRequest("User already exists");

                if (request == null)
                {
                    return BadRequest("Invalid token.");
                }

                var user = new Admin()
                {
                    Username = request.UserName,
                    PasswordHash = passwordHash,
                    Email = request.Email,
                    Name = request.UserName,
                    Surname = request.Surname,
                };

            
                _userRep.Insert(user);


                return Ok(user);
            }

        [HttpPost("Register")]
        public ActionResult<Supervisior> SupervisorRegister(UserDto request)
        {
           string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            if (_userRep.DoesItExist(request.Email))
                return BadRequest("User already exists");

            if (request == null)
            {
                return BadRequest("Invalid token.");
            }

            var user = new Supervisior()
            {
                Username = request.UserName,
                PasswordHash = passwordHash,
                Email = request.Email,
                Name = request.UserName,
                Surname = request.Surname,
            };

            _supervisorRep.Insert(user);


            return Ok(user);
        }

        [HttpPost("Login")]
        public ActionResult<User> Login(LoginDto request)
        {

               var supervisor = _supervisorRep.Find(request.Email);
               var user = _userRep.Find(request.Email);
               var userRole = "";
               var token = "";

            if (user != null)
               {

                if (!_userRep.DoesItExist(request.Email))
                {
                    return BadRequest("User not found");
                } 

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest("wrong password");
                }

                 
                 userRole = user.Roles;
                token = CreateTokenAdmin(user);
                return Ok(token);
            }
            else if(supervisor != null)
            {
                if (!_supervisorRep.DoesItExist(request.Email))
                {
                    return BadRequest("User not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, supervisor.PasswordHash))
                {
                    return BadRequest("wrong password");
                }

                 token = CreateToken(supervisor);
                 userRole = supervisor.Roles;
                return Ok(token);

            }


            return Ok(token);
            }
        private string CreateToken(Supervisior user)
            {

                var claims = new[]
                {
                  new Claim(ClaimTypes.NameIdentifier, user.Username),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Surname, user.Surname),
                  new Claim(ClaimTypes.GivenName, user.Name),
                  new Claim(ClaimTypes.Role,user.Roles),
            };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Key").Value!));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: cred
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
        private string CreateTokenAdmin(Admin user)
        {

            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier, user.Username),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Surname, user.Surname),
                  new Claim(ClaimTypes.GivenName, user.Name),
                  new Claim(ClaimTypes.Role,user.Roles),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Key").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private string CreateRandomToken()
            {
                return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            }
        }
    }
