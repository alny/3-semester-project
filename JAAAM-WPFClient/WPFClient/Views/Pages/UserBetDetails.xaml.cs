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
    /// Interaction logic for UserBetDetails.xaml
    /// </summary>
    public partial class UserBetDetails : Window {
        public UserBetDetails(User user) {
            InitializeComponent();
            loadBetList(user);
        }

        public void loadBetList(User user) {
            foreach (Bet bet in user.Bets) {
                betListView.Items.Add(bet);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
