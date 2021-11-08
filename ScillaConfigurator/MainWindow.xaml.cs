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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 
        public MainWindow()
        {
            InitializeComponent();
        }
 
        private void btnDlgConnection_Click(object sender, RoutedEventArgs e)
        {
            _ScillaConfigurator.dlg.dlgConnection dlg = new _ScillaConfigurator.dlg.dlgConnection
            {
                Owner = this
              //  DocumentMargin = documentTextBox.Margin
            };

            // Configure the dialog box

            // Open the dialog box modally 

            dlg.VM_DlgConnection.serverListenerPort = App.myApp.serverListenerPort;
            dlg.VM_DlgConnection.moduleListenerPort = App.myApp.moduleListenerPort;
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                App.myApp.serverListenerPort= dlg.VM_DlgConnection.serverListenerPort;
                App.myApp.moduleListenerPort= dlg.VM_DlgConnection.moduleListenerPort;

                //App.myApp.taskUDP_Listner = Task.Factory.StartNew(App.myApp.UDP_listening_PI1);
                //btnStartListener.IsEnabled = false;
                //App.myApp.VMM.ListnerOk = true;
               
            }
            
        }
    }
}
