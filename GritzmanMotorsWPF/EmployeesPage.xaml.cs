using ApiServiceNM;
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
using Model;

namespace GritzmanMotorsWPF
{
    /// <summary>
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private ApiService apiService = new ApiService();
        private EmployeeList employeeList; 

        public EmployeesPage()
        {
            InitializeComponent();
            ShowListView();
        }

        public async void ShowListView()
        {
            employeeList = await apiService.GetEmployeeList();
            dataListView.ItemsSource = employeeList;
        }

        private void AddNewEmployeeClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new AddEmployeePage());
        }

        private async void RemoveEmployeeClick(object sender, RoutedEventArgs e)
        {
            //הפעולה מוחקת עובד מרשימת העובדים ומעדכנת את התצוגה
            ApiService apiService = new ApiService();
            Employee employee = dataListView.SelectedItem as Employee;
            if(employee.Id != 68)
            {
                apiService.DeleteEmployee(employee);
                employeeList.Remove(employee);
                dataListView.ItemsSource = null;
                dataListView.ItemsSource = employeeList;
            }
        }
    }
}
