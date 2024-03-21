using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for AddManagerPage.xaml
    /// </summary>
    public partial class AddManagerPage : Page
    {
        private readonly Regex nameRegex = new Regex(@"^[A-Z][a-z]{2,9}$");
        private RoleList lst;

        public AddManagerPage()
        {
            InitializeComponent();
            RoleComboBox();
        }

        private async void AddNewManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                DateOnly dateOfBirth = DateOnly.FromDateTime((DateTime)dpDateOfBirth.SelectedDate);

                // Gather user input (you should validate and sanitize the input)
                Manager manager = new Manager
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    RoleCode = lst.Find(x => x.RoleName == roleComboBox.SelectedItem)
                };

                // Call the API to register the person
                ApiService apiService = new ApiService();

                if (!IsValidName(firstName))
                {
                    MessageBox.Show("Invalid first name. First letter should be capital, and it should be between 3 - 10 letters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate last name
                if (!IsValidName(lastName))
                {
                    MessageBox.Show("Invalid last name.It should be longer than 0 letters and no longer than 10 letters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate date of birth
                DateTime currentDate = DateTime.Now;
                DateTime minDateOfBirth = currentDate.AddYears(-17);

                if (dpDateOfBirth.SelectedDate == null || dpDateOfBirth.SelectedDate > minDateOfBirth)
                {
                    MessageBox.Show("Invalid date of birth. Age should be at least 17 years.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int registrationResult = await apiService.InsertManager(manager);

                // Display a message based on the registration result
                if (registrationResult == 1)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                    NavigationService.GetNavigationService(this).Navigate(new ManagersPage());

                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or display an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RoleComboBox()
        {
            ApiService apiService = new ApiService();
            lst = (await (apiService.GetRoleList()));
            Role ceo = lst.Find(x => x.RoleName == "CEO");
            lst.Remove(ceo);
            roleComboBox.ItemsSource = lst.Select(x => x.RoleName);
        }

        private void ClearFields()
        {
            // Clear the textboxes and other fields
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            dpDateOfBirth.SelectedDate = null; // Clear the DatePicker
        }

        private bool IsValidName(string name)
        {
            return nameRegex.IsMatch(name);
        }
    }
}
