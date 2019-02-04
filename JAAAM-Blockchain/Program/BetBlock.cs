using Model;
using System.Security.Cryptography;
using System.Text;

namespace JaaamBlockchain {
    public class BetBlock {
        private User _User;
        private Bet _Bet;
        private SHA256 _Hash = SHA256.Create();

        /// <summary>
        /// Constructor to create a BetBlock
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bet"></param>
        public BetBlock(User user, Bet bet) {
            _User = user;
            _Bet = bet;
        }
        /// <summary>
        /// Method to calculate hash for a betblock.
        /// </summary>
        /// <returns>calculated hash</returns>
        public string CalculateHash() {
            return GenerateHash(_User.ToString() + _Bet.ToString());
        }
        /// <summary>
        /// Generates hash.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Generated hash</returns>
        private string GenerateHash(string source) {
            StringBuilder stringBuilder = new StringBuilder();
            Encoding encoding = Encoding.UTF8;

            byte[] hashes = _Hash.ComputeHash(encoding.GetBytes(source));

            foreach (byte hash in hashes) {
                stringBuilder.Append(hash.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
