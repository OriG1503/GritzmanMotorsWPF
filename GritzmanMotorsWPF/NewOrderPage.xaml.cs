using ApiServiceNM;
using Model;
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
using ViewModel;

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for NewOrderPage.xaml
    /// </summary>
    public partial class NewOrderPage : Page
    {
        private CarCompanyList lst;
        private Pricing prc;
        public NewOrderPage()
        {
            InitializeComponent();
            CarCompanyComboBox();
        }

        private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiService apiService = new ApiService();
                CarCompany selectedCarCompany = carCompanyComboBox.SelectedItem as CarCompany;
                CarModel selectedCarModel = carModelComboBox.SelectedItem as CarModel;

                if (dpDateOfTreatment.SelectedDate == null || dpDateOfTreatment.SelectedDate < DateTime.Now)
                {
                    MessageBox.Show("Invalid date. Treatment date can only be in the future.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Order order = new Order
                {
                    PriceCode = prc,
                    CustomerCode = LoginPage.loggedInPerson as Customer,
                    EmployeeCode = (await apiService.GetEmployeeList()).Find(x => x.Id == 68),
                    DateOfTreatment = DateOnly.FromDateTime(DateTime.Parse(dpDateOfTreatment.SelectedDate.ToString())),
                    CarReady = false,
                    DateOfOrder = DateTime.Now
                };

            int orderResult = await apiService.InsertOrder(order);
            // Display a message based on the registration result
            if (orderResult == 1)
            {
                MessageBox.Show("Your order was added to our database successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //ClearFields();
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                MessageBox.Show("An error occurred. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private async void carModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //יש כאן שגיאה כשאני משנה את הבחירה של הסוג רכב עצמו אחרי שכבר בחרתי פעם ראשונה
            ApiService srv = new();
            // Get price
            prc = (await srv.GetPricingList()).Find(x => x.ModelCode.CarModelName == carModelComboBox.SelectedItem.ToString());
            price.Content = "Price: " + prc!.Price;
        }

    }
}