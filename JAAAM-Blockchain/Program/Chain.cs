using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace JaaamBlockchain {
    //Jeg vil gerne køres paralelt når min controller skal lave mig :) <3
    public class Chain {
        List<Block> _chain;
        public int Difficulty { get; set; }
        /// <summary>
        /// constructor to create a chain.
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="user"></param>
        public Chain(Bet bet, User user) {
            _chain = new List<Block>();
            _chain.Add(CreateGenesisBlock(bet, user));
            Difficulty = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        public void MineAndAddBlock(Block block) {
            block.PrevBlockHash = GetLastBlock().BlockHash;

            block.MineBlock(Difficulty);
            _chain.Add(block);

        }
        /// <summary>
        /// Method to validate the blocks in the chain.
        /// </summary>
        /// <returns>returns true if the entire chain is valid</returns>
        public bool Validate() {
            bool toReturn = true;
            for (int i = 1; i < _chain.Count; i++) {
                Block currBlock = _chain[i];
                Block prevBlock = _chain[i - 1];
                if ((currBlock.BlockHash != (currBlock.CalculateHash())) && toReturn) {
                    toReturn = false;
                    Debug.Write("Current hash not correct, chain is invalid!");
                }
                if ((!(currBlock.GetBetBlockHash().Equals(GetGenesisBlockBetHash()))) && toReturn) {
                    toReturn = false;
                    Debug.Write("Mismatched BetBlock hashes");
                }
                if ((currBlock.PrevBlockHash != prevBlock.BlockHash) && toReturn) {
                    toReturn = false;
                    Debug.Write("Previous hash on current block does not match hash of previous block, chain is invalid!");
                }
            }
            Console.WriteLine("\n" + _chain.Count + " blocks OK, chain valid!" +  Environment.NewLine +  "Status: " + toReturn.ToString());
            Thread.Sleep(10);
            return toReturn;
        }
        /// <summary>
        /// Method to print the block hashes in the chain.
        /// </summary>
        public void PrintChain() {
            foreach (Block block in _chain) {
                Debug.WriteLine(block.ToString());
            }
        }
        /// <summary>
        /// Method to create the genesis block in the chain
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="user"></param>
        /// <returns>Created block</returns>
        private Block CreateGenesisBlock(Bet bet, User user) {
            return new Block(0, DateTime.Now.ToString("d"), "Block #" + 0, "", bet, user);
        }
        /// <summary>
        ///  Get the last block in the chain
        /// </summary>
        /// <returns>Last block in chain.</returns>
        private Block GetLastBlock() {
            return _chain.Last();
        }
        /// <summary>
        /// Method to get genesis block hash
        /// </summary>
        /// <returns>genesis block hash</returns>
        public String GetGenesisBlockBetHash() {
            return _chain.First().GetBetBlockHash();
        }
    }
}
