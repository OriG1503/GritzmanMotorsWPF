using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for AddNewCarCompanyPage.xaml
    /// </summary>
    public partial class AddNewCarCompanyPage : Page
    {
        public AddNewCarCompanyPage()
        {
            InitializeComponent();
        }

        private async void AddNewCarCompnay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CarCompany carCompany = new CarCompany
                {
                    CarCompanyName = txtCarCompanyName.Text
                };

                // Call the API to register the person
                ApiService apiService = new ApiService();

               
                int registrationResult = await apiService.InsertCarCompany(carCompany);

                // Display a message based on the registration result
                if (registrationResult == 1)
                {
                    MessageBox.Show("Added successfuly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                    NavigationService.GetNavigationService(this).Navigate(new PricingPage());
                }
                else
                {
                    MessageBox.Show("Adding failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            txtCarCompanyName.Text = string.Empty;
        }
    }
}
