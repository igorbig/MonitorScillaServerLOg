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
using System.Windows.Shapes;

namespace _ScillaConfigurator.dlg
{
    /// <summary>
    /// Логика взаимодействия для WindowInfoDevice.xaml
    /// </summary>
    public partial class WindowInfoDevice : Window
    {
        public WindowInfoDevice()
        {
            InitializeComponent();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string InfoDevice
        {
           
            get { return infodeviceBox.Text; }
        }
    }
}
