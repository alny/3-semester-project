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
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : Page {
        ServiceClient client = null;
        IEnumerable<Player> listOfPlayers = new List<Player>();
        IEnumerable<Team> listOfTeams = new List<Team>();


        public PlayerPage() {
            InitializeComponent();
            loadPlayerList();
            loadTeamsInComboBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadTeamsInComboBox() {
            listOfTeams = client.GetTeams();
            foreach (Team item in listOfTeams) {
                teamComboBox.Items.Add(item);
            }
        }

        public void loadPlayerList() {
            client = new ServiceClient();
            listOfPlayers = client.GetPlayers();
            foreach (Player item in listOfPlayers) {
                playerViewList.Items.Add(item);
            }
        }

        public void EditSinglePlayer() {
            Player selectedPlayer = (Player)playerViewList.SelectedItem;
            selectedPlayer.NickName = nickNameInput.Text;
            selectedPlayer.Age = ageInput.Text;
            selectedPlayer.Role = roleInput.Text;
            Team selectedTeam = (Team)teamComboBox.SelectedItem;
            selectedPlayer.TeamId = selectedTeam.Id;
            client.EditPlayer(selectedPlayer);
            MessageBox.Show("Player successfully updated!");
        }

        public void DeleteSinglePlayer() {
            Player selectedPlayer = (Player)playerViewList.SelectedItem;
            client.DeletePlayer(selectedPlayer);
            MessageBox.Show("Player successfully Deleted!");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Player selectedPlayer = (Player)playerViewList.SelectedItem;
            nickNameInput.Text = selectedPlayer.NickName;
            ageInput.Text = selectedPlayer.Age;
            roleInput.Text = selectedPlayer.Role;
            
            for(int i = 0; i < teamComboBox.Items.Count; i++) {
                Team team = (Team)teamComboBox.Items[i];
                if ( team.Id == selectedPlayer.TeamId) {
                    teamComboBox.SelectedItem = team;
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Player newPlayerObject = new Player();
            newPlayerObject.NickName = nickNameInput.Text;
            newPlayerObject.Age = ageInput.Text;
            newPlayerObject.Role = roleInput.Text;
            newPlayerObject.TeamId = 2;
            client.CreatePlayer(newPlayerObject);
            playerViewList.Items.Add(newPlayerObject);
            playerViewList.Items.Refresh();
            MessageBox.Show("Player successfully created!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSinglePlayer();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            DeleteSinglePlayer();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
