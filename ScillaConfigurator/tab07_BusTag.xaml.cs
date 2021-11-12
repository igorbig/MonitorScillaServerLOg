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
    /// Interaction logic for tab07_BusTag.xaml
    /// </summary>
    public partial class tab07_BusTag : Page
    {
     ///////////Состояние реле////////////////
       static bool btnSetResetBusTagRelayState = false;
     ////////////////////////////////
        
        public tab07_BusTag()
        {
            InitializeComponent();
            DataContext = App.myApp.VMS;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.sBusTag.Name = App.myApp.VMS.TabBusTagName;
            App.myApp.sBusTag.Comment = App.myApp.VMS.TabBusTagComment;

            App.myApp.sBusTag.PositionX = App.myApp.VMS.TabBusTagPositionX;
            App.myApp.sBusTag.PositionY = App.myApp.VMS.TabBusTagPositionY;

            App.myApp.sBusTag.SlaveAddress = App.myApp.VMS.TabBusTagSlaveAddress;
            //+{
            App.myApp.sBusTag.Input1 = App.myApp.VMS.TabBusTagInput1;
            App.myApp.sBusTag.Input2 = App.myApp.VMS.TabBusTagInput2;
            App.myApp.sBusTag.Input3 = App.myApp.VMS.TabBusTagInput3;

            App.myApp.sBusTag.Relay = App.myApp.VMS.TabBusTagRelay;
            //}


            App.myApp.Plan.Draw();
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.TabBusTagName = App.myApp.sBusTag.Name;
            App.myApp.VMS.TabBusTagComment = App.myApp.sBusTag.Comment;

            App.myApp.VMS.TabBusTagPositionX = App.myApp.sBusTag.PositionX;
            App.myApp.VMS.TabBusTagPositionY = App.myApp.sBusTag.PositionY;

            App.myApp.VMS.TabBusTagSlaveAddress = App.myApp.sBusTag.SlaveAddress;
        }

        private void btnBeepBusTag_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[4];
            int ParamId = 70;

//-"{}            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[1] = 25;//RegisterAddres
            ParamData[2] = 0;//Beep
            ParamData[3] = 0;//Beep
            short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
            App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);

        }

        private void btnGetStage_Click(object sender, RoutedEventArgs e)
        {

        }

//-{err        Серьезность Контекст данных Путь привязки Целевой объект Конечный тип Описание    Файл Строка  Проект
//Ошибка  ViewModelCnf BusTagRelay Button.IsEnabled, имя — "btnSetResetBusTagRelay"	Boolean Свойство BusTagRelay не найдено для объекта типа ViewModelCnf.
       private void btnSetResetBusTagRelay_Click(object sender, RoutedEventArgs e)
        {
              byte[] ParamData = new byte[4];
                int ParamId = 70;
          if (btnSetResetBusTagRelayState) { 
            btnSetResetBusTagRelay.Content = "Включить РЕЛЕ";
    
                //-{}           ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
                ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
                ParamData[1] = 14;//RegisterAddres
                ParamData[2] = 255;//Beep
                ParamData[3] = 255;//Beep
                short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
                App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);
                btnSetResetBusTagRelayState = false;
            }
            else {
                btnSetResetBusTagRelay.Content = "Выключить РЕЛЕ";
                ParamData[0] = (byte)App.myApp.VMS.TabBusTagSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
                ParamData[1] = 14;//RegisterAddres
                ParamData[2] = 0;//Beep
                ParamData[3] = 0;//Beep
                short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
                App.myApp.SendCmd(App.myApp.sBusTag.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);
                btnSetResetBusTagRelayState = true;
            }
        }

        private void bntReadRawDataBusTag_Click(object sender, RoutedEventArgs e)
        {
            _ScillaConfigurator.dlgDrawRawDataBusTag modalWindow = new _ScillaConfigurator.dlgDrawRawDataBusTag();

            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveBuilding = App.myApp.sBusTag.Owner.Owner.Owner.Name;//Building
            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveFloor = App.myApp.sBusTag.Owner.Owner.Name;//Floor
            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveModule = App.myApp.sBusTag.Owner.Name;//Module

            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveModuleIp = App.myApp.sBusTag.Owner.DESS_ModuleIpAddress_01.ToString() + "." +
                App.myApp.sBusTag.Owner.DESS_ModuleIpAddress_02.ToString() + "." +
                App.myApp.sBusTag.Owner.DESS_ModuleIpAddress_03.ToString() + "." +
                App.myApp.sBusTag.Owner.DESS_ModuleIpAddress_04.ToString();

            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveModuleDev = App.myApp.sBusTag.Name;
            modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveModuleDevSlaveAdr = App.myApp.sBusTag.SlaveAddress.ToString();

  //-{}          MyGraphMS.DataSmogTr[0] = App.myApp.VMS.TabMSensorThWarnSmogO;
            MyGraphMS.DataSmogTr[0] = App.myApp.VMS.TabMSensorThWarnSmogO;
            MyGraphMS.DataSmogTr[1] = App.myApp.VMS.TabMSensorThAlrmSmogO;

            MyGraphMS.DataTempTr[0] = App.myApp.VMS.TabMSensorThWarnTempA;
            MyGraphMS.DataTempTr[1] = App.myApp.VMS.TabMSensorThAlrmTempA;

            MyGraphMS.DataTempDigTr[0] = App.myApp.VMS.TabMSensorThWarnTempD;
            MyGraphMS.DataTempDigTr[1] = App.myApp.VMS.TabMSensorThAlrmTempD;

            MyGraphMS.DataSmogETr[0] = App.myApp.VMS.TabMSensorThWarnSmogE;
            MyGraphMS.DataSmogETr[1] = App.myApp.VMS.TabMSensorThAlrmSmogE;

            MyGraphMS.DataFlameTr1[0] = App.myApp.VMS.TabMSensorThWarnFlamA;
            MyGraphMS.DataFlameTr1[1] = App.myApp.VMS.TabMSensorThAlrmFlamA;
            MyGraphMS.DataFlameTr2[0] = App.myApp.VMS.TabMSensorThWarnFlamD;
            MyGraphMS.DataFlameTr2[1] = App.myApp.VMS.TabMSensorThAlrmFlamD;

            MyGraphMS.DataCOTr[0] = App.myApp.VMS.TabMSensorThWarnCO;
            MyGraphMS.DataCOTr[1] = App.myApp.VMS.TabMSensorThAlrmCO;

            modalWindow.VM_DlgDrawRawDataBusTag.TabBusTagInput1 = App.myApp.VMS.TabBusTagInput1;
            modalWindow.VM_DlgDrawRawDataBusTag.TabBusTagInput2 = App.myApp.VMS.TabBusTagInput2;
            modalWindow.VM_DlgDrawRawDataBusTag.TabBusTagInput3 = App.myApp.VMS.TabBusTagInput3;
  //-{}           modalWindow.VM_DlgDrawRawDataBusTag.TabBusTagRelay = App.myApp.VMS.TabBusTagRelay;
      
            //modalWindow.VM_DlgDrawRawDataBusTag.dlgValidSlaveModuleDevSlaveAdr = "";

            modalWindow.ShowDialog();

        }
    }
}
