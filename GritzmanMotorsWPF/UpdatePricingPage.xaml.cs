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
    public partial class UpdatePricingPage : Page
    {
        private CarCompanyList lst;
        private Pricing prc;
        private readonly Regex priceRegex = new Regex(@"^\d+(?:\.\d+)?$");
        private ApiService apiService = new ApiService();


        public UpdatePricingPage()
        {
            InitializeComponent();
            CarCompanyComboBox();
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

        private async void UpdatePricing_Click(object sender, RoutedEventArgs e)
        {
            //הפעולה מעדכנת את המחיר של דגם רכב קיים במערכת ומנווטת לדף המחירון אם העדכון הצליח, אחרת מציגה הודעת שגיאה
            try
            {
                // Call the API to register the person
                ApiService apiService = new ApiService();
                if (!IsValidPrice(txtPrice.Text))
                {
                    MessageBox.Show("Invalid price. Price should be only '.' and digits.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Pricing pricing = (await apiService.GetPricingList()).Find(x => x.ModelCode.CarModelName == carModelComboBox.SelectedItem.ToString());
                pricing.Price = double.Parse(txtPrice.Text);

                int registrationResult = await apiService.UpdatePricing(pricing);

                // Display a message based on the registration result
                if (registrationResult == 1)
                {
                    MessageBox.Show("Updated successfuly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                    NavigationService.GetNavigationService(this).Navigate(new PricingPage());
                }
                else
                {
                    MessageBox.Show("Updating failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            //הפעולה מציבה את רשימת שמות החברות הרכב בתיבת הרשימה
            var x = (await (apiService.GetCarCompanyList())).Select(x => x.CarCompanyName);
            lst = (await (apiService.GetCarCompanyList()));
            carCompanyComboBox.ItemsSource = lst.Select(x => x.CarCompanyName);
        }

        private void CarCompanyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //הפעולה מפעילה את הפעולה
            //CarModelComboBox
            //כאשר המשתמש בוחר חברת רכב
            price.Content = "Current Price: ";
            CarModelComboBox();
        }

        private async void CarModelComboBox()
        {
            //הפעולה מציבה את רשימת דגמי הרכב של החברה הנבחרת בתיבת הרשימה הנפתחת של חברות הרכב בממשק המשתמש
            carModelComboBox.ItemsSource = null;
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

        private async void carModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //הפעולה מציבה את המחיר הנוכחי של דגם הרכב הנבחר בתיבת הרשימה הנפתחת של דגמי הרכב
            if (carModelComboBox.SelectedItem == null)
                return;
            ApiService srv = new();
            // Get price
            prc = (await srv.GetPricingList()).Find(x => x.ModelCode.CarModelName == carModelComboBox.SelectedItem.ToString());
            if( prc != null )
                price.Content = "Current Price: " + prc!.Price;
        }

        

    }
}
