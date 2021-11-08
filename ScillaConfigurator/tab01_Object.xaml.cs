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
    /// Interaction logic for tab01_Object.xaml
    /// </summary>
    public partial class tab01_Object : Page
    {
        
        public tab01_Object()
        {
            InitializeComponent();
            DataContext = App.myApp.VMCnfTab01;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.sScillaObject != null)
            {
                App.myApp.sScillaObject.Name = App.myApp.VMCnfTab01.Name;
                App.myApp.sScillaObject.Region = App.myApp.VMCnfTab01.Region;
                App.myApp.sScillaObject.City = App.myApp.VMCnfTab01.City;
                App.myApp.sScillaObject.Build = App.myApp.VMCnfTab01.Build;
                App.myApp.sScillaObject.Person = App.myApp.VMCnfTab01.Person;
                App.myApp.sScillaObject.TelN = App.myApp.VMCnfTab01.TelN;
            }
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.sScillaObject != null)
            {
                App.myApp.VMCnfTab01.Name = App.myApp.sScillaObject.Name;
                App.myApp.VMCnfTab01.Region = App.myApp.sScillaObject.Region;
                App.myApp.VMCnfTab01.City = App.myApp.sScillaObject.City;
                App.myApp.VMCnfTab01.Build = App.myApp.sScillaObject.Build;
                App.myApp.VMCnfTab01.Person = App.myApp.sScillaObject.Person;
                App.myApp.VMCnfTab01.TelN = App.myApp.sScillaObject.TelN;
            }
        }
    }
}
