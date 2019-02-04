using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using  System.Web.Mvc;
using JAAAM_WebClient.Models;
using JAAAM_WebClient.WCFService;
using System.Runtime;

namespace JAAAM_WebClient.Controllers {
    public class UserController : Controller {
        ServiceClient client = new ServiceClient();

        public ActionResult Index() {
            return View(client.GetUser(5));
        }

        [HttpPost]
        public PartialViewResult _BetDetails(string data) {
            return PartialView();
        }

        public PartialViewResult _BetDetails(Team team, decimal odds) {
            return PartialView();
        }
    }

}


