using FreeIntegration.Models.Login;
using FreeIntegration.Services.LoginService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FreeIntegration.Api.Controllers
{
    [ApiController]
    public class LoginController : BaseController
    {
        #region Ctor
        public LoginController(IConfiguration configuration, ILoginService loginService) : base(configuration, loginService)
        {

        }
        #endregion


        [HttpPost("freeintegration/login")]
        public async Task<IActionResult> Login([FromBody] LoginUserLogDT loginUser)
        {
            try
            {
                var result = await Task.Run(() => LoginService.LoginControl(loginUser)).ConfigureAwait(true);
                if (!result)
                    return Ok("Kayıtlı değil");
            }
            catch (Exception ex)
            {
                return Problem($@"Error occured:{ex.Message}");
            }
            return Ok("OK");
        }

    }
}
