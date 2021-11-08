using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for dlgValidSlave.xaml
    /// </summary>
    public partial class dlgValidSlave : Window
    {
        public ViewModelDlgValidSlave VM_DlgValidSlave = new ViewModelDlgValidSlave();
        public dlgValidSlave()
        {
            InitializeComponent();
            DataContext = VM_DlgValidSlave;
            App.myApp.VMDlgValidSlave = VM_DlgValidSlave;



        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }


    public class ViewModelDlgValidSlave : INotifyPropertyChanged
    {

     
        private string _dlgValidSlaveBuilding/* = 2323*/;// 
        public string dlgValidSlaveBuilding { get { return _dlgValidSlaveBuilding; } set { _dlgValidSlaveBuilding = value; NotifyPropertyChanged("dlgValidSlaveBuilding"); } }


        private string _dlgValidSlaveFloor/* = 2323*/;// 
        public string dlgValidSlaveFloor { get { return _dlgValidSlaveFloor; } set { _dlgValidSlaveFloor = value; NotifyPropertyChanged("dlgValidSlaveFloor"); } }

        private string _dlgValidSlaveModule/* = 2323*/;// 
        public string dlgValidSlaveModule { get { return _dlgValidSlaveModule; } set { _dlgValidSlaveModule = value; NotifyPropertyChanged("dlgValidSlaveModule"); } }

        private string _dlgValidSlaveModuleIp/* = 2323*/;// 
        public string dlgValidSlaveModuleIp { get { return _dlgValidSlaveModuleIp; } set { _dlgValidSlaveModuleIp = value; NotifyPropertyChanged("dlgValidSlaveModuleIp"); } }

        private string _dlgValidSlaveModuleDev/* = 2323*/;// 
        public string dlgValidSlaveModuleDev { get { return _dlgValidSlaveModuleDev; } set { _dlgValidSlaveModuleDev = value; NotifyPropertyChanged("dlgValidSlaveModuleDev"); } }

        private string _dlgValidSlaveModuleDevSlaveAdr/* = 2323*/;// 
        public string dlgValidSlaveModuleDevSlaveAdr { get { return _dlgValidSlaveModuleDevSlaveAdr; } set { _dlgValidSlaveModuleDevSlaveAdr = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSlaveAdr"); } }




        private string _dlgValidSlaveModuleDevType;// 
        public string dlgValidSlaveModuleDevType
        { get { return _dlgValidSlaveModuleDevType; } set { _dlgValidSlaveModuleDevType = value; NotifyPropertyChanged("dlgValidSlaveModuleDevType"); } }

        private string _dlgValidSlaveModuleDevSN_First;// 
        public string dlgValidSlaveModuleDevSN_First
        { get { return _dlgValidSlaveModuleDevSN_First; } set { _dlgValidSlaveModuleDevSN_First = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSN_First"); } }


        private string _dlgValidSlaveModuleDevSN_Second;// 
        public string dlgValidSlaveModuleDevSN_Second
        { get { return _dlgValidSlaveModuleDevSN_Second; }    set {_dlgValidSlaveModuleDevSN_Second = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSN_Second");} }

        private string _dlgValidSlaveModuleDevVerMajor; 
        public string dlgValidSlaveModuleDevVerMajor
        { get { return _dlgValidSlaveModuleDevVerMajor; } set { _dlgValidSlaveModuleDevVerMajor = value; NotifyPropertyChanged("dlgValidSlaveModuleDevVerMajor"); } }


        private string _dlgValidSlaveModuleDevVerMinor; 
        public string dlgValidSlaveModuleDevVerMinor
        { get { return _dlgValidSlaveModuleDevVerMinor; } set { _dlgValidSlaveModuleDevVerMinor = value; NotifyPropertyChanged("dlgValidSlaveModuleDevVerMinor"); } }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
