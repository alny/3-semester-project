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
    /// Interaction logic for TeamPage.xaml
    /// </summary>
    public partial class TeamPage : Page {
        ServiceClient client = null;
        IEnumerable<Team> listOfTeams = new List<Team>();

        public TeamPage() {
            InitializeComponent();
            loadTeamList();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadTeamList() {
            client = new ServiceClient();
            listOfTeams = client.GetTeams();

            foreach (Team item in listOfTeams) {
                teamViewList.Items.Add(item);
            }
        }

        public void EditSingleTeam() {
            Team selectedTeam = (Team)teamViewList.SelectedItem;
            selectedTeam.Name = teamNameInput.Text;

            if (playerListBox.SelectedItem != null && playersComboBox.SelectedItem != null) {
                selectedTeam.Players.Remove((Player)playerListBox.SelectedItem);
                selectedTeam.Players.Add((Player)playersComboBox.SelectedItem);

                playerListBox.Items.Clear();
                foreach (Player p in selectedTeam.Players) {
                    playerListBox.Items.Add(p);
                }
                playerListBox.Items.Refresh();
                client.EditTeam(selectedTeam);
                MessageBox.Show("Team successfully updated!");
            }

        }

        public void DeleteSingleTeam() {
            Team selectedTeam = (Team)teamViewList.SelectedItem;
            client.DeleteTeam(selectedTeam);
            MessageBox.Show("Team successfully Deleted!");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            playerListBox.Items.Clear();
            playersComboBox.Items.Clear();

            Team selectedTeam = (Team)teamViewList.SelectedItem;
            List<Player> allPlayers = client.GetPlayers();

            foreach (Player p in selectedTeam.Players) {
                playerListBox.Items.Add(p);
            }

            foreach (Player p in allPlayers) {
                playersComboBox.Items.Add(p);
            }

            teamNameInput.Text = selectedTeam.Name;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Team newTeamObject = new Team();
            newTeamObject.Name = teamNameInput.Text;
            client.CreateTeam(newTeamObject);
            teamViewList.Items.Add(newTeamObject);
            teamViewList.Items.Refresh();
            MessageBox.Show("Team successfully created!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSingleTeam();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            DeleteSingleTeam();
        }

        private void playersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
           
        }

        private void listBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Player selectedPlayer = (Player)playerListBox.SelectedItem;

            if (selectedPlayer != null) {
                for (int i = 0; i < playersComboBox.Items.Count; i++) {
                    Player player = (Player)playersComboBox.Items[i];
                    if (player.Id == selectedPlayer.Id) {
                        playersComboBox.SelectedItem = player;
                    }
                }
            }
            
        }
    }
}
