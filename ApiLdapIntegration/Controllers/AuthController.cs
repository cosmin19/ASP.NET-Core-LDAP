using ApiLdapIntegration.Models;
using ApiLdapIntegration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiLdapIntegration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            AuthResult authResult = _authService.Login(model.UserName, model.Password);

            if(authResult.IsSucceeded)
            {
                return Ok(new { Message = $"Hello {authResult.AppUser.DisplayName}. Your email is {authResult.AppUser.Email}" });
            }

            return BadRequest(authResult.Errors);
        }
    }
}
