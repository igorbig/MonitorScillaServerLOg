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
    /// Interaction logic for tab06_RelayUnit.xaml
    /// </summary>
    public partial class tab06_RelayUnit : Page
    {
        public tab06_RelayUnit()
        {
            InitializeComponent();
            DataContext = App.myApp.VMS;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.sRelayUnit.Name = App.myApp.VMS.TabRelayUnitName;
            App.myApp.sRelayUnit.Comment = App.myApp.VMS.TabRelayUnitComment;

            App.myApp.sRelayUnit.PositionX = App.myApp.VMS.TabRelayUnitPositionX;
            App.myApp.sRelayUnit.PositionY = App.myApp.VMS.TabRelayUnitPositionY;

            App.myApp.sRelayUnit.SlaveAddress = App.myApp.VMS.TabRelayUnitSlaveAddress;

            App.myApp.Plan.Draw();
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.TabRelayUnitName = App.myApp.sRelayUnit.Name;
            App.myApp.VMS.TabRelayUnitComment = App.myApp.sRelayUnit.Comment;

            App.myApp.VMS.TabRelayUnitPositionX = App.myApp.sRelayUnit.PositionX;
            App.myApp.VMS.TabRelayUnitPositionY = App.myApp.sRelayUnit.PositionY;

            App.myApp.VMS.TabRelayUnitSlaveAddress = App.myApp.sRelayUnit.SlaveAddress;
        }
    }
}
