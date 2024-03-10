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
        }

        public async void ShowListView()
        {
            ApiService apiService = new ApiService();
            var t = await apiService.GetPricingList();
            dataListView.ItemsSource = t;
        }
    }
}
