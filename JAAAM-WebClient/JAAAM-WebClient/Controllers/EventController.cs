using JAAAM_WebClient.WCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAAAM_WebClient.Controllers {
    public class EventController : Controller {
        ServiceClient client = new ServiceClient();
        // GET: Event
        public ActionResult Index() {

            IEnumerable<Event> Events = client.GetEvents();

            return View(Events.ToList());
        }

        public ActionResult EventDetails(int id) {
            Event eventt = client.GetEvent(id);
            
            return View(eventt);
        }

        public PartialViewResult _MatchDetails() {
            
            return PartialView(client.GetMatches().ToList());
        }


    }
}