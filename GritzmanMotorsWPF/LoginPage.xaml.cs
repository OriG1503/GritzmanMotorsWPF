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
using ApiServiceNM;
using Model;
using ViewModel;

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public static Person loggedInPerson { get; set; }

        public LoginPage()
        {
            InitializeComponent();
        }

        private void RedirectToRegisterPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        public async Task<Person> SpecificPerson(string username, string password)
        {
            ApiService apiService = new ApiService();
            PersonList personList = await apiService.GetPersonList();
            return personList.Find(x => x.FirstName == username && x.LastName == password);
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                string username = txtUsername.Text;
                string password = txtPassword.Password;

                Person person = await SpecificPerson(username, password);

                if (person != null)
                {
                    loggedInPerson = person;
                    // Successful login, navigate to HomePage.xaml with the person parameter
                    LoginPage.loggedInPerson = loggedInPerson;
                    NavigationService.Navigate(new HomePage());
                }
                else
                {
                    // Unsuccessful login, show an error message or handle accordingly
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearFields();

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or display an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearFields()
        {
            // Clear the textboxes and other fields
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }
    }
}
