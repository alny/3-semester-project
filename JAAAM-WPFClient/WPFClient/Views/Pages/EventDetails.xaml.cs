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
using System.Windows.Shapes;
using WPFClient.WCFService;

namespace WPFClient.Views.Pages {
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window {
        public EventDetails(Event eventt) {
            InitializeComponent();
            loadMatchList(eventt);
        }

        public void loadMatchList(Event eventt) {
            foreach (Match item in eventt.Matches) {
                matchListView.Items.Add(item);
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
