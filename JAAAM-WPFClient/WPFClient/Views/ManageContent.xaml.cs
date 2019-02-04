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
using WPFClient.Views.Pages;

namespace WPFClient.Views {
    /// <summary>
    /// Interaction logic for ManageContent.xaml
    /// </summary>
    public partial class ManageContent : Page {
        public ManageContent() {
            InitializeComponent();
            LoadEventPage();
            LoadUserPage();
            LoadMapPage();
            LoadMatchPage();
            LoadTeamPage();
            LoadPlayerPage();
        }

        public void LoadEventPage() {
            Frame eventContentFrame = new Frame();
            EventPage eventContent = new EventPage();
            eventContentFrame.Content = eventContent;
            EventTab.Content = eventContentFrame;
        }
        public void LoadMatchPage() {
            Frame matchContentFrame = new Frame();
            MatchPage matchContent = new MatchPage();
            matchContentFrame.Content = matchContent;
            MatchTab.Content = matchContentFrame;
        }

        public void LoadUserPage() {
            Frame userContentFrame = new Frame();
            UserPage userContent = new UserPage();
            userContentFrame.Content = userContent;
            UserTab.Content = userContentFrame;
        }

        public void LoadMapPage() {
            Frame mapContentFrame = new Frame();
            MapPage mapContent = new MapPage();
            mapContentFrame.Content = mapContent;
            MapTab.Content = mapContentFrame;
        }

        public void LoadTeamPage() {
            Frame teamContentFrame = new Frame();
            TeamPage teamContent = new TeamPage();
            teamContentFrame.Content = teamContent;
            TeamTab.Content = teamContentFrame;
        }

        public void LoadPlayerPage() {
            Frame playerContentFrame = new Frame();
            PlayerPage playerContent = new PlayerPage();
            playerContentFrame.Content = playerContent;
            PlayerTab.Content = playerContentFrame;
        }

    }
}
