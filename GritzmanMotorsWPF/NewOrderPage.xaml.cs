using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            //הפעולה יוצרת הזמנה חדשה ומטפלת בהוספתה למסד הנתונים דרך האיי פי איי, מציגה הודעות הצלחה או שגיאה ומנווטת לדף היסטוריית הזמנות
            try
            {
                ApiService apiService = new ApiService();
                CarCompany selectedCarCompany = carCompanyComboBox.SelectedItem as CarCompany;
                CarModel selectedCarModel = carModelComboBox.SelectedItem as CarModel;

                if (dpDateOfTreatment.SelectedDate == null || dpDateOfTreatment.SelectedDate < DateTime.Now)
                {
                    MessageBox.Show("Invalid date. Treatment date can only be in the future.", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Order order = new Order
                {
                    PriceCode = prc,
                    CustomerCode = (await apiService.GetCustomerList()).Find(x => x.Id == LoginPage.loggedInPerson.Id),
                    EmployeeCode = (await apiService.GetEmployeeList()).Find(x => x.Id == 68),
                    DateOfTreatment = DateOnly.FromDateTime(DateTime.Parse(dpDateOfTreatment.SelectedDate.ToString())),
                    CarReady = false,
                    DateOfOrder = DateTime.Now
                };

                int orderResult = await apiService.InsertOrder(order);

                // Display a message based on the registration result
                if (orderResult == 1)
                {
                    MessageBox.Show("Your order was added to our database successfully!", 
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new OrderHistoryPage());
                }
                else
                {
                    MessageBox.Show("An error occurred. Please try again.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or display an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CarCompanyComboBox()
        {
            //הפעולה מציגה את שמות חברות הרכב בתוך קומבו בוקס על פי נתונים שנשלפים מהאיי פי איי ומאפשרת בחירה מתוך האפשרויות המוצגות
            ApiService apiService = new ApiService();
            var x = (await (apiService.GetCarCompanyList())).Select(x => x.CarCompanyName);
            lst = (await (apiService.GetCarCompanyList()));
            carCompanyComboBox.ItemsSource = lst.Select(x => x.CarCompanyName);
        }

        private async void CarModelComboBox()
        {
            //הפעולה מציגה בקומבו בוקס את שמות דגמי הרכב של החברה שנבחרה על ידי המשתמש
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
            price.Content = "Price: ";
            CarModelComboBox();
        }

        private async void carModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //הפעולה מציגה את המחיר של הדגם הנבחר בקומבו בוקס בתוך התווית
            if (carModelComboBox.SelectedItem == null)
                return;
            ApiService srv = new();
            // Get price
            prc = (await srv.GetPricingList()).Find(x => x.ModelCode.CarModelName == carModelComboBox.SelectedItem.ToString());
            if (prc != null)
                price.Content = "Price: " + prc!.Price;
        }

    }
}