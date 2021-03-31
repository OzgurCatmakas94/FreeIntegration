using FreeIntegration.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeIntegration.Web.Controllers.Settings
{
    public class SettingsController : Controller
    {
        public IActionResult TrendyolSettings(SettingsDT trendyolSettingsDT)
        {
            if (ModelState.IsValid)
            {

            }
            return View("TrendyolSettings");
        }
    }
}
