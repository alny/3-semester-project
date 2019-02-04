using Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BusinessLogic.Controllers;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using System;

namespace UnitTest {
    [TestClass]
    public class UserTest {
        UserController UserController;
        TeamController TeamController;
        PlayerController PlayerController;
        MatchController MatchController;
        EventController EventController;
        Account TestUserAccount;
        User TestUser;
        Team TestTeam1;
        Team TestTeam2;
        Player TestPlayer1, TestPlayer2, TestPlayer3, TestPlayer4, TestPlayer5, TestPlayer6, TestPlayer7, TestPlayer8, TestPlayer9, TestPlayer10;
        Map TestMap;
        Match TestMatch;
        Event TestEvent;
        Bet TestBet;

        [TestInitialize]
        public void TestInit() {
            int testTeam1Id = -1;
            int testTeam2Id = -1;
            int testMapId = -1;
            int testEventId = -1;

            //Instantiate controller
            UserController = new UserController();
            TeamController = new TeamController();
            PlayerController = new PlayerController();
            MatchController = new MatchController();
            EventController = new EventController();

            //Build test objects:

            //Account
            TestUserAccount = new Account {
                Balance = 1000.00M
            };

            //User
            TestUser = new User {
                Account = TestUserAccount,
                FirstName = "UserTestUser",
                LastName = "UserTestUser",
                UserName = "UserTestUser",
                Email = "UserTestUser",
                PhoneNumber = "12345678"
            };

            //Players
            TestPlayer1 = new Player {
                NickName = "TestPlayer1",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer2 = new Player {
                NickName = "TestPlayer2",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer3 = new Player {
                NickName = "TestPlayer3",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer4 = new Player {
                NickName = "TestPlayer4",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer5 = new Player {
                NickName = "TestPlayer5",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer6 = new Player {
                NickName = "TestPlayer6",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer7 = new Player {
                NickName = "TestPlayer7",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer8 = new Player {
                NickName = "TestPlayer8",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer9 = new Player {
                NickName = "TestPlayer9",
                Age = "99",
                Role = "Tester"
            };
            TestPlayer10 = new Player {
                NickName = "TestPlayer10",
                Age = "99",
                Role = "Tester"
            };

            //Team
            TestTeam1 = new Team {
                Name = "TeamTest1"
            };

            TestTeam2 = new Team {
                Name = "TeamTest2"
            };

            //Map
            TestMap = new Map {
                Name = "TestMap",
                IsActive = true
            };

            //Match
            TestMatch = new Match {
                Format = "BOTest",
                Winner = TestTeam1
            };

            //Event
            TestEvent = new Event {
                Name = "TestEvent",
                GameName = "TestGO",
                Type = "Test",
                Held = false
            };

            //Bet
            TestBet = new Bet {
                Amount = 500.00M,
                Odds = 1.78M,
                Type = TestMatch,
                WinCondition = TestTeam1
            };

            //Create user
            UserController.CreateUser(TestUser);

            //Create teams
            testTeam1Id = TeamController.CreateTeam(TestTeam1);
            testTeam2Id = TeamController.CreateTeam(TestTeam2);

            //Add players to team 1
            TestTeam1.Players.Add(TestPlayer1);
            TestTeam1.Players.Add(TestPlayer2);
            TestTeam1.Players.Add(TestPlayer3);
            TestTeam1.Players.Add(TestPlayer4);
            TestTeam1.Players.Add(TestPlayer5);

            //Add team to players and create them
            foreach(Player p in TestTeam1.Players) {
                p.TeamId = testTeam1Id;
                p.Id = PlayerController.CreatePlayer(p);
                Debug.WriteLine("T1: " + p.Id);
            }

            //Update team with players
            TeamController.EditTeam(TestTeam1);

            //Add players to team 2
            TestTeam2.Players.Add(TestPlayer6);
            TestTeam2.Players.Add(TestPlayer7);
            TestTeam2.Players.Add(TestPlayer8);
            TestTeam2.Players.Add(TestPlayer9);
            TestTeam2.Players.Add(TestPlayer10);

            //Add team to players and create them
            foreach (Player p in TestTeam2.Players) {
                p.TeamId = testTeam2Id;
                p.Id = PlayerController.CreatePlayer(p);
                Debug.WriteLine("T2: " + p.Id);
            }

            //Update team with players
            TeamController.EditTeam(TestTeam2);

            //Persist map
            testMapId = MatchController.CreateMap(TestMap);
            TestMap.Id = testMapId;

            //Add map to match
            TestMatch.Maps.Add(TestMap);
            TestMatch.Teams.Add(TestTeam1);
            TestMatch.Teams.Add(TestTeam2);

            //Persist event
            testEventId = EventController.CreateEvent(TestEvent);

            //Persist match
            TestMatch.EventId = testEventId;
            MatchController.CreateMatch(TestMatch);
            
            //Add match to event
            TestEvent.Matches.Add(TestMatch);

        }

        [TestCleanup]
        public void TestCleanUp() {
            //Delete Bet
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "DELETE FROM TeamsOnBetsOnMatch WHERE TeamId=@id";
                        command.Parameters.AddWithValue("id", TestTeam1.Id);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        command.CommandText = "DELETE FROM BetsOnmatch WHERE BetId=@id";
                        command.Parameters.AddWithValue("id", TestBet.Id);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        command.CommandText = "DELETE FROM Bet WHERE Id=@id";
                        command.Parameters.AddWithValue("id", TestBet.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }

            //Delete TestUser
            UserController.DeleteUser(TestUser);

            //Delete TestPlayers
            foreach (Player p in TestTeam1.Players) {
                PlayerController.DeletePlayer(p);
            }
            foreach (Player p in TestTeam2.Players) {
                PlayerController.DeletePlayer(p);
            }

            //Delete TestMatch
            MatchController.DeleteMatch(TestMatch);

            //Delete TestEvent
            EventController.DeleteEvent(TestEvent);

            //Delete TestTeam
            TeamController.DeleteTeam(TestTeam1);
            TeamController.DeleteTeam(TestTeam2);

            //Delete TestMap
            MatchController.DeleteMap(TestMap);

        }

        [TestMethod]
        public void PersistCreatedUserAndAccountInDatabasePass() {
            User tempUser = new User {
                Account = TestUserAccount,
                FirstName = "tempTestUser",
                LastName = "tempTestUser",
                UserName = "tempTestUser",
                Email = "tempTestUser",
                PhoneNumber = "12345678"
            };

            //Persist user in database
            UserController.CreateUser(tempUser);

            //Get user with newly persisted id
            User retrievedUser = UserController.GetUser(tempUser.Id);

            //Delete temp user
            UserController.DeleteUser(tempUser);

            //Assert stuff!
            Assert.AreEqual(tempUser.ToString(), retrievedUser.ToString());
        }

        [TestMethod]
        public void PersistCreatedUserAndAccountInDatabaseFail() {
            User tempUser = new User {
                Id = -1,
                Account = TestUserAccount
            };

            try {
                //Persist user in database
                UserController.CreateUser(tempUser);
            } catch (SqlException e) {

            }

            //Assert stuff!
            Assert.IsTrue(tempUser.Id == -1);

        }

        [TestMethod]
        public void GetUserOnIdFromDatabasePass() {

            //Get user
            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.AreEqual(TestUser.ToString(), retrievedUser.ToString());
        }

        [TestMethod]
        public void GetUserOnIdFromDatabaseFail() {
            int lastIdent = -1;
            User retrievedUser = null;

            using (SqlConnection connection = GetSqlConnection()) {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand()) {
                    command.CommandText = "SELECT IDENT_CURRENT('[User]') AS LastIdent";
                    //command.Parameters.AddWithValue("table", "'[User]'");
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    lastIdent = int.Parse(reader["LastIdent"].ToString());
                    reader.Close();
                }
                connection.Close();
            }
            try {
                //Get user
                retrievedUser = UserController.GetUser((lastIdent + 10));
                } catch (IndexOutOfRangeException e) {

                }

            //Assert stuff!
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public void GetAllUsersFromDatabasePass() {
            //Setup method
            bool everyIndexHasUser = true;

            //Get all users
            List<User> allUsers = UserController.GetAllUsers();

            //Check type for each user
            foreach(var u in allUsers) {
                if(!(u is User)) {
                    everyIndexHasUser = false;
                }
            }

            //Assert stuff!
            Assert.IsTrue(everyIndexHasUser);
        }

        //[TestMethod]
        //public void GetAllUsersFromDatabaseFail() {
        //    //Setup method
        //    bool everyIndexHasUser = true;

        //    TestCleanUp();

        //    //Get all users
        //    List<User> allUsers = UserController.GetAllUsers();

        //    //Check users
        //    if (allUsers.Count == 0) {
        //        everyIndexHasUser = false;
        //    } else {
        //        foreach (User u in allUsers) {
        //            if (!(u is User)) {
        //                everyIndexHasUser = false;
        //            }
        //        }
        //    }

            //Assert stuff!
        //  Assert.IsFalse(everyIndexHasUser);
        //}

        [TestMethod]
        public void UpdateAlreadyPersistedUserInDatabasePass() {
            //Change firstname on usercopy and update in databate
            TestUser.FirstName = "Bent";
            UserController.UpdateUser(TestUser);

            //Get user from database
            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.AreEqual(TestUser.FirstName, retrievedUser.FirstName);
        }

        [TestMethod]
        public void UpdateAlreadyPersistedUserInDatabaseFail() {
            //Change firstname on usercopy and update in databate
            TestUser.FirstName = null;
            try {
                UserController.UpdateUser(TestUser);
            } catch (SqlException e) {

            }

            //Get user from database
            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.AreNotEqual(TestUser.FirstName, retrievedUser.FirstName);
        }

        [TestMethod]
        public void AddBetToUserPass() {

            //Add bet to user
            UserController.AddBetToUser(TestUser, TestBet);

            //Get user
            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.AreEqual(TestBet, retrievedUser.Bets[0]);
        }

        [TestMethod]
        public void AddBetToUserFail() {
            Bet tempBet = new Bet {
                Amount = 1.00M,
                Odds = 2.34M,
                Type = TestMatch,
                WinCondition = null
            };

            try {
                //Add bet to user
                UserController.AddBetToUser(TestUser, tempBet);
            } catch (NullReferenceException e) {
                
            }

            //Get user
            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.IsTrue(retrievedUser.Bets.Count == 0);
        }

        [TestMethod]
        public void DeleteUserAndAccountFromDatabasePass() {
            //Delete persisted user
            UserController.DeleteUser(TestUser);

            //Assert stuff!
            Assert.IsNull(UserController.GetUser(TestUser.Id));
        }

        [TestMethod]
        public void DeleteUserAndAccountFromDatabaseFail() {
            TestUser.Account = null;

            try {
                //Delete persisted user
                UserController.DeleteUser(TestUser);
            } catch (NullReferenceException e) {
                TestUser.Account = TestUserAccount;
            }

            //Assert stuff!
            Assert.IsNotNull(UserController.GetUser(TestUser.Id));
        }

        [TestMethod]
        public void UpdateAccountInDatabasePass() {
            //Copy user
            decimal oldBalance = TestUser.Account.Balance;

            //Update account
            TestUser.Account.Balance = 10000.00M;
            UserController.UpdateAccount(TestUser);

            User retrievedUser = UserController.GetUser(TestUser.Id);
            //Assert stuff!
            Assert.AreNotEqual(oldBalance, retrievedUser.Account.Balance);
        }

        [TestMethod]
        public void UpdateAccountInDatabaseFail() {
            //Copy user
            decimal oldBalance = TestUser.Account.Balance;

            try {
                //Update account
                TestUser.Account.Balance = (1000.34M*9000000.97M)*35847584M;
                UserController.UpdateAccount(TestUser);
            } catch (SqlException e) {

            }

            User retrievedUser = UserController.GetUser(TestUser.Id);

            //Assert stuff!
            Assert.AreEqual(oldBalance, retrievedUser.Account.Balance);
        }

        public static string ConnectionString {
            get {
                string connStr = ConfigurationManager.ConnectionStrings["JaaamDbString"].ToString();

                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connStr);
                sb.ApplicationName = ApplicationName ?? sb.ApplicationName;
                sb.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : sb.ConnectTimeout;
                return sb.ToString();
            }
        }

        public static SqlConnection GetSqlConnection() {
            SqlConnection conn = new SqlConnection(ConnectionString);
            return conn;
        }

        public static int ConnectionTimeout { get; set; }

        public static string ApplicationName {
            get;
            set;
        }

    }

}