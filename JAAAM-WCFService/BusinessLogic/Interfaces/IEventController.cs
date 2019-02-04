using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces {
    interface IEventController {
        int CreateEvent(Event eventToCreate);
        Event GetEvent(int id);
        void EditEvent(Event eventToUpdate);
        void DeleteEvent(Event eventToDelete);
        IEnumerable<Event> GetEvents();
    }
}
