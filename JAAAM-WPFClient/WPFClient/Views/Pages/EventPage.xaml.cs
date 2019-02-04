using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFClient.WCFService;

namespace WPFClient.Views.Pages {
    /// <summary>
    /// Interaction logic for EventPage.xaml
    /// </summary>
    public partial class EventPage : Page {

        ServiceClient client = null;
        IEnumerable<Event> listOfEvents = new List<Event>();

        public EventPage() {
            InitializeComponent();
            loadEventList();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadEventList() {
            client = new ServiceClient();
            listOfEvents = client.GetEvents();
            List<Event> eventList = new List<Event>();

            foreach (Event item in listOfEvents) {
                eventViewList.Items.Add(item);
            }
        }

        public void EditSingleEvent() {
            Event selectedEvent = (Event)eventViewList.SelectedItem;
            selectedEvent.Name = eventNameInput.Text;
            selectedEvent.GameName = eventGameNameInput.Text;
            selectedEvent.Type = eventTypeInput.Text;
            selectedEvent.Held = (bool)eventHeld.IsChecked;
            client.EditEvent(selectedEvent);
            MessageBox.Show("Event successfully updated!");
        }

        public void DeleteSingleEvent() {
            Event selectedEvent = (Event)eventViewList.SelectedItem;
            if (selectedEvent != null) {
                client.DeleteEvent(selectedEvent);
                MessageBox.Show("Event successfully Deleted!");
            } else {
                MessageBox.Show("Select an Event to delete");
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Event selectedEvent = (Event)eventViewList.SelectedItem;
            eventNameInput.Text = selectedEvent.Name;
            eventGameNameInput.Text = selectedEvent.GameName;
            eventTypeInput.Text = selectedEvent.Type;
            eventHeld.IsChecked = selectedEvent.Held;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Event newEventObject = new Event();
            newEventObject.Name = eventNameInput.Text;
            newEventObject.GameName = eventGameNameInput.Text;
            newEventObject.Type = eventTypeInput.Text;
            newEventObject.Held = (bool)eventHeld.IsChecked;
            client.CreateEvent(newEventObject);
            eventViewList.Items.Add(newEventObject);
            eventViewList.Items.Refresh();
            MessageBox.Show("Event successfully created!");
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSingleEvent();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            DeleteSingleEvent();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e) {
            Event selectedEvent = (Event)eventViewList.SelectedItem;

            EventDetails eventDetails = new EventDetails(selectedEvent);
            eventDetails.Show();
        }
    }
}
