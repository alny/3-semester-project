using BusinessLogic.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UnitTest {

    [TestClass]
    public class TeamTest {

        private TeamController _TeamController;
        private PlayerController _PlayerController;
        private Player _TestPlayer1, _TestPlayer2, _TestPlayer3, _TestPlayer4, _TestPlayer5;
        private Team _TestTeam, _TestTeam2, _TestTeamFail;

        [TestInitialize]
        public void TestInitialize() {
            _TeamController = new TeamController();
            _PlayerController = new PlayerController();

            _TestTeam = new Team {
                Name = "TeamTest"
            };
            _TestTeam2 = _TestTeam;
            _TestTeamFail = new Team();
            _TeamController.CreateTeam(_TestTeam);

            _TestPlayer1 = new Player {
                NickName = "JohnnyBoi",
                Age = "87",
                Role = "Kaffehenter",
                TeamId = _TestTeam.Id
            };
            _TestPlayer2 = new Player {
                NickName = "Zunny",
                Age = "109",
                Role = "AWP",
                TeamId = _TestTeam.Id
            };
            _TestPlayer3 = new Player {
                NickName = "SvendBent",
                Age = "7",
                Role = "Rifle",
                TeamId = _TestTeam.Id
            };
            _TestPlayer4 = new Player {
                NickName = "SkæveKaj",
                Age = "44",
                Role = "IGL/Rifler",
                TeamId = _TestTeam.Id
            };
            _TestPlayer5 = new Player {
                NickName = "D0rteDeagle",
                Age = "42",
                Role = "Rifle",
                TeamId = _TestTeam.Id
            };


            _PlayerController.CreatePlayer(_TestPlayer1);
            _TestTeam.Players.Add(_TestPlayer1);
            _PlayerController.CreatePlayer(_TestPlayer2);
            _TestTeam.Players.Add(_TestPlayer2);
            _PlayerController.CreatePlayer(_TestPlayer3);
            _TestTeam.Players.Add(_TestPlayer3);
            _PlayerController.CreatePlayer(_TestPlayer4);
            _TestTeam.Players.Add(_TestPlayer4);
            _PlayerController.CreatePlayer(_TestPlayer5);
            _TestTeam.Players.Add(_TestPlayer5);
            _TeamController.EditTeam(_TestTeam);
        }

        [TestMethod]
        public void TestEditTeamPass() {
            //Arrange 
            _TestTeam.Name = "Hans' Helte";
            //Act 
            _TeamController.EditTeam(_TestTeam);
            Team tempTeam = _TeamController.GetTeam(_TestTeam.Id);
            //Assert
            Assert.AreEqual(_TestTeam.Name, tempTeam.Name);
        }

        [TestMethod]
        public void TestEditTeamFail() {
            //Arrange 
            _TestTeam.Name = null;
            //Act 
            try
            {
                _TeamController.EditTeam(_TestTeam);
            }
            catch (Exception e)
            {}
            Team tempTeam = _TeamController.GetTeam(_TestTeam.Id);
            //Assert
            Assert.IsNotNull(tempTeam.Name);
        }

        [TestMethod]
        public void TestCreateTeamPersistedPass() {
            //Arrange
            int rowId = 0;

            //Act
            rowId = _TeamController.CreateTeam(_TestTeam2);

            //Assert
            Assert.IsTrue(rowId == _TestTeam.Id);
        }

        [TestMethod]
        public void TestCreateTeamPersistedFail() {
            //Arrange

            int rowId = 0;

            //Act
            try {
                rowId = _TeamController.CreateTeam(_TestTeamFail);
            }
            catch (Exception e) {
            }

            //Assert
            Assert.IsTrue(rowId == 0);
        }

        [TestMethod]
        public void TestGetTeamByIdPass() {
            //Arrange
            int id = _TestTeam.Id;
            //Act
            Team teamFound = _TeamController.GetTeam(id);

            //Assert
            Assert.AreEqual(_TestTeam.ToString(), teamFound.ToString());
        }

        [TestMethod]
        public void TestGetTeamByIdFail() {
            //Arrange
            int id = -1;
            //Act
            Team teamFound = _TeamController.GetTeam(id);

            //Assert
            Assert.IsNull(teamFound);
        }


        [TestMethod]
        public void TestDeletePersistedTeamPass() {
            //Arrange
            int id = _TeamController.CreateTeam(_TestTeam);
            _TestTeam.Id = id;
            //Act
            foreach(Player p in _TestTeam.Players) {
                _PlayerController.DeletePlayer(p);
            }
            _TeamController.DeleteTeam(_TestTeam);
            _TestTeam.Id = id;
            Team result = _TeamController.GetTeam(id);

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void TestDeletePersistedTeamFail() {
            //Arrange
            int id = _TestTeam.Id;
            _TestTeam.Id = (901);
            //Act
            try {
                _TeamController.DeleteTeam(_TestTeam);
                _TestTeam.Id = id; 
            } catch (SqlException e) {

            }
            Team result = _TeamController.GetTeam(id);
            //Assert
            Assert.IsNotNull(result);
        }



        [TestMethod]
        public void GetAllTeamsFromDatabase() {
            //Setup method
            bool everyIndexHasTeam = true;

            //Get all users
            IEnumerable<Team> allTeams = _TeamController.GetTeams();

            //Check type for each user
            foreach (Team t in allTeams) {
                if (!(t is Team)) {
                    everyIndexHasTeam = false;
                }
            }

            //Assert stuff!
            Assert.IsTrue(everyIndexHasTeam);
        }

        [TestCleanup]
        //Deletes all Teams with the TeamName "TestTeam" from db
        public void TestCleanUp() {
            //Delete all test-created players
            _PlayerController.DeletePlayer(_TestPlayer1);
            _PlayerController.DeletePlayer(_TestPlayer2);
            _PlayerController.DeletePlayer(_TestPlayer3);
            _PlayerController.DeletePlayer(_TestPlayer4);
            _PlayerController.DeletePlayer(_TestPlayer5);
            //Delete all test-created teams
            _TeamController.DeleteTeam(_TestTeam);
            _TeamController.DeleteTeam(_TestTeam2);
            _TeamController.DeleteTeam(_TestTeamFail);
            
        }

    }
}
