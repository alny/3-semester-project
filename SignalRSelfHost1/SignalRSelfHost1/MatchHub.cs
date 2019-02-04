using Microsoft.AspNet.SignalR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSelfHost1 {
    public class MatchHub : Hub {
        /// <summary>
        /// Joins a group for a match by matchName.
        /// </summary>
        /// <param name="matchName"></param>
        /// <returns>Task</returns>
        public Task JoinMatch(string matchName) {
            return Groups.Add(Context.ConnectionId, matchName);
        }
        /// <summary>
        /// Leaves a group for a match by matchName
        /// </summary>
        /// <param name="matchName"></param>
        /// <returns>Task</returns>
        public Task LeaveMatch(string matchName) {
            return Groups.Remove(Context.ConnectionId, matchName);
        }
        /// <summary>
        /// Awaits a group based on the round.Match.Name from round objects in order to Broadcast kills.
        /// </summary>
        /// <param name="round"></param>
        public async Task BroadcastRound(Round round) {
             await Clients.Group(round.Match.Name).addRound(round);
             await BroadcastKill(round);
        }
        /// <summary>
        /// Broadcasts kills to a group based on round.Match.Name
        /// </summary>
        /// <param name="round"></param>
        public async Task BroadcastKill(Round round) {
            foreach (var kill in round.Kills) {
                await Clients.Group(round.Match.Name).addKill(kill);
                System.Threading.Thread.Sleep(500);
            }
        }
        /// <summary>
        /// Method to live update odds in a group for a match 
        /// </summary>
        /// <param name="odds"></param>
        /// <param name="round"></param>
        public async Task BroadcastOdds(List<Decimal> odds, Round round) {
            Odds oddsObj = new Odds {
                Match = round.Match,
                Odds1 = odds[0],
                Odds2 = odds[1]
            };
            await Clients.Group(round.Match.Name).addOdds(oddsObj);
        }
        
    }
}

