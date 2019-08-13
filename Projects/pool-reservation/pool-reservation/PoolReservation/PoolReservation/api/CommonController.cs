using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoolReservation.api
{
    public class CommonController : Controller
    {
        public ActionResult Index()
        {
            var model = new SettingsModel { GoogleApiKey = ConfigurationManager.AppSettings["GoogleMapsKey"] };

            return View(model);
        }
    }

    public class SettingsModel
    {
        public string GoogleApiKey { get; set; }

    }
}