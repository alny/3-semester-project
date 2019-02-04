using JAAAM_WebClient.Models;
using JAAAM_WebClient.WCFService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAAAM_WebClient.Controllers {
    public class MatchController : Controller {
        ServiceClient client = new ServiceClient();
        public ActionResult Matches() {

            MatchViewModel mvm = new MatchViewModel() {
                Matches = client.GetMatches().ToList(),
                User = client.GetUser(5)
            };
            return View(mvm);
        }

        [HttpPost]
        public JsonResult Matches(BetData betData) {
            bool success;
            string UserMessage = "";
            string DevMessage = "";
            try {
                Bet bet = new Bet {
                    Amount = betData.Amount,
                    Odds = (decimal.Parse(betData.Odds)),
                    Type = client.GetMatch(betData.MatchId),
                    WinCondition = client.GetTeam(betData.TeamId)
                };
                client.AddBetToUser(client.GetUser(betData.UserId), bet);

                success = true;
                UserMessage = "Bet was placed successfully";
            } catch (Exception ex) {
                UserMessage = "Bet not placed, try again";
                DevMessage = ex.StackTrace;
                success = false;
            }

            return Json(new {success, UserMessage, DevMessage });
        }

        public List<Match> GetMatches() {
            return client.GetMatches().ToList();
        }
    }
}