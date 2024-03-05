using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using ViewModel;

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        public HomePage()
        {
            InitializeComponent();
            ShowLabels();   
        }

        public void ShowLabels()
        {
            string firstName = LoginPage.loggedInPerson.FirstName;
            firstNameLabel.Content = $"First Name: {firstName}";

            string LastName = LoginPage.loggedInPerson.LastName;
            lastNameLabel.Content = $"Last Name: {LastName}";

            string Birthdate = (LoginPage.loggedInPerson.DateOfBirth).ToString();
            BirthDateLabel.Content = $"Date Of Birth: {Birthdate}";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLoginPage();
        }

        private void NavigateToLoginPage()
        {
            // Navigate to the login page
            NavigationService?.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
        }

    }
}
