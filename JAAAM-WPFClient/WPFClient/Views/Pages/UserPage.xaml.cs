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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page {
        ServiceClient client = null;
        IEnumerable<User> listOfUsers = new List<User>();

        public UserPage() {
            InitializeComponent();
            loadUserList();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        public void loadUserList() {
            client = new ServiceClient();
            listOfUsers = client.GetAllUsers();
            foreach (User item in listOfUsers) {
                userViewList.Items.Add(item);
            }
        }

        public void EditSingleEvent() {
            User selectedUser = (User)userViewList.SelectedItem;
            selectedUser.FirstName = userFirstNameInput.Text;
            selectedUser.LastName = userLastNameInput.Text;
            selectedUser.UserName = userUserNameInput.Text;
            selectedUser.Email = userEmailInput.Text;
            selectedUser.PhoneNumber = userPhoneInput.Text;

            client.UpdateUser(selectedUser);
            MessageBox.Show("User successfully updated!");
        }

        public void DeleteSingleEvent() {
            //User selectedEvent = (User)eventViewList.SelectedItem;
            //client.DeleteEvent(selectedEvent);
            MessageBox.Show("User successfully Deleted!");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            User selectedUser = (User)userViewList.SelectedItem;
            userFirstNameInput.Text = selectedUser.FirstName;
            userLastNameInput.Text = selectedUser.LastName;
            userUserNameInput.Text = selectedUser.UserName;
            userEmailInput.Text = selectedUser.Email;
            userPhoneInput.Text = selectedUser.PhoneNumber;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            client = new ServiceClient();
            Account account = new Account();
            account.Balance = 0.00M;
            User newUserObject = new User();
            newUserObject.FirstName = userFirstNameInput.Text;
            newUserObject.LastName = userLastNameInput.Text;
            newUserObject.UserName = userUserNameInput.Text;
            newUserObject.Email = userEmailInput.Text;
            newUserObject.PhoneNumber = userPhoneInput.Text;
            newUserObject.Account = account;
            client.CreateUser(newUserObject);
            userViewList.Items.Add(newUserObject);
            userViewList.Items.Refresh();
            MessageBox.Show("User successfully created!");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            EditSingleEvent();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            DeleteSingleEvent();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) {
            User selectedUser = (User)userViewList.SelectedItem;
            UserBetDetails userBetDetails = new UserBetDetails(selectedUser);
            userBetDetails.Show();
        }
    }
}
