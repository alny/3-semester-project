using JaaamBlockchain;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BusinessLogic {
    public class BlockchainController {
        /// <summary>
        /// Method to validate won bets on a list of users. Utilizes threading to speed up the process. 
        /// </summary>
        /// <param name="users"></param>
        /// <returns>returns true if all bets have been validated</returns>
        public bool CheckBets(List<User> users) {
            bool returnValue = true;
            ParallelOptions parallelOptions = new ParallelOptions {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            Debug.WriteLine("Users with won bets: " + users.Count);
            Parallel.ForEach(users, parallelOptions, (user) => {
                foreach (Bet bet in user.Bets) {
                    Chain chain = new Chain(bet, user);
                    chain.Difficulty = 2;
                    for (int i = 1; i < 100; i++) {
                        Block block = new Block(i, DateTime.Now.ToString("d"), "Block #" + i, "", bet, user);
                        chain.MineAndAddBlock(block);
                    }
                    try {
                        if (chain.Validate()) {
                            Debug.WriteLine($"{bet.ToString()} has been validated");
                        }
                        else {
                            Debug.WriteLine($"{bet.ToString()} did not validate");
                            returnValue = false;
                        }

                    }
                    catch (Exception e) {
                        Debug.WriteLine("{0}", e);
                    }
                }

            });
            return returnValue;
        }

    }
}
