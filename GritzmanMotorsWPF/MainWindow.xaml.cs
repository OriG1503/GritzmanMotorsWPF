using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.ResizeMode = ResizeMode.NoResize;
            //this.SizeToContent = SizeToContent.Manual;
            this.ResizeMode = ResizeMode.CanResize;
            this.SizeToContent = SizeToContent.Manual;
        }
        private void Login()
        {
            frm.Navigate(new NavigationBar());
        }
    }
}