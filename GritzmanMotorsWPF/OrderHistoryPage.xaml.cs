using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrderHistoryPage.xaml
    /// </summary>
    public partial class OrderHistoryPage : Page
    {
        static ApiService apiService = new ApiService();
        public OrderHistoryPage()
        {
            InitializeComponent();
            ShowListView();
            //IsEmployee();
            //IsCustomer();
        }

        public async void ShowListView()
        {
            //ApiService apiService = new ApiService();
            var t = await apiService.GetOrderList();
            dataListView.ItemsSource = t;
            await IsEmployee();
        }

        public async Task IsCustomer()
        {
            CustomerList customerList;
            customerList = null;
            ApiService apiService = new ApiService();
            customerList = await apiService.GetCustomerList();
            Customer customer = customerList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName && x.LastName == LoginPage.loggedInPerson.LastName);
            if (customer != null)
            {
                List<Order> customerOrdersList = (await apiService.GetOrderList()).FindAll(x => x.CustomerCode.Id == customer.Id);
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = customerOrdersList;
            }
        }
        List<Order> orders = new List<Order>();
        public async Task IsEmployee()
        {
            EmployeeList employeeList;
            employeeList = null;
            ApiService apiService = new ApiService();
            employeeList = await apiService.GetEmployeeList();
            Employee employee = employeeList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName && x.LastName == LoginPage.loggedInPerson.LastName);
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

        private void MarkAsDone_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)dataListView.SelectedItem;
            if (selectedOrder != null)
            {
                int id = selectedOrder.Id;
                int ord = orders.FindIndex(order=>order.Id== id);
                selectedOrder.CarReady = true;
                selectedOrder.EmployeeCode = LoginPage.loggedInPerson as Employee;
                orders[ord] = selectedOrder;
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = orders;
                apiService.UpdateOrder(selectedOrder);
            }
        }
    }
}
