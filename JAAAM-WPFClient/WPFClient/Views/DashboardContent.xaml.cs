using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.WCFService;

namespace WPFClient.Views {
    /// <summary>
    /// Interaction logic for DashboardContent.xaml
    /// </summary>
    public partial class DashboardContent : Page {
        ServiceClient client = null;
        IEnumerable<Match> listOfMatches = new List<Match>();
        private static IHubProxy MatchHubProxy { get; set; }
        Match selectedMatch;

        const string ServerURI = "http://localhost:8080";
        private static HubConnection Connection { get; set; }
        public DashboardContent() {
            InitializeComponent();
            Odds1.IsEnabled = false;
            Odds2.IsEnabled = false;
            leaveMatch.IsEnabled = false;
            joinMatch.IsEnabled = false;
            loadMatchList();
            HubConnection();
        }

        public void loadMatchList() {
            client = new ServiceClient();
            listOfMatches = client.GetMatches();

            foreach (Match item in listOfMatches) {
                matchViewList.Items.Add(item);
            }
        }

        private async void HubConnection() {
            Connection = new HubConnection(ServerURI);
            MatchHubProxy = Connection.CreateHubProxy("MatchHub");

            MatchHubProxy.On<Round>("addRound", (round) =>
              this.Dispatcher.Invoke(() => {
                  roundList.Items.Add("Round " + round.Number + " started!");
                  roundList.Items.Add("");

              })
           );
            MatchHubProxy.On<Kill>("addKill", (kill) =>
            this.Dispatcher.Invoke(() => {
                roundList.Items.Add(kill.KillerName + " killed " + kill.KilledName);
                roundList.Items.Add("");
                UpdateScrollBar(roundList);
            })
            );

            MatchHubProxy.On<Odds>("addOdds", (odds) =>
            this.Dispatcher.Invoke(() => {
                decimal d1 = odds.Odds1;
                decimal d2 = odds.Odds2;
                d1 = decimal.Round(d1, 2);
                d2 = decimal.Round(d2, 2);

                Odds1.Text = d1.ToString();
                Odds2.Text = d2.ToString();
            })
            );
            try {
                await Connection.Start();
                Console.WriteLine("Connected");
            } catch (HttpRequestException) {
                Console.WriteLine("Unable to connect to server: Start server before connecting clients.");
                return;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e) {
        //    cStatus.Content = "Connected";

        //}

        private void UpdateScrollBar(ListBox listBox) {
            if (listBox != null) {
                var border = (Border)VisualTreeHelper.GetChild(listBox, 0);
                var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            joinMatch.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            leaveMatch.IsEnabled = true;
            joinMatch.IsEnabled = false;
           selectedMatch = (Match)matchViewList.SelectedItem;
           MatchHubProxy.Invoke("JoinMatch", selectedMatch.Name);
           cStatus.Content = "Connected";
        }

        private void leaveMatch_Click(object sender, RoutedEventArgs e) {
            leaveMatch.IsEnabled = false;
            joinMatch.IsEnabled = true;
            selectedMatch = (Match)matchViewList.SelectedItem;
            MatchHubProxy.Invoke("LeaveMatch", selectedMatch.Name);
            cStatus.Content = "Not Connected";
        }
    }
}
