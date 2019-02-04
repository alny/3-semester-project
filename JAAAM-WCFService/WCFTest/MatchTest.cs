using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using WCFService;
using BusinessLogic.Controllers;
using System.Configuration;
using System.Data.SqlClient;
using DataAccess;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class MatchUnitTest
    {

        private EventController _EventController;
        private MatchController _MatchController;
        private TeamController _TeamController;
        private PlayerController _PlayerController;
        private Match _TestMatch;
        private Event _TestEvent;
        private Team _TestTeam1;
        private Team _TestTeam2;
        private Map _TestMap;
        private int _EventId;
        private int _TestTeam1Id;
        public int _TestTeam2Id;
        private Player _TestTeam1Player1;
        private Player _TestTeam1Player2;
        private Player _TestTeam1Player3;
        private Player _TestTeam1Player4;
        private Player _TestTeam1Player5;
        private Player _TestTeam2Player1;
        private Player _TestTeam2Player2;
        private Player _TestTeam2Player3;
        private Player _TestTeam2Player4;
        private Player _TestTeam2Player5;


        [TestInitialize]
        public void TestInitialize()
        {
            _MatchController = new MatchController();
            _EventController = new EventController();
            _TeamController = new TeamController();
            _PlayerController = new PlayerController();
            _TestEvent = new Event {
                Name = "testname",
                GameName = "testgame",
                Type = "testtype"
            };
            _EventId = _EventController.CreateEvent(_TestEvent);

            _TestTeam1 = new Team {
                Name = "TestTeam"
            };

            _TestTeam2 = new Team {
                Name = "TestTeam2"
            };
            _TestTeam1Id = _TeamController.CreateTeam(_TestTeam1);
            _TestTeam2Id = _TeamController.CreateTeam(_TestTeam2);
            _TestTeam1Player1 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam1Id
            };
            _PlayerController.CreatePlayer(_TestTeam1Player1);
            _TestTeam1Player2 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam1Id
            };
            _PlayerController.CreatePlayer(_TestTeam1Player2);
            _TestTeam1Player3 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam1Id
            };
            _PlayerController.CreatePlayer(_TestTeam1Player3);
            _TestTeam1Player4 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam1Id
            };
            _PlayerController.CreatePlayer(_TestTeam1Player4);
            _TestTeam1Player5 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam1Id
            };
            _PlayerController.CreatePlayer(_TestTeam1Player5);
            _TestTeam1.Players.Add(_TestTeam1Player1);
            _TestTeam1.Players.Add(_TestTeam1Player2);
            _TestTeam1.Players.Add(_TestTeam1Player3);
            _TestTeam1.Players.Add(_TestTeam1Player4);
            _TestTeam1.Players.Add(_TestTeam1Player5);

            _TestTeam2Player1 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam2Id
            };
            _PlayerController.CreatePlayer(_TestTeam2Player1);
            _TestTeam2Player2 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam2Id
            };
            _PlayerController.CreatePlayer(_TestTeam2Player2);
            _TestTeam2Player3 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam2Id
            };
            _PlayerController.CreatePlayer(_TestTeam2Player3);
            _TestTeam2Player4 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam2Id
            };
            _PlayerController.CreatePlayer(_TestTeam2Player4);
            _TestTeam2Player5 = new Player {
                NickName = "TestPlayer",
                Age = "123",
                Role = "testRole",
                TeamId = _TestTeam2Id
            };
            _PlayerController.CreatePlayer(_TestTeam2Player5);
            _TestTeam2.Players.Add(_TestTeam2Player1);
            _TestTeam2.Players.Add(_TestTeam2Player2);
            _TestTeam2.Players.Add(_TestTeam2Player3);
            _TestTeam2.Players.Add(_TestTeam2Player4);
            _TestTeam2.Players.Add(_TestTeam2Player5);
            _TestMatch = new Match {
                Format = "TestFormat",
                Winner = null,
                EventId = _EventId
            };
            _TestMatch.Teams.Add(_TestTeam1);
            _TestMatch.Teams.Add(_TestTeam2);
            _TestMatch.GenerateName(_TestTeam1, _TestTeam2);

            _TestMap = new Map {
                Name = "TestMap",
                IsActive = true
            };
            _TestMap.Id = _MatchController.CreateMap(_TestMap);
            _TestMatch.Maps.Add(_TestMap);

        }

        [TestMethod]
        public void TestCreateMatchPersisted() {
            //Arrange
            int rowID = 0;
            //Act
            rowID = _MatchController.CreateMatch(_TestMatch);

            //Assert
            Assert.IsTrue(rowID > 0);
        }

        [TestMethod]
        public void TestGetMatchByIdPass() {
            //Arrange
            int id = _MatchController.CreateMatch(_TestMatch);
            Match matchFound = null;

            //Act
            matchFound = _MatchController.GetMatch(id);

            //Assert
            Assert.AreEqual(_TestMatch.Format, matchFound.Format);
        }
        [TestMethod]
        public void TestGetMatchByIdFail(){
            //Arrange
            int lastIdent = 0;
            Match recievedMatch = null;
            string connStr = ConfigurationManager.ConnectionStrings["JaaamDbString"].ToString();
            using (SqlConnection connection = new SqlConnection(connStr)) {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand()) {
                    command.CommandText = "SELECT IDENT_CURRENT('[User]') AS LastIdent";
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    lastIdent = int.Parse(reader["LastIdent"].ToString());
                    reader.Close();
                }
                connection.Close();
            }
            lastIdent += 10;
            //Act
            try {
                recievedMatch = _MatchController.GetMatch(lastIdent);
            } catch(IndexOutOfRangeException e) {
            }
            //Assert
            Assert.IsNull(recievedMatch);
        }
        [TestMethod]
        public void TestCreateMatchWithInvalidEventIdAttached()  {
            //Arrange
            _TestMatch.EventId = -19;
            int matchId = 0;
            //Act
            try {
                matchId = _MatchController.CreateMatch(_TestMatch);
            } catch(SqlException e) {

            }
            //Assert
            Assert.IsTrue(matchId == 0);
        }
        [TestMethod]
        public void TestUpdatePersistedMatch(){
            //Arrange
            int matchId = _MatchController.CreateMatch(_TestMatch);
            Match testMatchUpdate = new Match
            {
                Id = matchId,
                Held = true,
                Format = "TestFormatUpdated",
                Winner = _TestTeam1,
                EventId = _EventId
            };
            testMatchUpdate.Maps.Add(_TestMap);
            testMatchUpdate.Teams.Add(_TestTeam1);
            testMatchUpdate.Teams.Add(_TestTeam2);

            //Act
            _MatchController.EditMatch(testMatchUpdate);
            var result = _MatchController.GetMatch(matchId);
            //LocalCleanUp
            _MatchController.DeleteMatch(testMatchUpdate);
            //Assert
            Assert.AreEqual(testMatchUpdate.Winner.Id, result.Winner.Id);
        }

        [TestMethod]
        public void TestDeletePersistedMatch(){
            //Arrange
            int id = _MatchController.CreateMatch(_TestMatch);
            _TestMatch.Id = id;
            //Act
            _MatchController.DeleteMatch(_TestMatch);
            Match result = _MatchController.GetMatch(id);
            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllMatchesFromDatabase() {
            //Setup method
            bool everyIndexHasMatch = true;

            //Get all users
            IEnumerable<Match> allMatches = _MatchController.GetMatches();

            //Check type for each user
            foreach (Match m in allMatches)
            {
                if (!(m is Match))
                {
                    everyIndexHasMatch = false;
                }
            }

            //Assert
            Assert.IsTrue(everyIndexHasMatch);
        }

        [TestCleanup]
        //Deletes all Matches with the format "TestFormat" from db
        public void TestCleanUp(){
            _MatchController.DeleteMatch(_TestMatch);
            _EventController.DeleteEvent(_TestEvent);
            foreach (Player player in _TestTeam1.Players){
                _PlayerController.DeletePlayer(player);
            }
            foreach (Player player in _TestTeam2.Players) {
                _PlayerController.DeletePlayer(player);
            }
            _TeamController.DeleteTeam(_TestTeam1);
            _TeamController.DeleteTeam(_TestTeam2);
            string connStr = ConfigurationManager.ConnectionStrings["JaaamDbString"].ToString();
            using (SqlConnection connection = new SqlConnection(connStr)){
                connection.Open();
                using (SqlCommand command = connection.CreateCommand()) {
                    command.CommandText = "DELETE FROM MAP where Name = @Name; DBCC CHECKIDENT(MAP, RESEED);";
                    command.Parameters.AddWithValue("Name", _TestMap.Name);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
