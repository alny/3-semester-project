using BusinessLogic.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BusinessLogic.Controllers {
    public class EventController : IEventController {
        private DBEvent _DbEvent;
        /// <summary>
        /// Constructor to instantiate _DbEvent
        /// </summary>
        public EventController() {
            _DbEvent = new DBEvent();
        }
        /// <summary>
        /// Persists Event object
        /// </summary>
        /// <param name="eventToCreate"></param>
        /// <returns>EventId given to the persisted event</returns>
        public int CreateEvent(Event eventToCreate) {
            return _DbEvent.CreateEvent(eventToCreate);
        }
        /// <summary>
        /// Deletes persisted event
        /// </summary>
        /// <param name="eventToDelete"></param>
        public void DeleteEvent(Event eventToDelete) {
            _DbEvent.DeleteEvent(eventToDelete);
        }
        /// <summary>
        /// Updates information about persisted event
        /// </summary>
        /// <param name="eventToUpdate"></param>
        public void EditEvent(Event eventToUpdate) {
            _DbEvent.EditEvent(eventToUpdate);
        }
        /// <summary>
        /// Gets event by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found event</returns>
        public Event GetEvent(int id) {
            return _DbEvent.GetEvent(id);
        }
        /// <summary>
        /// Gets All events
        /// </summary>
        /// <returns>List of events</returns>
        public IEnumerable<Event> GetEvents() {
            return _DbEvent.GetEvents();
        }
    }
}
