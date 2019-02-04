using JaaamBlockchain;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLogic2 {
    public class BlockchainController {

        static void Main(string[] args) {
            
        }

        public bool CheckBets(List<User> users) {
            bool returnValue = true;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            Parallel.ForEach(users, parallelOptions, (user) => {
                foreach (Bet bet in user.Bets) {
                    Chain chain = new Chain(bet, user);
                    chain.Difficulty = 2;
                    for (int i = 1; i < 100; i++) {
                        Block block = new Block(i, DateTime.Now.ToString("d"), "Block #" + i, "", bet, user);
                        chain.MineAndAddBlock(block);
                    }

                    try {
                        chain.Validate();
                    }
                    catch (Exception e) {
                        Console.WriteLine("{0}", e);
                    }
                }

            });
            return returnValue;
        }
    }
}
