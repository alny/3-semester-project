using System;
using System.Collections.Generic;
using BusinessLogic.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;


namespace UnitTest {
    [TestClass]
    public class PlayerTest {

        private TeamController _teamController;
        private PlayerController _playerController;
        private Player _testPlayer1;
        private Player _testPlayer2;
        private Player _testPlayer3;
        private Player _testPlayer4;
        private Player _testPlayer5;
        private Team _testTeam;
        private int _teamId;
        private IEnumerable<Player> _testPlayerList;

        [TestInitialize]
        public void TestInitialize() {
            _teamController = new TeamController();
            _playerController = new PlayerController();

            _testTeam = new Team {
                Name = "TestTeam"
            };

            _teamId = _teamController.CreateTeam(_testTeam);
            _testTeam.Id = _teamId;

            _testPlayer1 = new Player {
                NickName = "TestNickName",
                Age = "TestAge",
                Role = "TestRole",
                TeamId = _teamId
            };
            _testPlayer2 = new Player {
                NickName = "TestNickName",
                Age = "TestAge",
                Role = "TestRole",
                TeamId = _teamId
            };
            _testPlayer3 = new Player {
                NickName = "TestNickName",
                Age = "TestAge",
                Role = "TestRole",
                TeamId = _teamId
            };
            _testPlayer4 = new Player {
                NickName = "TestNickName",
                Age = "TestAge",
                Role = "TestRole",
                TeamId = _teamId
            };
            _testPlayer5 = new Player {
                NickName = "TestNickName",
                Age = "TestAge",
                Role = "TestRole",
                TeamId = _teamId
            };
        }


        [TestMethod]
        public void TestCreatePlayerPersisted() {
            //arrange
            int rowid = 0;
            //act
            rowid = _playerController.CreatePlayer(_testPlayer1);
            _testPlayer1.Id = rowid;

            //assert
            Assert.IsTrue(rowid > 0);
        }

        [TestMethod]
        public void TestGetPlayerById() {
            //Arrange
            int id = _playerController.CreatePlayer(_testPlayer1);
            Player playerFound = null;
            _testPlayer1.Id = id;

            //Act
            playerFound = _playerController.GetPlayer(id);

            //Assert
            Assert.AreEqual(_testPlayer1.ToString(), playerFound.ToString());
        }

        [TestMethod]
        public void TestEditPersistedPlayer() {
            //Arrange
            int id = _playerController.CreatePlayer(_testPlayer1);
            Player testPlayerUpdated = new Player {
                Id = id,
                NickName = "TestNickNameUpdated",
                Age = "TestAgeUpdated",
                Role = "TestRoleUpdated",
                TeamId = _teamId
            };
            _testPlayer1.Id = id;

            //Act
            _playerController.EditPlayer(testPlayerUpdated);
            Player result = _playerController.GetPlayer(id);

            //Assert
            Assert.AreEqual(result.ToString(), testPlayerUpdated.ToString());
        }

        [TestMethod]
        public void TestDeletePersistedPlayer() {
            //Arrange
            int id = _playerController.CreatePlayer(_testPlayer1);
            _testPlayer1.Id = id;
            //Act
            _playerController.DeletePlayer(_testPlayer1);
            Player result = _playerController.GetPlayer(id);
            
            //Assert
            Assert.IsNull(result);
        }
        

        [TestMethod]
        public void GetAllPlayersFromDatabase() {
            //Setup method
            bool everyIndexHasPlayer = true;

            //Get all users
            _testPlayerList = _playerController.GetPlayers();

            //Check type for each user
            foreach (Player p in _testPlayerList) {
                if (!(p is Player)) {
                    everyIndexHasPlayer = false;
                }
            }

            //Assert stuff!
            Assert.IsTrue(everyIndexHasPlayer);
        }

        [TestCleanup]
        public void TestCleanUp() {
            _playerController.DeletePlayer(_testPlayer1);
            _playerController.DeletePlayer(_testPlayer2);
            _playerController.DeletePlayer(_testPlayer3);
            _playerController.DeletePlayer(_testPlayer4);
            _playerController.DeletePlayer(_testPlayer5);
            _teamController.DeleteTeam(_testTeam);

        }
    }
}
