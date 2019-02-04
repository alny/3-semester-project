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
using WPFClient.Views;

namespace WPFClient {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            LoadDashboard();
            LoadManageContentTab();
        }

        public void LoadDashboard() {
            Frame dashboardContentFrame = new Frame();
            DashboardContent dashboardContent = new DashboardContent();
            dashboardContentFrame.Content = dashboardContent;
            Dashboard.Content = dashboardContentFrame;
        }

        public void LoadManageContentTab() {
            Frame manageContentFrame = new Frame();
            ManageContent manageContent = new ManageContent();
            manageContentFrame.Content = manageContent;
            ManageContent.Content = manageContentFrame;
        }

    }
}
