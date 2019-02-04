using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using WCFService;
using BusinessLogic.Controllers;
using System.Configuration;
using System.Data.SqlClient;
using DataAccess;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTest {
    [TestClass]
    public class EventTest {
        private EventController _EventController;
        private Event _TestEvent;

        [TestInitialize]
        public void TestInitialize() {
            _EventController = new EventController();
            _TestEvent = new Event {
                Name = "Blast Lisbon",
                GameName = "CSGO",
                Type = "Offline"
            };
        }

        [TestCleanup]
        //Clean up the created Events by deleting them
        public void TestCleanUp() {
            _EventController.DeleteEvent(_TestEvent);
        }

        [TestMethod]
        public void TestCreateEventPersisted() {
            //Arrange
            int rowID = 0;
            //Act
            rowID = _EventController.CreateEvent(_TestEvent);
            _TestEvent.Id = rowID;

            //Assert
            Assert.IsTrue(rowID > 0);
        }

        [TestMethod]
        public void TestGetEventByID() {
            //Arrange
            int id = _EventController.CreateEvent(_TestEvent);

            //Act
            var result = _EventController.GetEvent(id);
            _TestEvent.Id = result.Id;

            //Assert
            Assert.AreEqual(_TestEvent.Name, result.Name);
        }

        [TestMethod]
        public void TestUpdatePersistedEvent() {
            //Arrange
            int id = _EventController.CreateEvent(_TestEvent);
            Event testEventUpdated = new Event {
                Id = id,
                Name = "testname",
                GameName = "testgame",
                Type = "testtype"
            };

            //Act
            _EventController.EditEvent(testEventUpdated);
           _TestEvent = _EventController.GetEvent(id);

            //Assert
            Assert.AreEqual(_TestEvent.ToString(), testEventUpdated.ToString());

        }

        [TestMethod]
        public void TestDeletePersistedEvent() {
            //Arrange
            int id = _EventController.CreateEvent(_TestEvent);

            //Act
            _EventController.DeleteEvent(_TestEvent);
            var result = _EventController.GetEvent(id);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllEventsFromDatabase() {
            //Setup method
            bool everyIndexHasEvent = true;

            //Get all events
            IEnumerable<Event> allEvents = _EventController.GetEvents();

            //Check type for each event
            foreach (Event e in allEvents) {
                if (!(e is Event)) {
                    everyIndexHasEvent = false;
                }
            }

            //Assert stuff!
            Assert.IsTrue(everyIndexHasEvent);
        }


    }
}
