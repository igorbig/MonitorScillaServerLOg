using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for dlgDrawRawData.xaml
    /// </summary>
    public partial class dlgDrawRawData : Window
    {

        public ViewModelDlgDrawRawData VM_DlgDrawRawData = new ViewModelDlgDrawRawData();
        MyGraphMS myGraphMB = null;
        static public Timer timerPollingMS;

        public dlgDrawRawData()
        {
            DataContext = VM_DlgDrawRawData;
            App.myApp.VMDlgDrawRawData = VM_DlgDrawRawData;

            InitializeComponent();
            this.Unloaded += this.OnUnloaded;
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            myGraphMB = new MyGraphMS();// (GraphCanvasMB, textblok);

            App.GraphCanvas = GraphCanvasMB;
            App.TextBlockFourZero = textblok;
            App.iReady = 0;
            App.ReadyAllData = false;
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
            
            StartPollingMultiSensor(true);
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {

            App.GraphCanvas = null;
            App.TextBlockFourZero = null;
            //myGraphMB = null;
            if (timerPollingMS != null)
            {
                timerPollingMS.Dispose();
                timerPollingMS = null;
            }
        }

        private void TabControlGraph_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = VM_DlgDrawRawData.MyGraphTabControlSelectedIndex;
            if (GraphCanvasMB != null)
            {
                App.myApp.Dispatcher.BeginInvoke((Action)(() =>
                {
                    myGraphMB.DrawGraph(GraphCanvasMB,i);
                }));
            }
        }

        private void btnCmdMRDG_Click(object sender, RoutedEventArgs e)
        {
            int ParamId = 67;
            byte[] ParamData = new byte[1];
            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MRDG", ParamId, ParamData, ParamData.Length, false);

        }

        private void StageLabelSetup()
        {
            //App.MRDG_RawData[0] = 0;
            switch (App.MRDG_RawData[0])
            {
                case 0:
                    StageMultiSensor.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageMultiSensor.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageMultiSensor.Foreground = Brushes.Red;
                    break;

            }

            switch (App.MRDG_RawData[1] & 0x3)
            {
                case 0:
                    StageSmogOpt.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageSmogOpt.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageSmogOpt.Foreground = Brushes.Red;
                    break;
            }

            switch ((App.MRDG_RawData[1] >> 2) & 0x3)
            {
                case 0:
                    StageTempAnl.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageTempAnl.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageTempAnl.Foreground = Brushes.Red;
                    break;
            }

            switch ((App.MRDG_RawData[1] >> 4) & 0x3)
            {
                case 0:
                    StageTempDgt.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageTempDgt.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageTempDgt.Foreground = Brushes.Red;
                    break;
            }


            switch ((App.MRDG_RawData[1] >> 6) & 0x3)
            {
                case 0:
                    StageSmogEch.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageSmogEch.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageSmogEch.Foreground = Brushes.Red;
                    break;
            }

            switch ((App.MRDG_RawData[1] >> 8) & 0x3)
            {
                case 0:
                    StageFlame.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageFlame.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageFlame.Foreground = Brushes.Red;
                    break;
            }
            switch ((App.MRDG_RawData[1] >> 10) & 0x3)
            {
                case 0:
                    StageCO.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageCO.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageCO.Foreground = Brushes.Red;
                    break;
            }
            switch ((App.MRDG_RawData[1] >> 12) & 0x3)
            {
                case 0:
                    StageVOC.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageVOC.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageVOC.Foreground = Brushes.Red;
                    break;
            }
        }

        private void btnCmdDraw_Click(object sender, RoutedEventArgs e)
        {
            myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawData.MyGraphTabControlSelectedIndex);

            //Состояние MultiSensor
            StageLabelSetup();


        }

        private void TabControlGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if(myGraphMB!=null)
             //   MyGraphMS.DrawAsync(VM_DlgDrawRawData.MyGraphTabControlSelectedIndex);
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timerPollingMS == null)
                StartPollingMultiSensor(true);
            else
                StartPollingMultiSensor(false);
        }
        public void StartPollingMultiSensor(bool start)
        {
            if (start)
            {
                // Делегат для типа Timer
                TimerCallback timerPollingCallback = new TimerCallback(DoPolling);
                //Класс Timer из пространства имен System.Threading предлагает ключевую функциональность. 
                //В его конструкторе можно передавать делегат, который должен вызываться через указываемый интервал времени.
                //pageModbus.RunType = 3;
                timerPollingMS = new Timer(timerPollingCallback, null, 1000, 1000);
                btnStartTimer.Content = "Stop Timer";
            }
            else
            {
                timerPollingMS.Dispose();
                timerPollingMS = null;
                btnStartTimer.Content = "Start Timer";
            }
        }
         void DoPolling(object state)
        {

            int ParamId = 67;
            byte[] ParamData = new byte[1];
            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MRDG", ParamId, ParamData, ParamData.Length, false);
            //GraphCanvasMB.Dispatcher.BeginInvoke((Action)(() => { MyGraphMS.DrawAsync(VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { StageLabelSetup(); }));
            //MyGraphMB.Draw1();
        }
         private void btnReStartDraw_Click(object sender, RoutedEventArgs e)
        {
            App.iReady = 0;
            App.ReadyAllData = false;
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
        }

        private void btnResetStage_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[4];
            int ParamId = 70;

            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[1] = 24;//RegisterAddres
            ParamData[2] = 0;//Beep
            ParamData[3] = 0;//Beep
            short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);
        }
    }



    public class ViewModelDlgDrawRawData : INotifyPropertyChanged
    {

        private int _MyGraphTabControlSelectedIndex;
        public int MyGraphTabControlSelectedIndex { get { return _MyGraphTabControlSelectedIndex; } set { _MyGraphTabControlSelectedIndex = value; NotifyPropertyChanged("MyGraphMBTabControlSelectedIndex"); } }


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

        private string _dlgValidSlaveModuleDevSlaveAdr = "2323";// 
        public string dlgValidSlaveModuleDevSlaveAdr { get { return _dlgValidSlaveModuleDevSlaveAdr; } set { _dlgValidSlaveModuleDevSlaveAdr = value; NotifyPropertyChanged("dlgValidSlaveModuleDevSlaveAdr"); } }









        private bool _TabMSensor_InclAnl_S_o = true;
        public bool TabMSensor_InclAnl_S_o { get { return _TabMSensor_InclAnl_S_o; } set { _TabMSensor_InclAnl_S_o = value; NotifyPropertyChanged("TabMSensor_InclAnl_S_o"); } }

        private bool _TabMSensor_InclAnl_T_a = true;
        public bool TabMSensor_InclAnl_T_a { get { return _TabMSensor_InclAnl_T_a; } set { _TabMSensor_InclAnl_T_a = value; NotifyPropertyChanged("TabMSensor_InclAnl_T_a"); } }

        private bool _TabMSensor_InclAnl_T_d = true;
        public bool TabMSensor_InclAnl_T_d { get { return _TabMSensor_InclAnl_T_d; } set { _TabMSensor_InclAnl_T_d = value; NotifyPropertyChanged("TabMSensor_InclAnl_T_d"); } }

        private bool _TabMSensor_InclAnl_S_e = true;
        public bool TabMSensor_InclAnl_S_e { get { return _TabMSensor_InclAnl_S_e; } set { _TabMSensor_InclAnl_S_e = value; NotifyPropertyChanged("TabMSensor_InclAnl_S_e"); } }

        private bool _TabMSensor_InclAnl_Flm = true;
        public bool TabMSensor_InclAnl_Flm { get { return _TabMSensor_InclAnl_Flm; } set { _TabMSensor_InclAnl_Flm = value; NotifyPropertyChanged("TabMSensor_InclAnl_Flm"); } }

        private bool _TabMSensor_InclAnl_CO = true;
        public bool TabMSensor_InclAnl_CO { get { return _TabMSensor_InclAnl_CO; } set { _TabMSensor_InclAnl_CO = value; NotifyPropertyChanged("TabMSensor_InclAnl_CO"); } }

        private bool _TabMSensor_InclAnl_VOC = true;
        public bool TabMSensor_InclAnl_VOC { get { return _TabMSensor_InclAnl_VOC; } set { _TabMSensor_InclAnl_VOC = value; NotifyPropertyChanged("TabMSensor_InclAnl_VOC"); } }








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
