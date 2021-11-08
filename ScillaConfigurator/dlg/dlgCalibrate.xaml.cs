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
    /// Interaction logic for dlgCalibrate.xaml
    /// </summary>
    public partial class dlgCalibrate : Window
    {
        public ViewModelDlgCalibrate VM_DlgCalibrate = new ViewModelDlgCalibrate();
        public int SelectedCalibrType;
        public dlgCalibrate()
        {
            DataContext = VM_DlgCalibrate;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedCalibrType = VM_DlgCalibrate.SelectedCalibrTypeArray();
            DialogResult = true;
        }

        private void btnOk_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void radioBtn1_Checked(object sender, RoutedEventArgs e)
        {

        }
    }

    public class ViewModelDlgCalibrate : INotifyPropertyChanged
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




        private bool[] _CalibrTypeArray = new bool[] { true, false, false, false, false, false, false };
        public bool[] CalibrTypeArray  {  get { return _CalibrTypeArray; }  }
        public int SelectedCalibrTypeArray()
        {
            return Array.IndexOf(_CalibrTypeArray, true);
        }


        /*
        private string _dlgValidSlaveModuleDevType;// 
        public string dlgValidSlaveModuleDevType
        { get { return _dlgValidSlaveModuleDevType; } set { _dlgValidSlaveModuleDevType = value; NotifyPropertyChanged("dlgValidSlaveModuleDevType"); } }

        private string _dlgValidSlaveModuleDevSN_First;// 
        public string dlgValidSlaveModuleDevSN_First
        { get { return _dlgValidSlaveModuleDevSN_First; } set { _dlgValidSlaveModuleDevSN_First = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSN_First"); } }


        private string _dlgValidSlaveModuleDevSN_Second;// 
        public string dlgValidSlaveModuleDevSN_Second
        { get { return _dlgValidSlaveModuleDevSN_Second; } set { _dlgValidSlaveModuleDevSN_Second = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSN_Second"); } }

        private string _dlgValidSlaveModuleDevVerMajor;
        public string dlgValidSlaveModuleDevVerMajor
        { get { return _dlgValidSlaveModuleDevVerMajor; } set { _dlgValidSlaveModuleDevVerMajor = value; NotifyPropertyChanged("dlgValidSlaveModuleDevVerMajor"); } }


        private string _dlgValidSlaveModuleDevVerMinor;
        public string dlgValidSlaveModuleDevVerMinor
        { get { return _dlgValidSlaveModuleDevVerMinor; } set { _dlgValidSlaveModuleDevVerMinor = value; NotifyPropertyChanged("dlgValidSlaveModuleDevVerMinor"); } }

        */
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
