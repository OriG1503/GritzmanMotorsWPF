using ApiServiceNM;
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
            IsManager();
        }

        public void ShowLabels()
        {
            //הפעולה מציגה את פרטי המשתמש המחובר בעמוד ההתחברות בממשק המשתמש על ידי הגדרת תוכן תיבות התוויות בהתאם לפרטיו
            string firstName = LoginPage.loggedInPerson.FirstName;
            firstNameLabel.Content = $"First Name: {firstName}";

            string LastName = LoginPage.loggedInPerson.LastName;
            lastNameLabel.Content = $"Last Name: {LastName}";

            string Birthdate = (LoginPage.loggedInPerson.DateOfBirth).ToString();
            BirthDateLabel.Content = $"Date Of Birth: {Birthdate}";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //הפעולה מבצעת התנתקות מהחשבון ומבצעת ניווט לדף ההתחברות
            NavigationService?.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
        }

        public async Task IsManager()
        {
            //הפעולה בודקת אם המשתמש הנוכחי הוא מנהל במערכת ובמקרה וכן, היא משנה את נראות התוויות
            ApiService apiService = new ApiService();
            ManagerList managerList = await apiService.GetManagerList();
            Manager manager = managerList.Find(x => x.FirstName ==
            LoginPage.loggedInPerson.FirstName && x.LastName == LoginPage.loggedInPerson.LastName);
            if (manager != null)
            {
                sumOfAllOrdersLabel.Visibility = Visibility.Visible;
                averageOfAllOrdersLabel.Visibility = Visibility.Visible;
                SumOfAllOrders();
            }
        }

        public async void SumOfAllOrders()
        {
            //הפעולה מחשבת ומציגה את הסכום והממוצע של מחירי כל ההזמנות שהתקבלו ומציגה אותן בממשק המשתמש
            ApiService apiService = new ApiService();
            List<Order> orders = new List<Order>();
            orders = await apiService.GetOrderList();
            double sumOfAllOrders = orders.Sum(x => x.PriceCode.Price);
            double averageOfAllOrders = orders.Average(x => x.PriceCode.Price);
            sumOfAllOrdersLabel.Content = $"Sum Of All Orders: {sumOfAllOrders}";
            averageOfAllOrdersLabel.Content = $"Average Of All Orders: {averageOfAllOrders}";
        }

    }
}
