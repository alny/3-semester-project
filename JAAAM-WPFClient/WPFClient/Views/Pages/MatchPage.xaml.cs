using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPFClient.Views.Pages {
    /// <summary>
    /// Interaction logic for MatchPage.xaml
    /// </summary>
    public partial class MatchPage : Page {
        ServiceClient client = null;
        IEnumerable<Match> listOfMatches = new List<Match>();

        public MatchPage() {
            InitializeComponent();
            loadMatchList();
            matchNameInput.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadMatchList() {
            client = new ServiceClient();
            listOfMatches = client.GetMatches();

            foreach (Match item in listOfMatches) {
                matchViewList.Items.Add(item);
            }
        }

        public void EditSingleMatch() {
            Match selectedMatch = (Match)matchViewList.SelectedItem;
            selectedMatch.Name = matchNameInput.Text;

            if (teamListBox.SelectedItem != null && teamComboBox.SelectedItem != null) {
                selectedMatch.Teams.Remove((Team)teamListBox.SelectedItem);
                selectedMatch.Teams.Add((Team)teamComboBox.SelectedItem);

                teamListBox.Items.Clear();
                foreach (Team t in selectedMatch.Teams) {
                    teamListBox.Items.Add(t);
                }
                
                client.EditMatch(selectedMatch);
                selectedMatch.GenerateName(selectedMatch.Teams[0], selectedMatch.Teams[1]);
                teamListBox.Items.Refresh();
                matchViewList.Items.Refresh();

                MessageBox.Show("Match successfully updated!");
            }

        }

        public void DeleteSingleMatch() {
            Match selectedMatch = (Match)matchViewList.SelectedItem;
            client.DeleteMatch(selectedMatch);
            MessageBox.Show("Match successfully Deleted!");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            teamListBox.Items.Clear();
            teamComboBox.Items.Clear();

            Match selectedMatch = (Match)matchViewList.SelectedItem;

            foreach (Team t in selectedMatch.Teams) {
                teamListBox.Items.Add(t);
            }

            List<Team> allTeams = client.GetTeams();

            foreach (Team t in allTeams) {
                teamComboBox.Items.Add(t);
            }

            matchNameInput.Text = selectedMatch.Name;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Match newMatchObject = new Match();
            newMatchObject.Name = matchNameInput.Text;
            client.CreateMatch(newMatchObject);
            matchViewList.Items.Add(newMatchObject);
            matchViewList.Items.Refresh();
            MessageBox.Show("Match successfully created!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSingleMatch();
            matchViewList.Items.Refresh();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            DeleteSingleMatch();
        }

        private void teamListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Team selectedTeam = (Team)teamListBox.SelectedItem;

            if (selectedTeam != null) {
                for (int i = 0; i < teamComboBox.Items.Count; i++) {
                    Team team = (Team)teamComboBox.Items[i];
                    if (team.Id == selectedTeam.Id) {
                        teamComboBox.SelectedItem = team;
                    }
                }
            }
        }
    }
}
