using Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace DataAccess {
    public class DBUser : IDBUser {
        /// <summary>
        /// Persists User object.
        /// </summary>
        /// <param name="user">The user.</param>
        public void CreateUser(User user) {
            List<int> ids = new List<int>();
            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //First persist account data in the database and get the id
                        cmd.CommandText = "INSERT INTO Account (Balance) OUTPUT INSERTED.ID VALUES (@balance)";
                        cmd.Parameters.AddWithValue("balance", user.Account.Balance);
                        user.Account.Id = (int)cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        //Persist the user data in the database
                        cmd.CommandText = "INSERT INTO [User] (FirstName, LastName, Username, Email, PhoneNumber, AccountId) OUTPUT INSERTED.ID VALUES (@firstName, @lastName, @userName, @email, @phoneNumber, @accountId)";
                        cmd.Parameters.AddWithValue("firstName", user.FirstName);
                        cmd.Parameters.AddWithValue("lastName", user.LastName);
                        cmd.Parameters.AddWithValue("userName", user.UserName);
                        cmd.Parameters.AddWithValue("email", user.Email);
                        cmd.Parameters.AddWithValue("phoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("accountId", user.Account.Id);
                        user.Id = (int)cmd.ExecuteScalar();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }

        }
        /// <summary>
        /// Gets a user on a unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The found user</returns>
        public User GetUser(int id) {
            User user = null;
            int accountId = 0;
            SqlDataReader reader;

            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Get the user data from the database and instantiate user
                        cmd.CommandText = "SELECT Id, FirstName, LastName, Username, Email, phoneNumber, AccountId FROM [User] where Id = @id";
                        cmd.Parameters.AddWithValue("id", id);
                        reader = cmd.ExecuteReader();

                        while (reader.Read()) {
                            user = new User {
                                Id = int.Parse(reader["Id"].ToString()),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                UserName = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["phoneNumber"].ToString()
                            };
                            accountId = int.Parse(reader["AccountId"].ToString());
                        }

                        cmd.Parameters.Clear();
                        reader.Close();

                        //Get the account data from the database and set account on user
                        cmd.CommandText = "SELECT Id, Balance FROM Account where Id = @id";
                        cmd.Parameters.AddWithValue("id", accountId);
                        reader = cmd.ExecuteReader();

                        while (reader.Read()) {
                            Account account = new Account {
                                Id = int.Parse(reader["Id"].ToString()),
                                Balance = decimal.Parse(reader["Balance"].ToString())
                            };
                            user.Account = account;
                        }
                        reader.Close();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }
            //Add bets to user
            if (user != null) {
                user.Bets = GetBets(user);
            }
            return user;
        }
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetAllUsers() {
            List<User> userList = new List<User>();
            User user = null;
            SqlDataReader reader;

            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Get all user data from the database
                        cmd.CommandText = "SELECT Id, FirstName, LastName, Username, Email, phoneNumber, AccountId FROM [User]";
                        reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            user = new User {
                                Id = int.Parse(reader["Id"].ToString()),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                UserName = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["phoneNumber"].ToString()
                            };
                            if (user.Id > 1) {
                                userList.Add(user);
                            }
                        }

                        cmd.Parameters.Clear();
                        reader.Close();

                        //Get account for each user and add it to user
                        foreach (User u in userList) {
                            cmd.CommandText = "SELECT A.Id, A.Balance FROM Account A, [User] U WHERE A.Id = u.AccountId AND U.Id = @id";
                            cmd.Parameters.AddWithValue("id", u.Id);
                            reader = cmd.ExecuteReader();
                            reader.Read();
                            Account account = new Account {
                                Id = int.Parse(reader["Id"].ToString()),
                                Balance = decimal.Parse(reader["Balance"].ToString())
                            };
                            u.Account = account;
                            cmd.Parameters.Clear();
                            reader.Close();
                        }

                    }
                    //Add bets to user
                    foreach (User u in userList) {
                        u.Bets = GetBets(u);
                    }
                    //Close connection for good measure
                    conn.Close();
                }

                //Close scope for good measure
                scope.Complete();
            }
            return userList;
        }
        /// <summary>
        /// Deletes a persisted user however the users bets is not deleted. The userId on the user's bets is set to 1 for statistical purposes.
        /// </summary>
        /// <param name="user">The user.</param>
        public void DeleteUser(User user) {
            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {
                    
                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Delete users bets in database
                        cmd.CommandText = "UPDATE Bet SET UserId = @UserId WHERE UserId = @id";
                        cmd.Parameters.AddWithValue("UserId", 1);
                        cmd.Parameters.AddWithValue("id", user.Id);
                        cmd.ExecuteNonQuery();

                        cmd.Parameters.Clear();
                        //Delete user data in database
                        cmd.CommandText = "DELETE FROM [User] WHERE id = @id; DBCC CHECKIDENT (@tableName, RESEED);";
                        cmd.Parameters.AddWithValue("id", user.Id);
                        cmd.Parameters.AddWithValue("tableName", "[User]");
                        cmd.ExecuteNonQuery();

                        cmd.Parameters.Clear();
                        //Delete users account data in database
                        cmd.CommandText = "DELETE FROM Account WHERE Id = @id; DBCC CHECKIDENT (@tableName, RESEED);";
                        cmd.Parameters.AddWithValue("id", user.Account.Id);
                        cmd.Parameters.AddWithValue("tableName", "Account");
                        cmd.ExecuteNonQuery();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }

        }
        /// <summary>
        /// Updates information about user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void UpdateUser(User user) {
            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Update user data in the database
                        cmd.CommandText = "UPDATE [User] SET FirstName = @firstName, LastName = @lastName, Username = @userName, Email = @email, PhoneNumber = @phoneNumber WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", user.Id);
                        cmd.Parameters.AddWithValue("firstName", user.FirstName);
                        cmd.Parameters.AddWithValue("lastName", user.LastName);
                        cmd.Parameters.AddWithValue("userName", user.UserName);
                        cmd.Parameters.AddWithValue("email", user.Email);
                        cmd.Parameters.AddWithValue("phoneNumber", user.PhoneNumber);
                        cmd.ExecuteNonQuery();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }

        }
        /// <summary>
        /// Updates the account on a specific user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void UpdateAccount(User user) {
            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Update account data in the database
                        cmd.CommandText = "UPDATE Account SET Balance = @balance WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", user.Account.Id);
                        cmd.Parameters.AddWithValue("balance", user.Account.Balance);
                        cmd.ExecuteNonQuery();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }

        }
        /// <summary>
        /// Adds a bet to user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="bet">The bet.</param>
        public void AddBetToUser(User user, Bet bet) {
            try {
                //Set transaction options with isolation level
                TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                //Create transaction scope with options
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                    //Create sqlconnection and open it
                    using (SqlConnection conn = DBConnection.GetSqlConnection()) {
                        conn.Open();
                        //Create sqlcommand
                        using (SqlCommand cmd = conn.CreateCommand()) {
                            int BetId = 0;
                            //Add bets to user 
                            cmd.CommandText = "INSERT INTO Bet (Amount, Odds, UserId) OUTPUT INSERTED.ID VALUES (@amount, @Odds, @UserId)";
                            cmd.Parameters.AddWithValue("amount", bet.Amount);
                            cmd.Parameters.AddWithValue("odds", bet.Odds);
                            cmd.Parameters.AddWithValue("UserId", user.Id);
                            BetId = (int)cmd.ExecuteScalar();
                            bet.Id = BetId;
                            cmd.Parameters.Clear();

                            if (bet.Type is Match) {
                                cmd.CommandText = "INSERT INTO BetsOnMatch (MatchId, BetId) OUTPUT INSERTED.ID VALUES (@matchId, @betId)";
                                cmd.Parameters.AddWithValue("matchId", (bet.Type as Match).Id);
                                cmd.Parameters.AddWithValue("betId", bet.Id);
                                int BomId = (int)cmd.ExecuteScalar();

                                cmd.Parameters.Clear();

                                cmd.CommandText = "INSERT INTO TeamsOnBetsOnMatch(TeamId, BomId) VALUES(@TeamId, @BomId)";
                                cmd.Parameters.AddWithValue("TeamId", (bet.WinCondition as Team).Id);
                                cmd.Parameters.AddWithValue("BomId", BomId);
                                cmd.ExecuteNonQuery();

                            }
                            if (bet.Type is Event) {
                                cmd.CommandText = "INSERT INTO BetsOnEvent (EventId, BetId) VALUES (@eventId, @betId)";
                                cmd.Parameters.AddWithValue("eventId", (bet.Type as Event).Id);
                                cmd.Parameters.AddWithValue("betId", bet.Id);
                                cmd.ExecuteNonQuery();
                            }
                            if (bet.Type is Team) {
                                cmd.CommandText = "INSERT INTO BetsOnTeam (TeamId, BetId) VALUES (@teamId, @betId)";
                                cmd.Parameters.AddWithValue("TeamId", (bet.Type as Team).Id);
                                cmd.Parameters.AddWithValue("betId", bet.Id);
                                cmd.ExecuteNonQuery();
                            }
                            if (bet.Type is Player) {
                                cmd.CommandText = "INSERT INTO BetsOnPlayer (PlayerId, BetId) VALUES (@playerId, @betId)";
                                cmd.Parameters.AddWithValue("uplayerId", (bet.Type as Player).Id);
                                cmd.Parameters.AddWithValue("betId", bet.Id);
                                cmd.ExecuteNonQuery();
                            }

                        }
                        //Close connection for good measure
                        conn.Close();
                    }
                    //Close scope for good measure
                    scope.Complete();
                }
            } catch (System.Exception) {

                throw;
            }
        }
        /// <summary>
        /// Gets the bets from a given User.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>List of Bets</returns>
        public List<Bet> GetBets(User user) {
            List<Bet> betsList = new List<Bet>();
            Bet bet = null;
            int typeId = -1;
            int bomId = -1;
            int teamId = -1;
            string typeString = "";
            SqlDataReader reader;

            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Get all bets on user from the database
                        cmd.CommandText = "SELECT b.id, b.amount, b.odds, b.Verified FROM Bet b, [User] u WHERE b.UserId = u.Id AND b.UserId = @userId";
                        cmd.Parameters.AddWithValue("userId", user.Id);
                        reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            bet = new Bet {
                                Id = int.Parse(reader["Id"].ToString()),
                                Amount = decimal.Parse(reader["Amount"].ToString()),
                                Odds = decimal.Parse(reader["Odds"].ToString()),
                                Verified = bool.Parse(reader["Verified"].ToString())
                            };
                            betsList.Add(bet);
                        }
                        reader.Close();

                        foreach (Bet b in betsList) {
                            if (b != null) {
                                typeString = "";
                                cmd.Parameters.Clear();
                                cmd.CommandText = "SELECT e.eventId AS eventId, m.matchId AS matchId, t.teamId AS teamId, p.playerId AS playerId, tobom.TeamId as team2Id FROM bet AS b LEFT JOIN betsonevent AS e ON e.betid = b.Id LEFT JOIN BetsOnMatch AS m ON m.betid = b.Id LEFT JOIN betsonteam AS t ON t.betid = b.Id LEFT JOIN TeamsOnBetsOnMatch as tobom on m.Id = tobom.BomId LEFT JOIN betsonplayer AS p ON p.betid = b.Id WHERE b.id =  @betId";
                                cmd.Parameters.AddWithValue("betId", b.Id);
                                reader = cmd.ExecuteReader();
                                if (reader.Read()) {
                                    if (!string.IsNullOrEmpty(reader["matchId"].ToString())) {
                                        typeId = int.Parse(reader["matchId"].ToString());
                                        bomId = int.Parse(reader["team2Id"].ToString());
                                        typeString = "matchId";
                                    }
                                    else if (!string.IsNullOrEmpty(reader["eventId"].ToString())) {
                                        typeId = int.Parse(reader["eventId"].ToString());
                                        typeString = "eventId";
                                    }
                                    else if (!string.IsNullOrEmpty(reader["teamId"].ToString())) {
                                        typeId = int.Parse(reader["teamId"].ToString());
                                        typeString = "teamId";
                                    }
                                    else if (!string.IsNullOrEmpty(reader["playerId"].ToString())) {
                                        typeId = int.Parse(reader["playerId"].ToString());
                                        typeString = "playerId";
                                    }
                                }

                                reader.Close();

                                if (!typeString.Equals("")) {
                                    if (typeString.Equals("matchId")) {
                                        reader.Close();
                                        DBTeam dBTeam = new DBTeam();
                                        Team team = dBTeam.GetTeam(bomId);

                                        DBMatch dBMatch = new DBMatch();
                                        Match match = dBMatch.GetMatch(typeId);
                                        b.Type = match;
                                        b.WinCondition = team;
                                    }
                                    else if (typeString.Equals("eventId")) {
                                        DBEvent dBEvent = new DBEvent();
                                        Event betEvent = dBEvent.GetEvent(typeId);
                                        b.Type = betEvent;
                                    }
                                    else if (typeString.Equals("teamId")) {
                                        DBTeam dBTeam = new DBTeam();
                                        Team team = dBTeam.GetTeam(typeId);
                                        b.Type = team;
                                    }
                                    else if (typeString.Equals("playerId")) {
                                        DBPlayer dBPlayer = new DBPlayer();
                                        Player player = dBPlayer.GetPlayer(typeId);
                                        b.Type = player;
                                    }
                                }
                            }
                        }
                        //Close connection for good measure
                        conn.Close();
                    }
                    //Close scope for good measure
                    scope.Complete();
                }
                return betsList;
            }

        }
        /// <summary>
        /// Gets all users with won bets for a given match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>List of Users</returns>
        public List<User> GetBetsOnUserOnMatch(Match match) {
            List<Bet> betsList = new List<Bet>();
            List<User> users = new List<User>();
            Bet bet = null;
            List<int> userIds = new List<int>();
            List<int> betIds = new List<int>();
            SqlDataReader reader;


            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {

                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        //Get all bets on user from the database
                        cmd.CommandText = "SELECT b.id as BetId, b.Amount as Amount, b.Odds as Odds, b.Verified as Verified, b.UserId as UserId FROM Bet as b INNER JOIN BetsOnMatch as bom ON b.Id = bom.BetId INNER JOIN TeamsOnBetsOnMatch as tobom ON tobom.BomId = bom.Id WHERE bom.MatchId = @MatchId AND tobom.TeamId = @WinnerId; ";
                        cmd.Parameters.AddWithValue("matchId", match.Id);
                        cmd.Parameters.AddWithValue("winnerId", match.Winner.Id);
                        reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            bet = new Bet {
                                Id = int.Parse(reader["BetId"].ToString()),
                                Amount = decimal.Parse(reader["Amount"].ToString()),
                                Odds = decimal.Parse(reader["Odds"].ToString()),
                                Verified = bool.Parse(reader["Verified"].ToString()),
                            };
                            betIds.Add(int.Parse(reader["BetId"].ToString()));
                            userIds.Add(int.Parse(reader["UserId"].ToString()));

                            betsList.Add(bet);
                        }
                        reader.Close();

                        foreach (int abe in userIds) {
                            if (abe != 1) {
                                users.Add(GetUser(abe));
                            }
                        }
                        foreach (User u in users) {
                            u.Bets.RemoveAll(item => !betsList.Contains(item));
                            u.Bets.RemoveAll(item => item.Verified == true);
                        }

                        foreach (Bet b in betsList) {
                            if (b != null) {
                                b.Type = match;
                                b.WinCondition = match.Winner;
                            }
                        }
                        //Close connection for good measure
                        conn.Close();
                    }
                    //Close scope for good measure
                    scope.Complete();
                }
                return users;
            }
        }
        /// <summary>
        /// Sets the boolean Verified on a persisted Bet.
        /// </summary>
        /// <param name="bet"></param>
        public void UpdateVerifiedOnBet(Bet bet) {
            //Set transaction options with isolation level
            TransactionOptions options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            //Create transaction scope with options
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options)) {
                //Create sqlconnection and open it
                using (SqlConnection conn = DBConnection.GetSqlConnection()) {
                    conn.Open();
                    //Create sqlcommand
                    using (SqlCommand cmd = conn.CreateCommand()) {
                        cmd.CommandText = "UPDATE Bet SET Verified = @Verified WHERE id = @id";
                        cmd.Parameters.AddWithValue("id", bet.Id);
                        if (bet.Verified) {
                            cmd.Parameters.AddWithValue("Verified", 1);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    //Close connection for good measure
                    conn.Close();
                }
                //Close scope for good measure
                scope.Complete();
            }
        }
    }
}

