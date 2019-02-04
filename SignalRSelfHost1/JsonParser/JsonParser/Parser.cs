using BusinessLogic.Controllers;
using DataLogic;
using Model;
using Newtonsoft.Json;
using OddsCalculator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace JsonParser {
    public class Parser {
        Calculator Calculator;
        ConsoleColor color;
        TeamController teamController;
        MatchController matchController;
        UserController userController;
        int sleepTime = 1000;
        public Parser(ConsoleColor consoleColor) {
            Calculator = new Calculator();
            color = consoleColor;
            matchController = new MatchController();
            userController = new UserController();
            teamController = new TeamController();

        }
        /// <summary>
        /// Parses CSGO Demos in JSON file. jsonpath has to follow the naming convention "team1Name vs. team2Name" and be placed in the folder matches in order to read automatically.
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <param name="match"></param>
        public void ReadJsonObject(string jsonPath, Match match) {

            StreamReader r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            if (Properties.Settings.Default.Forever) {
                while (true) {
                    ReadMatch(match, jsonObj);
                }
            }
            else {
                ReadMatch(match, jsonObj);
                Thread.Sleep(sleepTime * 1000);
            }



        }
        /// <summary>
        /// Used by the ReadJsonObject method. Builds round objects used to calculate odds.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="jsonObj"></param>
        private void ReadMatch(Match match, dynamic jsonObj) {
            Round roundObj = null;
            Console.ForegroundColor = color;
            Console.WriteLine($"#### #### #### - {match.Name} - #### #### ####");
            foreach (var dataFromRound in jsonObj.rounds) {
                roundObj = new Round {
                    Number = dataFromRound.number,
                    WinnerSide = dataFromRound.winner_side,
                    Match = match
                };

                foreach (Team t in match.Teams) {
                    if (t.Name == (string)dataFromRound.winner_name) {
                        roundObj.Winner = t;
                    }
                }

                foreach (var killFromRound in dataFromRound.kills) {
                    Kill killObj = new Kill {
                        KillerName = killFromRound.killer_name,
                        KilledName = killFromRound.killed_name
                    };
                    roundObj.Kills.Add(killObj);
                }
                ConsolePrintRounds(roundObj);
                Calculator.CalculateOdds(roundObj);
            }
            int winnerId = jsonObj.team_winner.id;
            OnMatchEnd(winnerId, match);

        }
        /// <summary>
        /// Begins blockchain validation for the won bets.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="match"></param>
        private void OnMatchEnd(int id, Match match) {
            Team team = teamController.GetTeam(id);
            match.Winner = team;
            match.Held = true;
            matchController.EditMatch(match);
            Thread.Sleep(1500);
            List<User> usersWithWonBets = userController.GetBetsOnUserOnMatch(match);
            Console.WriteLine($"GZ {team.Name} med jeres win");
            BlockchainController bctr = new BlockchainController();
            if (bctr.CheckBets(usersWithWonBets)) {
                foreach (User user in usersWithWonBets) {
                    foreach (Bet bet in user.Bets) {
                        user.Account.Balance = user.Account.Balance + (bet.Amount * bet.Odds);
                        userController.UpdateAccount(user);
                        bet.Verified = true;
                        userController.UpdateVerifiedOnBet(bet);
                    }
                }
            }
            Thread.Sleep(100000);
        }

        /// <summary>
        /// Prints relevant information to console for debugging and watching purposes.
        /// </summary>
        /// <param name="round"></param>
        public void ConsolePrintRounds(Round round) {
            Console.ForegroundColor = color;
            Console.WriteLine("Round " + round.Number + " has started!");
            Thread.Sleep(sleepTime);
            foreach (var kill in round.Kills) {
                Console.ForegroundColor = color;
                Console.WriteLine(kill.KillerName + " killed " + kill.KilledName);
                Thread.Sleep(sleepTime);
            }
            Console.ForegroundColor = color;
            Console.WriteLine("Round " + round.Number + " ended!");
        }
    }

}
