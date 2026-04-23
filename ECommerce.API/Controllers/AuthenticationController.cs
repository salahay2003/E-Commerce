using ECommerce.BLL;
using ECommerce.Common;
using ECommerce.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public AuthenticationController(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<GeneralResult<IdentityResult>>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var applicationUser = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email.Split('@')[0],
                Email = registerDto.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            //IdentityResult addRoleResult = await _userManager.AddToRoleAsync(applicationUser, "Admin");
            //if (!addRoleResult.Succeeded)
            //{
            //    return BadRequest(addRoleResult);
            //}
            return Ok(result);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user is null)
            {
                return Unauthorized("Invalid Email or Password");
            }
         
            var result = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
            if (!result)
            {
                return Unauthorized("Invalid Email or Password");
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(ClaimTypes.Email, user.Email!));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //await _userManager.AddClaimsAsync(user, claims);
            var tokenDto = GenerateToken(claims);
            return Ok(tokenDto);
        }
        /*------------------------------------------------------------------*/
        private TokenDto GenerateToken(List<Claim> claims)
        {
            var keyFromConfig = _jwtSettings.SecretKey;
            var keyInBytes = Convert.FromBase64String(keyFromConfig);
            var key = new SymmetricSecurityKey(keyInBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryDateTime = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: expiryDateTime
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenDto = new TokenDto(token, _jwtSettings.DurationInMinutes);
            return tokenDto;
        }
    }
}
