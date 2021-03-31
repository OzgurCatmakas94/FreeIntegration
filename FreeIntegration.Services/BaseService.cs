using Microsoft.Extensions.Configuration;

namespace FreeIntegration.Services
{
    public class BaseService
    {
        #region Ctor
        public BaseService(IConfiguration configuration)
        {
            Configuration = configuration;
 
        }
        #endregion
        #region private properties,
        /// <summary>
        /// Ayarlar için kullanılır.
        /// </summary>
        protected IConfiguration Configuration { get; set; }

        #endregion
    }
}
