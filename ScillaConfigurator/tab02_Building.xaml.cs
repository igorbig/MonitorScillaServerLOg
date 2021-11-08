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

namespace _ScillaConfigurator
{
    /// <summary>
    /// Interaction logic for tab02_Building.xaml
    /// </summary>
    public partial class tab02_Building : Page
    {
        public tab02_Building()
        {
            InitializeComponent();
            DataContext = App.myApp.VMCnfTab02;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.sBuilding.Name = App.myApp.VMCnfTab02.Name;
            App.myApp.sBuilding.Comment = App.myApp.VMCnfTab02.Comment;
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMCnfTab02.Name= App.myApp.sBuilding.Name;
            App.myApp.VMCnfTab02.Comment= App.myApp.sBuilding.Comment;
        }
    }
}
