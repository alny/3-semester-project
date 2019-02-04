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
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapPage : Page {
        ServiceClient client = null;
        IEnumerable<Map> listOfMaps = new List<Map>();

        public MapPage() {
            InitializeComponent();
            loadMapList();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadMapList() {
            client = new ServiceClient();
            listOfMaps = client.GetMaps();

            foreach (Map item in listOfMaps) {
                mapViewList.Items.Add(item);
            }
        }

        public void EditSingleMap() {
            Map selectedMap = (Map)mapViewList.SelectedItem;
            selectedMap.Name = mapNameInput.Text;
            selectedMap.IsActive = (bool)mapCheckBox.IsChecked;
            client.EditMap(selectedMap);
            MessageBox.Show("Map successfully updated!");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Map selectedMap = (Map)mapViewList.SelectedItem;
            mapNameInput.Text = selectedMap.Name;
            mapCheckBox.IsChecked = (bool?)selectedMap.IsActive;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Map newMapObject = new Map();
            newMapObject.Name = mapNameInput.Text;
            client.CreateMap(newMapObject);
            mapViewList.Items.Add(newMapObject);
            mapViewList.Items.Refresh();
            MessageBox.Show("Map successfully created!");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSingleMap();
        }
    }
}
