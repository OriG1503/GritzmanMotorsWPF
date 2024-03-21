using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddPricingPage.xaml
    /// </summary>
    public partial class AddPricingPage : Page
    {

        private CarCompanyList lst;
        private CarModelList lstCarModel;
        private readonly Regex priceRegex = new Regex(@"^\d+(?:\.\d+)?$");


        public AddPricingPage()
        {
            InitializeComponent();
            CarCompanyComboBox();
        }

        private async void AddNewPricing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Call the API to register the person
                ApiService apiService = new ApiService();

                lstCarModel = await apiService.GetCarModelList();

                if (!IsValidPrice(txtPrice.Text))
                {
                    MessageBox.Show("Invalid price. It should be greater than 0 and be only '.' and digits.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Pricing pricing = new Pricing
                {
                    ModelCode = (await apiService.GetCarModelList()).Find(x => x.CarModelName == carModelComboBox.SelectedItem.ToString()),

                    Price = double.Parse(txtPrice.Text.ToString())
                };


                int registrationResult = await apiService.InsertPricing(pricing);

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

        private async void CarCompanyComboBox()
        {
            ApiService apiService = new ApiService();
            var x = (await (apiService.GetCarCompanyList())).Select(x => x.CarCompanyName);
            lst = (await (apiService.GetCarCompanyList()));
            carCompanyComboBox.ItemsSource = lst.Select(x => x.CarCompanyName);
        }

        private async void CarModelComboBox()
        {
            carModelComboBox.ItemsSource = null;
            ApiService apiService = new ApiService();
            var x = carCompanyComboBox.SelectedItem as string;

            var y = (await (apiService.GetCarModelList()))
                // Filter enumerable by a certain lambda condition
                .Where(z => z.CompanyCode.Id == lst
                // Find a singular item based on a lambda condition
                .Find(q => q.CarCompanyName == x)!.Id);
           
            carModelComboBox.ItemsSource = y
                // Map enumerable with a callback fn based on the lambda condition
                .Select(x => x.CarModelName);
        }

        private void CarCompanyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarModelComboBox();
        }

        private void ClearFields()
        {
            // Clear the textboxes and other fields
            txtPrice.Text = string.Empty;
        }

        private bool IsValidPrice(string price)
        {
            return priceRegex.IsMatch(price);
        }
    }
}
