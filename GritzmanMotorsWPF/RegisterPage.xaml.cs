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
using System.Xml.Linq;

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        // Regular expressions for first and last names
        private readonly Regex nameRegex = new Regex(@"^[A-Z][a-z]{2,9}$");
        private readonly Regex phoneNumberRegex = new Regex(@"^0[0-9]{9}$");

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RedirectToLoginPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                DateOnly dateOfBirth = DateOnly.FromDateTime((DateTime)dpDateOfBirth.SelectedDate);
                string phoneNumber = txtPhoneNumber.Text;

                // Gather user input (you should validate and sanitize the input)
                Customer customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    PhoneNumber = phoneNumber
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

                if (!phoneNumberRegex.IsMatch(phoneNumber))
                {
                    MessageBox.Show("Invalid phone number.It should be longer than 10 digits and start with the digit 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int registrationResult = await apiService.InsertCustomer(customer);
                // Display a message based on the registration result
                if (registrationResult == 1)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                    NavigationService.Navigate(new LoginPage());
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
