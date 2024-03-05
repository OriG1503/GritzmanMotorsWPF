using ApiServiceNM;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public static Person loggeduser = null;
        ManagerList managerList;
        EmployeeList employeeList;
        public NavigationBar()
        {
            InitializeComponent();
            loggeduser = LoginPage.loggedInPerson;
            IsManager();
            IsEmployee();
        }

        public async Task IsManager()
        {
            managerList = null;
            ApiService apiService = new ApiService();
            managerList = await apiService.GetManagerList();
            Manager manager = managerList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName && x.LastName == LoginPage.loggedInPerson.LastName);
            if (manager != null)
            {
                NewOrderLabel.Visibility = Visibility.Collapsed;
                EmployeesLabel.Visibility = Visibility.Visible;
                DataLabel.Visibility = Visibility.Visible;
            }
        }

        public async Task IsEmployee()
        {
            employeeList = null;
            ApiService apiService = new ApiService();
            employeeList = await apiService.GetEmployeeList();
            Employee employee = employeeList.Find(x => x.FirstName == LoginPage.loggedInPerson.FirstName && x.LastName == LoginPage.loggedInPerson.LastName);
            if (employee != null)
            {
                NewOrderLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void HomePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new HomePage());
        }

        private void PricingPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new PricingPage());
        }

        private void NewOrderPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new NewOrderPage());
        }

        private void OrderHistoryPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new OrderHistoryPage());
        }

        private void EmployeesPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new EmployeesPage());
        }

        private void DataPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new DataPage());
        }

    }
}
