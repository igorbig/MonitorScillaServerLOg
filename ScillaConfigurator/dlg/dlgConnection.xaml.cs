using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _ScillaConfigurator.dlg
{
    /// <summary>
    /// Interaction logic for dlgConnection.xaml
    /// </summary>
    public partial class dlgConnection : Window
    {
        public ViewModelDlgConnection VM_DlgConnection = new ViewModelDlgConnection();
        public dlgConnection()
        {
            InitializeComponent();
            DataContext = VM_DlgConnection;

            String str = Dns.GetHostName();
            HostName.Content = str;
            //App.myApp.VMM.strDNS_Name = str;

            listBoxIP.Items.Clear();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
            {
                listBoxIP.Items.Add(ipHostInfo.AddressList[i]);
            }

            if (listBoxIP.Items.Count > 0)
                listBoxIP.SelectedIndex = listBoxIP.Items.Count - 1;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    public class ViewModelDlgConnection : INotifyPropertyChanged
    {

        private int _serverListenerPort/* = 2424*/;// 
        public int serverListenerPort { get { return _serverListenerPort; } set { _serverListenerPort = value; NotifyPropertyChanged("serverListenerPort"); } }

        private int _moduleListenerPort/* = 2323*/;// 
        public int moduleListenerPort { get { return _moduleListenerPort; } set { _moduleListenerPort = value; NotifyPropertyChanged("moduleListenerPort"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
