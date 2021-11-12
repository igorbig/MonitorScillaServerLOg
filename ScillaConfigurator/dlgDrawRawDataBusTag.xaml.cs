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

namespace _ScillaConfigurator
{
    /// <summary>
    /// Логика взаимодействия для dlgDrawRawDataBusTag.xaml
     /// </summary>
    public partial class dlgDrawRawDataBusTag : Window
    {
     public ViewModelDlgDrawRawDataBusTag VM_DlgDrawRawDataBusTag = new ViewModelDlgDrawRawDataBusTag();
    GraphBusTag myGraphMB = null;
    static public Timer timerPollingMS;
      public dlgDrawRawDataBusTag()
        {
            DataContext = VM_DlgDrawRawDataBusTag;
            App.myApp.VMDlgDrawRawDataBusTag = VM_DlgDrawRawDataBusTag;

            InitializeComponent();
            this.Unloaded += this.OnUnloaded;
            this.Loaded += this.OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            myGraphMB = new  GraphBusTag();// (GraphCanvasMB, textblok);

            App.GraphCanvas = GraphCanvasMB;
            App.TextBlockFourZero = textblok;
            App.iReady = 0;
            App.ReadyAllData = false;
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawDataBusTag.MyGraphTabControlSelectedIndex); }));

            StartPollingBusTag(true);
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
            int i = VM_DlgDrawRawDataBusTag.MyGraphTabControlSelectedIndex;
            if (GraphCanvasMB != null)
            {
                App.myApp.Dispatcher.BeginInvoke((Action)(() =>
                {
                    myGraphMB.DrawGraph(GraphCanvasMB, i);
                }));
            }
        }

        private void btnCmdMRDG_Click(object sender, RoutedEventArgs e)
        {
            int ParamId = 67;
            byte[] ParamData = new byte[1];
//-{}            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MRDG", ParamId, ParamData, ParamData.Length, false);

        }

        private void StageLabelSetup()
        {
            //App.MRDG_RawData[0] = 0;
            switch (App.MRDG_RawData[0])
            {
                case 0:
                    StageBusTag.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageBusTag.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageBusTag.Foreground = Brushes.Red;
                    break;

            }

            switch (App.MRDG_RawData[1] & 0x3)
            {
                case 0:
                    StageInput1.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageInput1.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageInput1.Foreground = Brushes.Red;
                    break;
            }

            switch ((App.MRDG_RawData[1] >> 2) & 0x3)
            {
                case 0:
                    StageInput2.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageInput2.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageInput2.Foreground = Brushes.Red;
                    break;
            }

          switch ((App.MRDG_RawData[1] >> 4) & 0x3)
 //-?          switch ((App.MRDG_RawData[1] >> 3) & 0x3)
            {
                case 0:
                    StageInput3.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageInput3.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageInput3.Foreground = Brushes.Red;
                    break;
            }


  
            switch ((App.MRDG_RawData[1] >> 8) & 0x3)
            {
                case 0:
                    StageRelay.Foreground = Brushes.Green;
                    break;
                case 1:
                    StageRelay.Foreground = Brushes.Yellow;
                    break;
                case 2:
                    StageRelay.Foreground = Brushes.Red;
                    break;
            }
            switch ((App.MRDG_RawData[1] >> 10) & 0x3)
            {
                case 0:
                    //StageCO.Foreground = Brushes.Green;
                    break;
                case 1:
               //-{}     StageCO.Foreground = Brushes.Yellow;
                    break;
                case 2:
            //        StageCO.Foreground = Brushes.Red;
                    break;
            }
            switch ((App.MRDG_RawData[1] >> 12) & 0x3)
            {
                case 0:
         //           StageVOC.Foreground = Brushes.Green;
                    break;
                case 1:
          //          StageVOC.Foreground = Brushes.Yellow;
                    break;
                case 2:
           //         StageVOC.Foreground = Brushes.Red;
                    break;
            }
        }

        private void btnCmdDraw_Click(object sender, RoutedEventArgs e)
        {
            myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawDataBusTag.MyGraphTabControlSelectedIndex);

            //Состояние BusTag
            StageLabelSetup();


        }

        private void TabControlGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if(myGraphMB!=null)
            //    GraphBusTag.DrawAsync(VM_DlgDrawRawData.MyGraphTabControlSelectedIndex);
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timerPollingMS == null)
                StartPollingBusTag(true);
            else
                StartPollingBusTag(false);
        }
        public void StartPollingBusTag(bool start)
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
            ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MRDG", ParamId, ParamData, ParamData.Length, false);
            //GraphCanvasMB.Dispatcher.BeginInvoke((Action)(() => {  GraphBusTag.DrawAsync(VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawDataBusTag.MyGraphTabControlSelectedIndex); }));
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { StageLabelSetup(); }));
            //MyGraphMB.Draw1();
        }
        private void btnReStartDraw_Click(object sender, RoutedEventArgs e)
        {
            App.iReady = 0;
            App.ReadyAllData = false;
            App.myApp.Dispatcher.BeginInvoke((Action)(() => { myGraphMB.DrawGraph(GraphCanvasMB, VM_DlgDrawRawDataBusTag.MyGraphTabControlSelectedIndex); }));
        }

        private void btnResetStage_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[4];
            int ParamId = 70;

 //-{}           ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[1] = 24;//RegisterAddress
            ParamData[2] = 0;//Beep
            ParamData[3] = 0;//Beep
            short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
            App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);
        }
    }



    public class ViewModelDlgDrawRawDataBusTag : INotifyPropertyChanged
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









        private bool _TabBusTagInput1 = true;
        public bool TabBusTagInput1 { get { return _TabBusTagInput1; } set { _TabBusTagInput1 = value; NotifyPropertyChanged("TabBusTagInput1"); } }

        private bool _TabBusTagInput2 = true;
        public bool TabBusTagInput2 { get { return _TabBusTagInput2; } set { _TabBusTagInput2 = value; NotifyPropertyChanged("TabBusTagInput2"); } }

        private bool _TabBusTagInput3 = true;
        public bool TabBusTagInput3 { get { return _TabBusTagInput3; } set { _TabBusTagInput3 = value; NotifyPropertyChanged("TabBusTagInput3"); } }

         private bool _TabBusTagRelay = true;
        public bool TabBusTagRelay { get { return _TabBusTagRelay; } set { _TabBusTagRelay = value; NotifyPropertyChanged("TabBusTagRelay"); } }

 





        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
