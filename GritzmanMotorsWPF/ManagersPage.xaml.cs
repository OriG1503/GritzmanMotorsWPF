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
using ApiServiceNM;
using Model;

namespace GritzmanMotorsWPF
{
    
    public partial class ManagersPage : Page
    {

        private ApiService apiService = new ApiService();
        private ManagerList managerList;
        public ManagersPage()
        {
            InitializeComponent();
            ShowListView();
        }

        public async void ShowListView()
        {
            managerList = await apiService.GetManagerList();
            dataListView.ItemsSource = managerList;
        }

        private void AddNewManagerClick(object sender, RoutedEventArgs e)
        {
            if(LoginPage.loggedInPerson.Id==1)
                NavigationService.GetNavigationService(this).Navigate(new AddManagerPage());
            else
                MessageBox.Show("You Are Not The CEO!.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private async void RemoveManagerClick(object sender, RoutedEventArgs e)
        {

            if (LoginPage.loggedInPerson.Id != 1)
                MessageBox.Show("You Are Not The CEO!.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {
                ApiService apiService = new ApiService();
                Manager manager = dataListView.SelectedItem as Manager;
                if (manager.Id != 1 || manager.Id != LoginPage.loggedInPerson.Id)
                {
                    apiService.DeleteManager(manager);
                    managerList.Remove(manager);
                    dataListView.ItemsSource = null;
                    dataListView.ItemsSource = managerList;
                }
            }
            
        }
    }
}
