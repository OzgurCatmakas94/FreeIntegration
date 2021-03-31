using FreeIntegration.Services.LoginService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FreeIntegration.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region Ctor
        public BaseController(IConfiguration configuration,ILoginService loginService)
        {
            Configuration = configuration;
            LoginService = loginService;
        }
        #endregion
        #region Protected Properties
        /// <summary>
        /// Login işlemleri servisi
        /// </summary>
        protected ILoginService LoginService { get; set; }
        /// <summary>
        /// Ayar servisi
        /// </summary>
        protected IConfiguration Configuration { get; set; }
        #endregion
    }
}
