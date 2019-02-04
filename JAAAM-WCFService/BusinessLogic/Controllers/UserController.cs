using Model;
using System.Collections.Generic;
using DataAccess;

namespace BusinessLogic.Controllers {
    public class UserController : IUserController {
        DBUser _dbUser;
        /// <summary>
        /// constructor to initialize _dbUser
        /// </summary>
        public UserController() {
            _dbUser = new DBUser();
        }
        /// <summary>
        /// Gets all users with won bets for a given match.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>List of users</returns>
        public List<User> GetBetsOnUserOnMatch(Match match) {
            return _dbUser.GetBetsOnUserOnMatch(match);
        }
        /// <summary>
        /// persists a user.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user) {
            _dbUser.CreateUser(user);
        }
        /// <summary>
        /// Gets a User by unique Identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found User</returns>
        public User GetUser(int id) {
           return _dbUser.GetUser(id);
        }
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetAllUsers() {
            return _dbUser.GetAllUsers();
        }
        /// <summary>
        /// Deletes the persisted user
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user) {
            if (user != null) {
                _dbUser.DeleteUser(user);
            }
        }
        /// <summary>
        /// Update information about the user
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user) {
            _dbUser.UpdateUser(user);
        }
        /// <summary>
        /// Updates a persisted users Account balance
        /// </summary>
        /// <param name="user"></param>
        public void UpdateAccount(User user) {
           _dbUser.UpdateAccount(user);
        }
        /// <summary>
        /// Adds a bet to Users and persists it. Returns true if the transaction succeeds.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bet"></param>
        /// <returns>Boolean</returns>
        public bool AddBetToUser(User user, Bet bet) {
            try {
                bool toReturn;
                if (user.Account.Balance >= bet.Amount) {
                    user.Account.Balance = user.Account.Balance - bet.Amount;
                    UpdateAccount(user);
                    bet.Odds = bet.Odds / 100M;
                    _dbUser.AddBetToUser(user, bet);
                    toReturn = true;
                } else {
                    toReturn = false;
                }
                return toReturn;
            } catch (System.Exception) {

                throw;
            }
        }
        /// <summary>
        /// Get all bets for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of Bets</returns>
        public List<Bet> GetBets(User user) {
            return _dbUser.GetBets(user);
        }
        /// <summary>
        /// Sets the boolean Verified on a persisted Bet.
        /// </summary>
        /// <param name="bet"></param>
        public void UpdateVerifiedOnBet(Bet bet) {
            _dbUser.UpdateVerifiedOnBet(bet);
        }
    }
}
