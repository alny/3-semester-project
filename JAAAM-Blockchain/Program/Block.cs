using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JaaamBlockchain {
    public class Block {
        int BlockIndex { get; set; }
        public string BlockHash { get; set; }
        string Timestamp { get; set; }
        public string Ident { get; set; }
        public string PrevBlockHash { get; set; }
        public BetBlock betBlock { get; set; }    
        private SHA256 Hash = SHA256.Create();
        private int _shaSalt;
        /// <summary>
        /// Constructor to create a Block
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="timestamp"></param>
        /// <param name="ident"></param>
        /// <param name="prevBlockHash"></param>
        /// <param name="bet"></param>
        /// <param name="user"></param>
        public Block(int blockIndex, string timestamp, string ident, string prevBlockHash, Bet bet, User user) {
            BlockIndex = blockIndex;
            BlockHash = CalculateHash();
            Timestamp = timestamp;
            Ident = ident;
            PrevBlockHash = prevBlockHash;
            _shaSalt = 69;
            betBlock = new BetBlock(user, bet);
        }
        /// <summary>
        /// Method to mine a blocks' hash
        /// </summary>
        /// <param name="difficulty"></param>
        public void MineBlock(int difficulty) {
            while (BlockHash.Substring(0, difficulty) != new string('0', difficulty)) {
                _shaSalt++;
                BlockHash = CalculateHash();
            }
            betBlock.CalculateHash();
        }
        /// <summary>
        /// Method to calculate block hash
        /// </summary>
        /// <returns>Calculated hash as string</returns>
        public string CalculateHash() {
            return GenerateHash(BlockIndex + PrevBlockHash + Timestamp + Ident + _shaSalt);
        }

        //public override string ToString() {
        //    return Ident;
        //}

        /// <summary>
        /// Method to generate hash
        /// </summary>
        /// <param name="source"></param>
        /// <returns>generated hash</returns>
        private string GenerateHash(string source) {
            StringBuilder stringBuilder = new StringBuilder();
            Encoding encoding = Encoding.UTF8;

            byte[] hashes = Hash.ComputeHash(encoding.GetBytes(source));

            foreach (byte hash in hashes) {
                stringBuilder.Append(hash.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
        /// <summary>
        /// method to get Block hash
        /// </summary>
        /// <returns>generated hash</returns>
        public string GetBetBlockHash() {
            return betBlock.CalculateHash();
        }
        /// <summary>
        /// override of ToString() to get relevant information.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return $" {Ident} + {BlockIndex} + {BlockHash} + {Timestamp} + {PrevBlockHash} + {_shaSalt}";
        }
    }
}
