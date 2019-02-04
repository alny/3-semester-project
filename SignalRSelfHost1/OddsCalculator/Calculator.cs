using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRSelfHost1;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;
using Model;
using System.Diagnostics;

namespace OddsCalculator
{
    public class Calculator {
        public Decimal CurrentTeam0Odds { get; set; }
        public Decimal CurrentTeam1Odds { get; set; }
        private static IHubProxy MatchHubProxy { get; set; }

        const string ServerURI = "http://localhost:8080";
        private static HubConnection Connection { get; set; }
        List<Decimal> odds = new List<decimal>();

        public Calculator() {
            HubConnection();
        }   
        /// <summary>
        /// Algorithm to calculate odds based on round win. invokes BroadcastRoed(Round round) and BroadcastOdds(List<Odds> odds, Round round) on the matchHub.
        /// </summary>
        /// <param name="round"></param>
        public void CalculateOdds(Round round) {
            MatchHubProxy.Invoke("BroadcastRound", round);

            if (round.Number == 1) {
                odds.Add(CurrentTeam0Odds = 1.5M);
                odds.Add(CurrentTeam1Odds = 1.5M);
            }

            if (round.Winner == round.Match.Teams[0]) {
               odds[0] = (CurrentTeam0Odds = CurrentTeam0Odds - ((CurrentTeam0Odds - 1M) * 0.15M));
               odds[1] = (CurrentTeam1Odds = CurrentTeam1Odds + ((CurrentTeam1Odds - 1M) * 0.15M));
            } else {
                odds[1] = (CurrentTeam1Odds = CurrentTeam1Odds - ((CurrentTeam1Odds - 1M) * 0.15M));
                odds[0] = (CurrentTeam0Odds = CurrentTeam0Odds + ((CurrentTeam0Odds - 1M) * 0.15M));
            }
            MatchHubProxy.Invoke("BroadcastOdds", odds, round);

        }
        /// <summary>
        /// Method to connect to SignalR MatchHub
        /// </summary>
        private static async void HubConnection() {
            Connection = new HubConnection(ServerURI);
            MatchHubProxy = Connection.CreateHubProxy("MatchHub");

            try {
                await Connection.Start();
                Console.WriteLine("Started");
            }
            catch (HttpRequestException) {
                Console.WriteLine("Unable to connect to server: Start server before connecting clients.");
                return;
            }
        }
    }
}
