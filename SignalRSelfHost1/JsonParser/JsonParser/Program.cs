using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Controllers;
using Model;

namespace JsonParser {
    public class Program {
        static int colorCount = 10;
        /// <summary>
        /// Starts matches
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {

            MatchController matchController = new MatchController();
            List<Match> matchList = matchController.GetMatches().ToList();
            

            foreach (Match match in matchList) {
                Parser parser = new Parser(GetConsoleColor());
                string path = $"../../Matches/{match.Name}.json";
                Thread newThread = new Thread(() => parser.ReadJsonObject(path, match));
                newThread.Start();
            }

        }
        /// <summary>
        /// Color codes text output per match up to 14 matches
        /// </summary>
        /// <returns></returns>
        static ConsoleColor GetConsoleColor(){
            ConsoleColor toReturn;
            if (colorCount == 13)
            {
                colorCount = 1;
                toReturn = ConsoleColor.Yellow;
            }
            else{
                toReturn = (ConsoleColor)colorCount;
                colorCount++;
            }
            return toReturn;              

        }
    }
}
