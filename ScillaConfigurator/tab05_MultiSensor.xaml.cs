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
    /// Interaction logic for tab05_MultiSensor.xaml
    /// </summary>
    public partial class tab05_MultiSensor : Page
    {
        public tab05_MultiSensor()
        {
            InitializeComponent();
            DataContext = App.myApp.VMS;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.sMultiSensor.Name = App.myApp.VMS.TabMSensorName;
            App.myApp.sMultiSensor.Comment = App.myApp.VMS.TabMSensorComment;
            
            App.myApp.sMultiSensor.PositionX = App.myApp.VMS.TabMSensorPositionX;
            App.myApp.sMultiSensor.PositionY = App.myApp.VMS.TabMSensorPositionY;

            App.myApp.sMultiSensor.SlaveAddress = App.myApp.VMS.TabMSensorSlaveAddress;

            App.myApp.sMultiSensor.InclAnl_S_o = App.myApp.VMS.TabMSensor_InclAnl_S_o;
            App.myApp.sMultiSensor.InclAnl_T_a = App.myApp.VMS.TabMSensor_InclAnl_T_a;
            App.myApp.sMultiSensor.InclAnl_T_d = App.myApp.VMS.TabMSensor_InclAnl_T_d;
            App.myApp.sMultiSensor.InclAnl_S_e = App.myApp.VMS.TabMSensor_InclAnl_S_e;
            App.myApp.sMultiSensor.InclAnl_Flm = App.myApp.VMS.TabMSensor_InclAnl_Flm;
            App.myApp.sMultiSensor.InclAnl_CO = App.myApp.VMS.TabMSensor_InclAnl_CO;
            App.myApp.sMultiSensor.InclAnl_VOC = App.myApp.VMS.TabMSensor_InclAnl_VOC;

            App.myApp.sMultiSensor.ThWarnSmogO= App.myApp.VMS.TabMSensorThWarnSmogO;
            App.myApp.sMultiSensor.ThAlrmSmogO= App.myApp.VMS.TabMSensorThAlrmSmogO;
            
            App.myApp.sMultiSensor.ThWarnTempA= App.myApp.VMS.TabMSensorThWarnTempA;
            App.myApp.sMultiSensor.ThAlrmTempA= App.myApp.VMS.TabMSensorThAlrmTempA;

            App.myApp.sMultiSensor.ThWarnTempD= App.myApp.VMS.TabMSensorThWarnTempD;
            App.myApp.sMultiSensor.ThAlrmTempD= App.myApp.VMS.TabMSensorThAlrmTempD;

            App.myApp.sMultiSensor.ThWarnSmogE= App.myApp.VMS.TabMSensorThWarnSmogE;
            App.myApp.sMultiSensor.ThAlrmSmogE= App.myApp.VMS.TabMSensorThAlrmSmogE;

            App.myApp.sMultiSensor.ThWarnFlamA= App.myApp.VMS.TabMSensorThWarnFlamA;
            App.myApp.sMultiSensor.ThAlrmFlamA= App.myApp.VMS.TabMSensorThAlrmFlamA;

            App.myApp.sMultiSensor.ThWarnFlamD= App.myApp.VMS.TabMSensorThWarnFlamD;
            App.myApp.sMultiSensor.ThAlrmFlamD= App.myApp.VMS.TabMSensorThAlrmFlamD;

            App.myApp.sMultiSensor.ThWarnCO= App.myApp.VMS.TabMSensorThWarnCO;
            App.myApp.sMultiSensor.ThAlrmCO= App.myApp.VMS.TabMSensorThAlrmCO;

            App.myApp.sMultiSensor.ThWarnVOC= App.myApp.VMS.TabMSensorThWarnVOC;
            App.myApp.sMultiSensor.ThAlrmVOC= App.myApp.VMS.TabMSensorThAlrmVOC;

            App.myApp.sMultiSensor.ThSmogORefU = App.myApp.VMS.TabMSensorThSmogORefU;
            App.myApp.sMultiSensor.ThSmogO_LED = App.myApp.VMS.TabMSensorThSmogO_LED;

            App.myApp.Plan.Draw();

        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.TabMSensorName = App.myApp.sMultiSensor.Name;
            App.myApp.VMS.TabMSensorComment = App.myApp.sMultiSensor.Comment;

            App.myApp.VMS.TabMSensorPositionX = App.myApp.sMultiSensor.PositionX;
            App.myApp.VMS.TabMSensorPositionY = App.myApp.sMultiSensor.PositionY;

            App.myApp.VMS.TabMSensorSlaveAddress = App.myApp.sMultiSensor.SlaveAddress;

            App.myApp.VMS.TabMSensor_InclAnl_S_o = App.myApp.sMultiSensor.InclAnl_S_o;
            App.myApp.VMS.TabMSensor_InclAnl_T_a = App.myApp.sMultiSensor.InclAnl_T_a;
            App.myApp.VMS.TabMSensor_InclAnl_T_d = App.myApp.sMultiSensor.InclAnl_T_d;
            App.myApp.VMS.TabMSensor_InclAnl_S_e = App.myApp.sMultiSensor.InclAnl_S_e;
            App.myApp.VMS.TabMSensor_InclAnl_Flm = App.myApp.sMultiSensor.InclAnl_Flm;
            App.myApp.VMS.TabMSensor_InclAnl_CO =App.myApp.sMultiSensor.InclAnl_CO;
            App.myApp.VMS.TabMSensor_InclAnl_VOC = App.myApp.sMultiSensor.InclAnl_VOC;


            App.myApp.VMS.TabMSensorThWarnSmogO = App.myApp.sMultiSensor.ThWarnSmogO;
            App.myApp.VMS.TabMSensorThAlrmSmogO = App.myApp.sMultiSensor.ThAlrmSmogO;

            App.myApp.VMS.TabMSensorThWarnTempA = App.myApp.sMultiSensor.ThWarnTempA;
            App.myApp.VMS.TabMSensorThAlrmTempA = App.myApp.sMultiSensor.ThAlrmTempA;

            App.myApp.VMS.TabMSensorThWarnTempD = App.myApp.sMultiSensor.ThWarnTempD;
            App.myApp.VMS.TabMSensorThAlrmTempD = App.myApp.sMultiSensor.ThAlrmTempD;

            App.myApp.VMS.TabMSensorThWarnSmogE = App.myApp.sMultiSensor.ThWarnSmogE;
            App.myApp.VMS.TabMSensorThAlrmSmogE = App.myApp.sMultiSensor.ThAlrmSmogE;

            App.myApp.VMS.TabMSensorThWarnFlamA = App.myApp.sMultiSensor.ThWarnFlamA;
            App.myApp.VMS.TabMSensorThAlrmFlamA = App.myApp.sMultiSensor.ThAlrmFlamA;

            App.myApp.VMS.TabMSensorThWarnFlamD = App.myApp.sMultiSensor.ThWarnFlamD;
            App.myApp.VMS.TabMSensorThAlrmFlamD = App.myApp.sMultiSensor.ThAlrmFlamD;

            App.myApp.VMS.TabMSensorThWarnCO = App.myApp.sMultiSensor.ThWarnCO;
            App.myApp.VMS.TabMSensorThAlrmCO = App.myApp.sMultiSensor.ThAlrmCO;

            App.myApp.VMS.TabMSensorThWarnVOC = App.myApp.sMultiSensor.ThWarnVOC;
            App.myApp.VMS.TabMSensorThAlrmVOC = App.myApp.sMultiSensor.ThAlrmVOC;

            App.myApp.VMS.TabMSensorThSmogORefU = App.myApp.sMultiSensor.ThSmogORefU;
            App.myApp.VMS.TabMSensorThSmogO_LED = App.myApp.sMultiSensor.ThSmogO_LED;

        }

        private void btnBeepMultiSensor_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[4];
            int ParamId = 70;

            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
            ParamData[1] = 25;//RegisterAddres
            ParamData[2] = 0;//Beep
            ParamData[3] = 0;//Beep
            short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length,false);
        }

        private void btnGetSlaveAddress_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetSlaveAddress_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[3];
            int ParamId = 77;//????????????????????????
            ParamData[0] = 247;// (byte)App.myApp.VMM.MSAS_SlaveAddressSend;
            ParamData[1] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; // (byte)App.myApp.VMM.MSAS_SlaveAddressNewSend;
            ParamData[2] = 1;//(byte)((App.myApp.VMM.cbIsChecked_MSAS_saveFlash == true) ? 1 : 0);


            short CmdLen = 4 + 1 + 3;// CmdId(4) + ParamId(1) + ParamData(3)

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MSAS", ParamId, ParamData, ParamData.Length,false);
        }

        private void btnValidSlave_Click(object sender, RoutedEventArgs e)
        {
            _ScillaConfigurator.dlg.dlgValidSlave modalWindow = new _ScillaConfigurator.dlg.dlgValidSlave();

            modalWindow.VM_DlgValidSlave.dlgValidSlaveBuilding = App.myApp.sMultiSensor.Owner.Owner.Owner.Name;//Building
            modalWindow.VM_DlgValidSlave.dlgValidSlaveFloor = App.myApp.sMultiSensor.Owner.Owner.Name;//Floor
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModule = App.myApp.sMultiSensor.Owner.Name;//Module

            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleIp = App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_01.ToString() + "."+
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_02.ToString() + "."+
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_03.ToString() + "."+
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_04.ToString();

            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDev = App.myApp.sMultiSensor.Name;
            //modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevSlaveAdr = App.myApp.sMultiSensor.SlaveAddress.ToString();

            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevSlaveAdr = "";
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevType = "";
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevSN_First = "";
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevSN_Second = "";
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevVerMajor = "";
            modalWindow.VM_DlgValidSlave.dlgValidSlaveModuleDevVerMinor = "";


            int ParamId = 67;
            byte[] ParamData = new byte[1];
            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
            short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(),CmdLen, "ISSR", "MVSV", ParamId, ParamData, ParamData.Length,false);
            modalWindow.ShowDialog();
        }

        private void btnGetThresholds_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[40];
            int ParamId = 83;

            ThresholdsFieldsReset();

            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;
            ParamData[1] = (byte)0;//((App.myApp.VMM.MTGS_SelectedGetSetArray() == 0) ? 0 : 1); //Get or Set

            ParamData[2] = (byte)0;
            ParamData[3] = (byte)(
               ((App.myApp.VMS.TabMSensor_InclAnl_S_o == false) ? 0 : 1) +
               ((App.myApp.VMS.TabMSensor_InclAnl_T_a == false) ? 0 : 2) +
               ((App.myApp.VMS.TabMSensor_InclAnl_T_d == false) ? 0 : 4) +
               ((App.myApp.VMS.TabMSensor_InclAnl_S_e == false) ? 0 : 8) +
               ((App.myApp.VMS.TabMSensor_InclAnl_Flm == false) ? 0 : 16) +
               ((App.myApp.VMS.TabMSensor_InclAnl_CO == false) ? 0 : 32) +
               ((App.myApp.VMS.TabMSensor_InclAnl_VOC == false) ? 0 : 64));

            ParamData[4] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogO >> 8);
            ParamData[5] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogO);
            ParamData[6] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogO >> 8);
            ParamData[7] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogO);

            ParamData[8] = (byte)(App.myApp.VMS.TabMSensorThWarnTempA >> 8);
            ParamData[9] = (byte)(App.myApp.VMS.TabMSensorThWarnTempA);
            ParamData[10] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempA >> 8);
            ParamData[11] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempA);

            ParamData[12] = (byte)(App.myApp.VMS.TabMSensorThWarnTempD >> 8);
            ParamData[13] = (byte)(App.myApp.VMS.TabMSensorThWarnTempD);
            ParamData[14] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempD >> 8);
            ParamData[15] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempD);

            ParamData[16] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogE >> 8);
            ParamData[17] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogE);
            ParamData[18] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogE >> 8);
            ParamData[19] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogE);

            ParamData[20] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamA >> 8);
            ParamData[21] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamA);
            ParamData[22] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamA >> 8);
            ParamData[23] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamA);

            ParamData[24] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamD >> 8);
            ParamData[25] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamD);
            ParamData[26] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamD >> 8);
            ParamData[27] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamD);

            ParamData[28] = (byte)(App.myApp.VMS.TabMSensorThWarnCO >> 8);
            ParamData[29] = (byte)(App.myApp.VMS.TabMSensorThWarnCO);
            ParamData[30] = (byte)(App.myApp.VMS.TabMSensorThAlrmCO >> 8);
            ParamData[31] = (byte)(App.myApp.VMS.TabMSensorThAlrmCO);

            ParamData[32] = (byte)(App.myApp.VMS.TabMSensorThWarnVOC >> 8);
            ParamData[33] = (byte)(App.myApp.VMS.TabMSensorThWarnVOC);
            ParamData[34] = (byte)(App.myApp.VMS.TabMSensorThAlrmVOC >> 8);
            ParamData[35] = (byte)(App.myApp.VMS.TabMSensorThAlrmVOC);

            ParamData[36] = (byte)(App.myApp.VMS.TabMSensorThSmogORefU >> 8);
            ParamData[37] = (byte)(App.myApp.VMS.TabMSensorThSmogORefU);

            ParamData[38] = (byte)(App.myApp.VMS.TabMSensorThSmogO_LED >> 8);
            ParamData[39] = (byte)(App.myApp.VMS.TabMSensorThSmogO_LED);

            short CmdLen = 4 + 1 + 40;// CmdId(4) + ParamId(1) + ParamData(2)

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MTGS", ParamId, ParamData, ParamData.Length, false);
        }

        private void btnSetThresholds_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[40];
            int ParamId = 83;

            ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;
            ParamData[1] = (byte) 1;//((App.myApp.VMM.MTGS_SelectedGetSetArray() == 0) ? 0 : 1); //Get or Set

            ParamData[2] = (byte)0;
            ParamData[3] = (byte)(
               ((App.myApp.VMS.TabMSensor_InclAnl_S_o == false) ? 0 : 1) +
               ((App.myApp.VMS.TabMSensor_InclAnl_T_a == false) ? 0 : 2) +
               ((App.myApp.VMS.TabMSensor_InclAnl_T_d == false) ? 0 : 4) +
               ((App.myApp.VMS.TabMSensor_InclAnl_S_e == false) ? 0 : 8) +
               ((App.myApp.VMS.TabMSensor_InclAnl_Flm == false) ? 0 : 16) +
               ((App.myApp.VMS.TabMSensor_InclAnl_CO == false) ? 0 : 32) +
               ((App.myApp.VMS.TabMSensor_InclAnl_VOC == false) ? 0 : 64));

            ParamData[4] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogO >> 8);
            ParamData[5] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogO);
            ParamData[6] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogO >> 8);
            ParamData[7] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogO);

            ParamData[8] = (byte)(App.myApp.VMS.TabMSensorThWarnTempA >> 8);
            ParamData[9] = (byte)(App.myApp.VMS.TabMSensorThWarnTempA);
            ParamData[10] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempA >> 8);
            ParamData[11] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempA);

            ParamData[12] = (byte)(App.myApp.VMS.TabMSensorThWarnTempD >> 8);
            ParamData[13] = (byte)(App.myApp.VMS.TabMSensorThWarnTempD);
            ParamData[14] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempD >> 8);
            ParamData[15] = (byte)(App.myApp.VMS.TabMSensorThAlrmTempD);

            ParamData[16] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogE >> 8);
            ParamData[17] = (byte)(App.myApp.VMS.TabMSensorThWarnSmogE);
            ParamData[18] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogE >> 8);
            ParamData[19] = (byte)(App.myApp.VMS.TabMSensorThAlrmSmogE);

            ParamData[20] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamA >> 8);
            ParamData[21] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamA);
            ParamData[22] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamA >> 8);
            ParamData[23] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamA);

            ParamData[24] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamD >> 8);
            ParamData[25] = (byte)(App.myApp.VMS.TabMSensorThWarnFlamD);
            ParamData[26] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamD >> 8);
            ParamData[27] = (byte)(App.myApp.VMS.TabMSensorThAlrmFlamD);

            ParamData[28] = (byte)(App.myApp.VMS.TabMSensorThWarnCO >> 8);
            ParamData[29] = (byte)(App.myApp.VMS.TabMSensorThWarnCO);
            ParamData[30] = (byte)(App.myApp.VMS.TabMSensorThAlrmCO >> 8);
            ParamData[31] = (byte)(App.myApp.VMS.TabMSensorThAlrmCO);

            ParamData[32] = (byte)(App.myApp.VMS.TabMSensorThWarnVOC >> 8);
            ParamData[33] = (byte)(App.myApp.VMS.TabMSensorThWarnVOC);
            ParamData[34] = (byte)(App.myApp.VMS.TabMSensorThAlrmVOC >> 8);
            ParamData[35] = (byte)(App.myApp.VMS.TabMSensorThAlrmVOC);

            ParamData[36] = (byte)(App.myApp.VMS.TabMSensorThSmogORefU >> 8);
            ParamData[37] = (byte)(App.myApp.VMS.TabMSensorThSmogORefU);

            ParamData[38] = (byte)(App.myApp.VMS.TabMSensorThSmogO_LED >> 8);
            ParamData[39] = (byte)(App.myApp.VMS.TabMSensorThSmogO_LED);

            short CmdLen = 4 + 1 + 40;// CmdId(4) + ParamId(1) + ParamData(2)

            ThresholdsFieldsReset();

            App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MTGS", ParamId, ParamData, ParamData.Length,false);
        }

        private void ThresholdsFieldsReset()
        {
            App.myApp.VMS.TabMSensor_InclAnl_S_o = false;
            App.myApp.VMS.TabMSensorThWarnSmogO = 0;
            App.myApp.VMS.TabMSensorThAlrmSmogO = 0;

            App.myApp.VMS.TabMSensor_InclAnl_T_a = false;
            App.myApp.VMS.TabMSensorThWarnTempA = 0;
            App.myApp.VMS.TabMSensorThAlrmTempA = 0;

            App.myApp.VMS.TabMSensor_InclAnl_T_d = false;
            App.myApp.VMS.TabMSensorThWarnTempD = 0;
            App.myApp.VMS.TabMSensorThAlrmTempD = 0;

            App.myApp.VMS.TabMSensor_InclAnl_S_e = false;
            App.myApp.VMS.TabMSensorThWarnSmogE = 0;
            App.myApp.VMS.TabMSensorThAlrmSmogE = 0;

            App.myApp.VMS.TabMSensor_InclAnl_Flm = false;
            App.myApp.VMS.TabMSensorThWarnFlamA = 0;
            App.myApp.VMS.TabMSensorThAlrmFlamA = 0;
            App.myApp.VMS.TabMSensorThWarnFlamD = 0;
            App.myApp.VMS.TabMSensorThAlrmFlamD = 0;

            App.myApp.VMS.TabMSensor_InclAnl_CO = false;
            App.myApp.VMS.TabMSensorThWarnCO = 0;
            App.myApp.VMS.TabMSensorThAlrmCO = 0;

            App.myApp.VMS.TabMSensor_InclAnl_VOC = false;
            App.myApp.VMS.TabMSensorThWarnVOC = 0;
            App.myApp.VMS.TabMSensorThAlrmVOC = 0;

            App.myApp.VMS.TabMSensorThSmogORefU = 0;
            App.myApp.VMS.TabMSensorThSmogO_LED = 0;
        }

        private void bntReadRawData_Click(object sender, RoutedEventArgs e)
        {
            _ScillaConfigurator.dlg.dlgDrawRawData modalWindow = new _ScillaConfigurator.dlg.dlgDrawRawData();
            
            modalWindow.VM_DlgDrawRawData.dlgValidSlaveBuilding = App.myApp.sMultiSensor.Owner.Owner.Owner.Name;//Building
            modalWindow.VM_DlgDrawRawData.dlgValidSlaveFloor = App.myApp.sMultiSensor.Owner.Owner.Name;//Floor
            modalWindow.VM_DlgDrawRawData.dlgValidSlaveModule = App.myApp.sMultiSensor.Owner.Name;//Module

            modalWindow.VM_DlgDrawRawData.dlgValidSlaveModuleIp = App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_01.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_02.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_03.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_04.ToString();

            modalWindow.VM_DlgDrawRawData.dlgValidSlaveModuleDev = App.myApp.sMultiSensor.Name;
            modalWindow.VM_DlgDrawRawData.dlgValidSlaveModuleDevSlaveAdr = App.myApp.sMultiSensor.SlaveAddress.ToString();

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
            
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_S_o = App.myApp.VMS.TabMSensor_InclAnl_S_o;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_T_a = App.myApp.VMS.TabMSensor_InclAnl_T_a;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_T_d = App.myApp.VMS.TabMSensor_InclAnl_T_d;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_S_e = App.myApp.VMS.TabMSensor_InclAnl_S_e;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_Flm = App.myApp.VMS.TabMSensor_InclAnl_Flm;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_CO = App.myApp.VMS.TabMSensor_InclAnl_CO;
            modalWindow.VM_DlgDrawRawData.TabMSensor_InclAnl_VOC = App.myApp.VMS.TabMSensor_InclAnl_VOC;

            //modalWindow.VM_DlgDrawRawData.dlgValidSlaveModuleDevSlaveAdr = "";
            
            modalWindow.ShowDialog();
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

        private void btnCalibration_Click(object sender, RoutedEventArgs e)
        {

            _ScillaConfigurator.dlg.dlgCalibrate modalWindow = new _ScillaConfigurator.dlg.dlgCalibrate();

            modalWindow.VM_DlgCalibrate.dlgValidSlaveBuilding = App.myApp.sMultiSensor.Owner.Owner.Owner.Name;//Building
            modalWindow.VM_DlgCalibrate.dlgValidSlaveFloor = App.myApp.sMultiSensor.Owner.Owner.Name;//Floor
            modalWindow.VM_DlgCalibrate.dlgValidSlaveModule = App.myApp.sMultiSensor.Owner.Name;//Module

            modalWindow.VM_DlgCalibrate.dlgValidSlaveModuleIp = App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_01.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_02.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_03.ToString() + "." +
                App.myApp.sMultiSensor.Owner.DESS_ModuleIpAddress_04.ToString();

            modalWindow.VM_DlgCalibrate.dlgValidSlaveModuleDev = App.myApp.sMultiSensor.Name;
            modalWindow.VM_DlgCalibrate.dlgValidSlaveModuleDevSlaveAdr = App.myApp.sMultiSensor.SlaveAddress.ToString();
            /*
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
            */
            //modalWindow.VM_DlgDrawRawData.dlgValidSlaveModuleDevSlaveAdr = "";

            if (modalWindow.ShowDialog() == true)
            {
                modalWindow.VM_DlgCalibrate.SelectedCalibrTypeArray(); //SelectedCalibrTypeArray();

                byte[] ParamData = new byte[4];
                int ParamId = 70;

                ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress;//(byte)App.myApp.VMM.MHRS_SlaveAddressSend;
                ParamData[1] = 25;//RegisterAddres
                ParamData[2] = 0; //Beep
                ParamData[3] = (byte)(modalWindow.VM_DlgCalibrate.SelectedCalibrTypeArray() + 1);//Beep
                short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(4)
                App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MHRS", ParamId, ParamData, ParamData.Length, false);
            }

        }
    }
}
