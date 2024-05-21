using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for PricingPage.xaml
    /// </summary>
    public partial class PricingPage : Page
    {
        public PricingPage()
        {
            InitializeComponent();
            ShowListView();
            IsManager();
        }

        public async Task IsManager()
        {
            //הפעולה בודקת אם המשתמש המחובר הוא מנהל במערכת ומשנה את רמת הנראות של תפריט ההקשר בהתאם
            ManagerList managerList = null;
            ApiService apiService = new ApiService();
            managerList = await apiService.GetManagerList();
            Manager manager = managerList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName
            && x.LastName == LoginPage.loggedInPerson.LastName);
            if (manager == null)
            {
                dataListView.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        public async void ShowListView()
        {
            //הפעולה מאתחלת את הליסט וויו
            ApiService apiService = new ApiService();
            var t = await apiService.GetPricingList();
            dataListView.ItemsSource = t;
        }

        private void UpdatePricingClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new UpdatePricingPage());
        }

        private async void RemovePricingClick(object sender, RoutedEventArgs e)
        {
            //הפעולה מופעלת כאשר המנהל לוחץ על כפתור המחיקה בממשק המשתמש כדי להסיר מחיר מרשימת המחירים ומסירה אותו מן הרשימה
            ApiService apiService = new ApiService();
            Pricing price = dataListView.SelectedItem as Pricing;
            apiService.DeletePricing(price);

            var t = await apiService.GetPricingList();
            t.Remove(price);

            dataListView.ItemsSource = null;
            dataListView.ItemsSource = t;
            
        }
        private void AddNewPricingClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new AddPricingPage());
        }

        private void AddNewCarCompanyClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new AddNewCarCompanyPage());
        }

        private void AddNewCarModelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new AddCarModelPage());
        }
    }
}
