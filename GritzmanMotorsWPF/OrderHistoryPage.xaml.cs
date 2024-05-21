using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrderHistoryPage.xaml
    /// </summary>
    public partial class OrderHistoryPage : Page
    {
        static ApiService apiService = new ApiService();
        List<Order> orders = new List<Order>();
        public OrderHistoryPage()
        {
            InitializeComponent();
            ShowListView();
        }

        public async void ShowListView()
        {
            //ApiService apiService = new ApiService();
            orders = await apiService.GetOrderList();
            dataListView.ItemsSource = orders;
            await IsManager();
            await IsEmployee();
        }

        public async Task IsCustomer()
        {
            //הפעולה בודקת האם המשתמש המחובר הוא לקוח במערכת ומציגה את רשימת ההזמנות שלו אם הוא קיים במערכת
            CustomerList customerList;
            customerList = null;
            ApiService apiService = new ApiService();
            customerList = await apiService.GetCustomerList();
            Customer customer = customerList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName 
            && x.LastName == LoginPage.loggedInPerson.LastName);
            if (customer != null)
            {
                List<Order> customerOrdersList = (await apiService.GetOrderList()).FindAll(x => x.CustomerCode.Id == customer.Id);
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = customerOrdersList;
            }
        }

        public async Task IsEmployee()
        {
            //הפעולה בודקת האם המשתמש המחובר הוא עובד במערכת ומציגה את רשימת ההזמנות שטרם הסתיימו אם כן
            EmployeeList employeeList;
            employeeList = null;
            ApiService apiService = new ApiService();
            employeeList = await apiService.GetEmployeeList();
            Employee employee = employeeList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName
            && x.LastName == LoginPage.loggedInPerson.LastName);
            if (employee != null)
            {
                markAsDoneButton.Visibility = Visibility.Visible;
                List<Order> carReadyFalseOrdersList = (await apiService.GetOrderList()).Where(x => x.CarReady==false).ToList();
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = carReadyFalseOrdersList;
                orders = carReadyFalseOrdersList;
            }
            await IsCustomer();
        }

        private async void MarkAsDone_Click(object sender, RoutedEventArgs e)
        {
            //הפעולה מסמנת הזמנה נבחרת כבוצעה ומעדכנת את המספרים ברשימת ההזמנות
            Order selectedOrder = (Order)dataListView.SelectedItem;
            if (selectedOrder != null)
            {
                int id = selectedOrder.Id;
                int ord = orders.FindIndex(order=>order.Id== id);
                selectedOrder.CarReady = true;
                selectedOrder.EmployeeCode = (await apiService.GetEmployeeList()).Find(x => x.Id == LoginPage.loggedInPerson.Id);
                orders[ord] = selectedOrder;
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = orders;
                apiService.UpdateOrder(selectedOrder);
            }
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
                txtFirstName.Visibility = Visibility.Collapsed;
                filterByCustomerName.Visibility = Visibility.Collapsed;
            }
        }

        private void SortByDateAscClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי תאריך בסדר עולה
            IOrderedEnumerable<Order> sortByDateAscList = orders.OrderBy(x => x.DateOfOrder);
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByDateAscList;
        }

        private void SortByDateDescClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי תאריך בסדר יורד
            IOrderedEnumerable<Order> sortByDateDescList = orders.OrderByDescending(x => x.DateOfOrder);
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByDateDescList;
        }

        private void SortByPriceAscClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי מחיר ההזמנה בסדר עולה
            IOrderedEnumerable<Order> sortByPriceAscList = orders.OrderBy(x => x.PriceCode.Price);
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByPriceAscList;
        }

        private void SortByPriceDescClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי מחיר ההזמנה בסדר יורד
            IOrderedEnumerable<Order> sortByPriceDescList = orders.OrderByDescending(x => x.PriceCode.Price);
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByPriceDescList;
        }

        private void SortByCarReadyFalseClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי כל הרכבים שעוד לא מוכנים ועוד לא עברו טיפול
            List<Order> sortByCarReadyFalseList = orders.Where(x => x.CarReady == false).ToList(); 
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByCarReadyFalseList;
        }

        private void SortByCarReadyTrueClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי כל הרכבים שכבר מוכנים ועברו טיפול
            List<Order> sortByCarReadyTrueList = orders.Where(x => x.CarReady == true).ToList();
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByCarReadyTrueList;
        }

        private void CancelAllSortsClick(object sender, RoutedEventArgs e)
        {
            //הפעולה מחזירה את התצוגה לתצוגה המקורית
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = orders;
        }

        private void SortByCustomerNameClick(object sender, RoutedEventArgs e)
        {
            //הפעולה ממיינת את רשימת ההזמנות לפי השם של הלקוח שהמנהל הכניס
            string firstName = txtFirstName.Text;
            List<Order> sortByCustomerName = orders.Where(x => x.CustomerCode.FirstName.Contains(firstName)).ToList();
            dataListView.ItemsSource = null;
            dataListView.ItemsSource = sortByCustomerName;
        }
    }
}
