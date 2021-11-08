using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _ScillaConfigurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static App myApp;

        public ViewModelCnf VMS;
        public VM_CnfTab01_Object VMCnfTab01;
        public VM_CnfTab02_Building VMCnfTab02;
        public VM_CnfTab03_Floor VMCnfTab03;
        //public VM_CnfTab04_Module VMCnfTab04;
        //public VM_CnfTab05_MultiSensor VMCnfTab05;
        //public VM_CnfTab06_RelayUnit VMCnfTab06;
        //public VM_CnfTab07_BusTag VMCnfTab07;

        //

        static public Canvas GraphCanvas = null;
        static public TextBlock TextBlockFourZero = null;


        public _ScillaConfigurator.dlg.ViewModelDlgValidSlave VMDlgValidSlave;
        public _ScillaConfigurator.dlg.ViewModelDlgDrawRawData VMDlgDrawRawData;
        public _ScillaConfigurator.ViewModelDlgDrawRawDataBusTag VMDlgDrawRawDataBusTag;//{+}

        public UdpClient udpServerListner=null;
        public int serverListenerPort=2424;
        public int moduleListenerPort=2323;

        //public Task taskUDP_Listner = null;
        //bool stopListner = false;

        //static byte[] ResivedBytes = new byte[2048];
        //static int iResivedBytes = 0;
        //static int iProcessedBytes = 0;
        //static bool FlagResivedBytesFull = false;
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>
        static public int iReady = 0;
        static public bool ReadyAllData = false;

        public static int[,] DataSmog = new int[3, 100];
        public static int[] DataSmogTr = new int[2];

        public static int[] DataTemp = new int[100];
        public static int[] DataTempTr = new int[2];

        public static int[] DataTempDig = new int[100];
        public static int[] DataTempDigTr = new int[2];

        public static int[] DataSmogE = new int[100];
        public static int[] DataSmogETr = new int[2];

        public static int[,] DataFlame = new int[8, 100];
        public static int[] DataFlameSTD = new int[100];
        public static int[] DataFlameTr1 = new int[2];
        public static int[] DataFlameTr2 = new int[2];

        public static int[] DataCO = new int[100];
        public static int[] DataCOTr = new int[2];

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public static byte[] ResivedBytes = new byte[2048];
        public static int iResivedBytes = 0;
        public static bool FlagResivedBytesFull = false;
        static int iProcessedBytes = 0;

        public PlanArea Plan = null;
        public ScrollViewer ScrollViewerCan;
        public RulerX_Area RulerX = null;
        public RulerY_Area RulerY = null;

        //Active on TreeView
        public ScillaObject sScillaObject = null;
        public Building sBuilding = null;
        public Floor sFloor = null;
        public Module sModule = null;

        public MultiSensor sMultiSensor = null;
        public RelayUnit sRelayUnit = null;
        public BusTag sBusTag = null;

        //Active on CanvasPlanArea
        public int? iLightModule = null;
        public int? iLightMultiSensor = null;

        //
        public int? iDragModule = null;
        public int? iDragMultiSensor = null;

        public App()
        {
            myApp = this;
            VMS=new ViewModelCnf();
            VMCnfTab01 = new VM_CnfTab01_Object();
            VMCnfTab02 = new VM_CnfTab02_Building();
            VMCnfTab03 = new VM_CnfTab03_Floor();
            //VMCnfTab04 = new VM_CnfTab04_Module();
            //VMCnfTab05 = new VM_CnfTab05_MultiSensor();
            //VMCnfTab06 = new VM_CnfTab06_RelayUnit();
            //VMCnfTab07 = new VM_CnfTab07_BusTag();
        }


        public void SendCmd(UInt32 IpAddress, short CmdLen, String STX, String CMD, int ParamID, byte[] ParamData, int nButes, bool Broadcast)
        {

            int len = 0;

            byte[] sendbuf = new byte[512];
            byte[] buf01 = new byte[256];

            //Преамбула 4 байта
            buf01 = Encoding.ASCII.GetBytes(STX);
            for (int i = 0; i < 4; i++)
            {
                sendbuf[i] = buf01[i];
            }
            len += 4;

            //Номер пакета Считаем сами
            short NMessage = 12;
            sendbuf[len] = (byte)(NMessage >> 8);
            len++;
            sendbuf[len] = (byte)NMessage;
            len++;

            //CMD_LEN Длинна мкоманды
            //short CmdLen = 4 + 1 + 1;// CmdId + ParamId + ParamData
            sendbuf[len] = (byte)(CmdLen >> 8);
            len++;
            sendbuf[len] = (byte)CmdLen;
            len++;

            //ID команды
            buf01 = Encoding.ASCII.GetBytes(CMD);
            for (int i = 0; i < 4; i++)
            {
                sendbuf[len] = buf01[i];
                len++;
            }

            //ID параметра
            if (CMD == "INFO" || CMD == "DESG" || CMD == "DESS")
                ;
            else
            {
                sendbuf[len] = (byte)ParamID;
                len++;
            }

            //Данные параметра
            for (int i = 0; i < nButes; i++)
            {
                sendbuf[len] = ParamData[i];//(byte)((App.myApp.VMM.Relay19 == true) ? 0x01 : 0x00); ;
                len++;
            }

            /// Crc16 
            sendbuf[len] = 0xAB;
            len++;
            sendbuf[len] = 0xCD;
            len++;

            UdpClient UDPsender = new UdpClient();

            // Создаем endPoint по информации об удаленном хосте
            //string strAddress = DESS_ModuleIpAddress_01.ToString() + "." + DESS_ModuleIpAddress_02.ToString() + "." + DESS_ModuleIpAddress_03.ToString() + "." + DESS_ModuleIpAddress_04.ToString();
            string strAddress = ((IpAddress>>24) & 0xFF).ToString() + "." + ((IpAddress >> 16) & 0xFF).ToString() + "." + ((IpAddress >> 8) & 0xFF).ToString() + "." + (IpAddress & 0xFF).ToString();

            

            IPAddress remoteIPAddress;
            if (Broadcast == true)
                remoteIPAddress = IPAddress.Parse("192.168.1.255");//IPAddress.Broadcast;// IPAddress.Parse(strAddress);
            //https://ru.wikipedia.org/wiki/%D0%A8%D0%B8%D1%80%D0%BE%D0%BA%D0%BE%D0%B2%D0%B5%D1%89%D0%B0%D1%82%D0%B5%D0%BB%D1%8C%D0%BD%D1%8B%D0%B9_%D0%B0%D0%B4%D1%80%D0%B5%D1%81
            else
                remoteIPAddress = IPAddress.Parse(strAddress);

            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, App.myApp.moduleListenerPort);//2323

            try
            {
                // Преобразуем данные в массив байтов
                //byte[] bytes = Encoding.UTF8.GetBytes(datagram);

                // Отправляем данные
                UDPsender.Send(sendbuf, len, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                UDPsender.Close();
            }

            //FillTextBoxEthernetSendedCmd(sendbuf, len);
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {

            UdpClient udpServerListner = ar.AsyncState as UdpClient;
            IPEndPoint IpEndPointServerListenerPort = new IPEndPoint(IPAddress.Any, App.myApp.serverListenerPort);

            try
            {
                byte[] bytes = udpServerListner.EndReceive(ar, ref IpEndPointServerListenerPort);
            
                for (int i = 0; i < bytes.Length; i++)
                {
                    ResivedBytes[iResivedBytes] = bytes[i];
                    iResivedBytes++;
                    if (iResivedBytes == ResivedBytes.Length)
                    {
                        iResivedBytes = 0;
                        FlagResivedBytesFull = true;
                    }
                }
                Recive();//module
                udpServerListner.BeginReceive(new AsyncCallback(ReceiveCallback), udpServerListner);//udpState
            }
            catch (Exception e)
            {
                ;
            }
        }
        static int stage = 0;
        static int stageCmd = 0;

        static int PackageNumber = 0;
        static int CmdLen = 0;
        static byte[] Cmd = new byte[4];
        static string strCMD;
        //static int CmdId;
        static byte ParamId;

        static byte TmpByte;
        static int Tmp;
        static UInt32 TmpIpAddress;
        static int TmpNumberSlaveAddress;
        static UInt16[] TmpValidSlaves = new UInt16[248]; //Адреса на шине MODBUS, которые опрашивает модуль
        static UInt32[] MASG_RawData = new UInt32[248];
        static public UInt16[] MRDG_RawData = new UInt16[18];
        static byte[] INFO_RawData = new byte[4+2]; //Пока используется только IP адрес и первые 2 байта из параметров ответа: состояние кнопки Fire, Security 
        public static void Recive(/*Module module*/)
        {


            while (iResivedBytes != iProcessedBytes)
            {
                switch (stage)
                {
                    case 0: //Преамбула byte 1
                        stage = (ResivedBytes[iProcessedBytes] == 0x49) ? 1 : 0;
                        break;
                    case 1://Преамбула byte 2
                        stage = (ResivedBytes[iProcessedBytes] == 0x53) ? 2 : 0;
                        break;
                    case 2://Преамбула byte 3
                        stage = (ResivedBytes[iProcessedBytes] == 0x53) ? 3 : 0;
                        break;
                    case 3://Преамбула byte 4
                        stage = (ResivedBytes[iProcessedBytes] == 0x41) ? 4 : 0;
                        break;
                    case 4://номер пакета byte 1
                        PackageNumber = ResivedBytes[iProcessedBytes] << 8;
                        stage++;
                        break;
                    case 5://номер пакета byte 2
                        PackageNumber += ResivedBytes[iProcessedBytes];
                        stage++;
                        break;
                    case 6://Длинна команды byte 1
                        CmdLen = ResivedBytes[iProcessedBytes] << 8;
                        stage++;
                        break;
                    case 7://Длинна команды byte 2
                        CmdLen += ResivedBytes[iProcessedBytes];
                        stage++;
                        break;
                    case 8://ID команды byte 1
                        Cmd[0] = ResivedBytes[iProcessedBytes];
                        stage++;
                        CmdLen--;
                        break;
                    case 9://ID команды byte 2
                        Cmd[1] = ResivedBytes[iProcessedBytes];
                        stage++;
                        CmdLen--;
                        break;
                    case 10://ID команды byte 3
                        Cmd[2] = ResivedBytes[iProcessedBytes];
                        stage++;
                        CmdLen--;
                        break;
                    case 11://ID команды byte 4
                        Cmd[3] = ResivedBytes[iProcessedBytes];
                        stage++;
                        CmdLen--;
                        stageCmd = 0;
                        strCMD = System.Text.Encoding.Default.GetString(Cmd);
                        /* "INFO"-0,"MVSV","MVSG","MVSS","MVSC","MASG","MRDG","MIRG","MHRG","MHRS","MUSS","MHMG","MHMS","MSAS","MASB","MMSS","DESS"*/
                        break;
                    
                    case 12:
                        switch (strCMD)
                        {
                            case "DESG"://DESG
                            case "DESS"://DESG
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 1 - IP address контроллера
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 1:
                                        App.myApp.VMS.TabModuleIpAddress_01 = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 2:
                                        App.myApp.VMS.TabModuleIpAddress_02 = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 3:
                                        App.myApp.VMS.TabModuleIpAddress_03 = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 4:
                                        App.myApp.VMS.TabModuleIpAddress_04 = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;

                                    case 5:// ID Параметра 2 - MAC address контроллера
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 6:
                                        App.myApp.VMS.TabModuleMacAddress_01 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 7:
                                        App.myApp.VMS.TabModuleMacAddress_02 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 8:
                                        App.myApp.VMS.TabModuleMacAddress_03 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 9:
                                        App.myApp.VMS.TabModuleMacAddress_04 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 10:
                                        App.myApp.VMS.TabModuleMacAddress_05 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 11:
                                        App.myApp.VMS.TabModuleMacAddress_06 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;

                                    case 12:// ID Параметра 3 - порт контроллера
                                        ParamId = ResivedBytes[iProcessedBytes]; //stageCmd++; CmdLen--;
                                        break;
                                    case 13:
                                        App.myApp.VMS.TabModulePort = ((int)ResivedBytes[iProcessedBytes])<<8; //stageCmd++; CmdLen--;
                                        break;
                                    case 14:
                                        App.myApp.VMS.TabModulePort += ResivedBytes[iProcessedBytes]; //stageCmd++; CmdLen--;
                                        break;


                                    case 15:// ID Параметра 4 - IP address сервера
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 16:
                                        App.myApp.VMS.TabModuleServerIpAddress_01 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 17:
                                        App.myApp.VMS.TabModuleServerIpAddress_02 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 18:
                                        App.myApp.VMS.TabModuleServerIpAddress_03 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 19:
                                        App.myApp.VMS.TabModuleServerIpAddress_04 = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;


                                    case 20:// ID Параметра 5 - порт сервера
                                        ParamId = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;
                                    case 21:
                                        App.myApp.VMS.TabModuleServerPort = ((int)ResivedBytes[iProcessedBytes]) << 8;// stageCmd++; CmdLen--;
                                        break;
                                    case 22:
                                        App.myApp.VMS.TabModuleServerPort += ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;

                                    case 23:// ID Параметра 6 - тел 12 байтов
                                        ParamId = ResivedBytes[iProcessedBytes]; //stageCmd++; CmdLen--;
                                        break;

                                    case 24:
                                        App.myApp.VMS.TabModuleTel = ResivedBytes[iProcessedBytes].ToString(); CmdLen--; iProcessedBytes++;
                                        for (int i = 0; i < 10; i++)
                                        {
                                            App.myApp.VMS.TabModuleTel += ResivedBytes[iProcessedBytes].ToString();  CmdLen--; iProcessedBytes++;

                                        }
                                        App.myApp.VMS.TabModuleTel += ResivedBytes[iProcessedBytes].ToString(); //CmdLen--; stageCmd++;
                                        break;
                                    
                                    case 25:// ID Параметра 7 - разрешение на использование телефона 1 байт
                                        ParamId = ResivedBytes[iProcessedBytes];// stageCmd++; CmdLen--;
                                        break;

                                    case 26:// разрешение на использование телефона 1 байт
                                        App.myApp.VMS.TabModuleTelAvailable = (ResivedBytes[iProcessedBytes] == 0) ? false : true; //stageCmd++; CmdLen--;
                                        break;

                                    case 27:// ID Параметра 8 - Контрольная сумма дескриптора 1 байт
                                        ParamId = ResivedBytes[iProcessedBytes]; //stageCmd++; CmdLen--;
                                        break;

                                    case 28:// ID Параметра 8 - Контрольная сумма дескриптора 1 байт
                                        ParamId = ResivedBytes[iProcessedBytes]; //stageCmd++; CmdLen--;

                                        stage ++;// CmdLen = 1
                                        break;
                                }
                                stageCmd++; CmdLen--;
                                break;
                            case "MASG"://MASG MultiSensor Array Stage Get
                                switch (stageCmd)
                                {
                                    case 0: // ID Параметра
                                        ParamId = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 1: //IP address от кого получен ответ
                                        TmpIpAddress = ((UInt32)ResivedBytes[iProcessedBytes]) << 24; //CmdLen--;stageCmd++;
                                        break;
                                    case 2:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 16; //CmdLen--; stageCmd++;
                                        break;
                                    case 3:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 8; //CmdLen--; stageCmd++; 
                                        break;
                                    case 4:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]); //CmdLen--; stageCmd++;
                                        break;

                                    case 5://Число подчиненных устройств в банке пока всегда 247
                                        int n = ResivedBytes[iProcessedBytes];//CmdLen--;stageCmd++;
                                        break;
                                    case 6:
                                        for (int i = 0; i < 247; i++)
                                        {
                                            TmpByte = ResivedBytes[iProcessedBytes];//slave address
                                            MASG_RawData[i] = ResivedBytes[iProcessedBytes];
                                            CmdLen--;
                                            iProcessedBytes++;
                                            if (iProcessedBytes == ResivedBytes.Length)
                                                iProcessedBytes = 0;

                                            TmpByte = ResivedBytes[iProcessedBytes];
                                            MASG_RawData[i] += ((UInt32)ResivedBytes[iProcessedBytes]) << 8;//младшая часть регистра 6 - состояние мультисенсора
                                            CmdLen--;
                                            iProcessedBytes++;
                                            if (iProcessedBytes == ResivedBytes.Length)
                                                iProcessedBytes = 0;

                                            TmpByte = ResivedBytes[iProcessedBytes];
                                            MASG_RawData[i] += ((UInt32)ResivedBytes[iProcessedBytes]) << 16;//младшая часть регистра 7 
                                            CmdLen--;
                                            iProcessedBytes++;
                                            if (iProcessedBytes == ResivedBytes.Length)
                                                iProcessedBytes = 0;

                                            TmpByte = ResivedBytes[iProcessedBytes];
                                            MASG_RawData[i] += ((UInt32)(ResivedBytes[iProcessedBytes]) << 24);//старшая часть регистра 7 
                                           
                                            if (i != (247 - 1))
                                            {
                                                CmdLen--;
                                                iProcessedBytes++;
                                                if (iProcessedBytes == ResivedBytes.Length)
                                                    iProcessedBytes = 0;
                                            }
                                        }


                                        Module module;// = App.myApp.sModule;//sScillaObject.Buildings[0].Floors[0].Modules[0];
                                        UInt16 TempV;
                                        for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
                                            for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                                                for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                                                {
                                                    if (
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_01) << 24) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_02) << 16) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_03) << 8) +
                                                        (UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_04 == TmpIpAddress
                                                            )
                                                    //Для модуля IP адрес, которого совпадает с отправителем полученной датаграммы заполняем состояние мультидатчиков
                                                    {
                                                        module = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m];
                                                        if (module.sended_MASG_and_INFO_count >= 3)
                                                        {
                                                            string str00 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Name; //module.Owner.Owner.Name; ;
                                                            string str01 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Name;//module.Owner.Name;
                                                            string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                                            string str03 = "All";
                                                            string str04 = "Connection recovered";

                                                            App.myApp.Dispatcher.BeginInvoke((Action)(() =>
                                                            {
                                                                App.myApp.VMS.ScillaEvents.Insert(0,new ScillaEvent(DateTime.Now,
                                                                                                                str00,//"Строение 1А (Конференц зал)",
                                                                                                                str01,//"Технический этаж",
                                                                                                                str02,//module.Name+"IP:192.168.001.201 Module001", 
                                                                                                                str03,//"Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", "+ module.ModuleDevs[i].Name, //+"Addr:001 MultiSensor 001 ",
                                                                                                                str04,
                                                                                                                "This is comment,"));
                                                            }));
                                                        }
                                                        module.sended_MASG_and_INFO_count = 0;
                                                        module.moduleConnectionOk = true;

                                                        for (int i = 0; i < module.ModuleDevs.Count; i++)
                                                        {
                                                            if (module.ModuleDevs[i].ModuleDevType == 1) //MultiSensor
                                                            {
                                                                ((MultiSensor)module.ModuleDevs[i]).InputReg[6] = (UInt16)((MASG_RawData[module.ModuleDevs[i].SlaveAddress - 1] >> 8) & 0xff);
                                                                ((MultiSensor)module.ModuleDevs[i]).InputReg[7] = (UInt16)(MASG_RawData[module.ModuleDevs[i].SlaveAddress - 1] >> 16);
                                                                TempV = ((MultiSensor)module.ModuleDevs[i]).InputReg[7];
                                                                //TempV = (UInt16)(((MultiSensor)module.ModuleDevs[i]).InputReg[7]>>8);
                                                                // Отобразим состояние датчика


                                                                MdState newModuleDevState = MdState.MS_NotSet;
                                                                //Если датчик не включен в массив ValidSlave[] опроса Модуля
                                                                if (((((MultiSensor)module.ModuleDevs[i]).InputReg[7]) & 0x20) == 0x20)
                                                                    newModuleDevState = MdState.MS_NotPolled;
                                                                else
                                                                    {
                                                                    //Если нет ошибок
                                                                    if (((((MultiSensor)module.ModuleDevs[i]).InputReg[7]) & 0xC0) == 0)
                                                                        switch (((MultiSensor)module.ModuleDevs[i]).InputReg[6])
                                                                        {
                                                                            case 0://Norma
                                                                                newModuleDevState = MdState.MS_Ok;
                                                                                break;
                                                                            case 1://Warning
                                                                                newModuleDevState = MdState.MS_Warning;
                                                                                break;
                                                                            case 2://Alarm
                                                                                newModuleDevState = MdState.MS_Alarm;
                                                                                break;
                                                                        }
                                                                    //если есть ошибки
                                                                    else
                                                                    {
                                                                        newModuleDevState = MdState.MS_ConnectError;
                                                                        module.ModuleDevs[i].moduleDevErrorCount++;
                                                                    }
                                                                    }

                                                                if (module.ModuleDevs[i].moduleDevState != newModuleDevState)
                                                                {
                                                                    string str00 = module.Owner.Owner.Name; ;
                                                                    string str01 = module.Owner.Name;
                                                                    string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                                                    string str03 = "Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", " + module.ModuleDevs[i].Name;
                                                                    string str04 ="";
                                                                    switch (newModuleDevState)
                                                                    {
                                                                        case MdState.MS_NotSet:
                                                                            str04 = "not Defined";
                                                                            break;
                                                                        case MdState.MS_Ok:
                                                                            str04 = "Ok";
                                                                            break;
                                                                        case MdState.MS_Warning:
                                                                            str04 = "Warning";
                                                                            break;
                                                                        case MdState.MS_Alarm:
                                                                            str04 = "Alarm";
                                                                            break;
                                                                        case MdState.MS_ConnectError:
                                                                            str04 = "Error";
                                                                            break;
                                                                        case MdState.MS_NotPolled:
                                                                            str04 = "not Polled";
                                                                            break;
                                                                    }
                                                                    //GraphCanvasMB
                                                                   
                                                                    App.myApp.Dispatcher.BeginInvoke((Action)(() => {
                                                                        App.myApp.VMS.ScillaEvents.Insert(0,new ScillaEvent(DateTime.Now,
                                                                            str00,//"Строение 1А (Конференц зал)",
                                                                            str01,//"Технический этаж",
                                                                            str02,//module.Name+"IP:192.168.001.201 Module001", 
                                                                            str03,//"Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", "+ module.ModuleDevs[i].Name, //+"Addr:001 MultiSensor 001 ",
                                                                            str04,
                                                                            "This is comment,"));
                                                                    }));
                                                                    module.ModuleDevs[i].moduleDevState = newModuleDevState;
                                                                }

                                                            }
                                                        }
                                                    }

                                                }



                                        stage ++;// CmdLen = 1
                                        break;
                                }
                                stageCmd++; CmdLen--;
                                break;
                            case "MVSS"://MultiSensor Valid Slaves Set and save to flash or Get
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 63 
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 1: //IP address от кого получен ответ
                                        TmpIpAddress = ((UInt32)ResivedBytes[iProcessedBytes]) << 24; //CmdLen--;stageCmd++;
                                        break;
                                    case 2:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 16; //CmdLen--; stageCmd++;
                                        break;
                                    case 3:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 8; //CmdLen--; stageCmd++; 
                                        break;
                                    case 4:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]); //CmdLen--; stageCmd++;
                                        break;

                                    case 5:
                                        TmpNumberSlaveAddress = ResivedBytes[iProcessedBytes];//Число подчиненных устройств в банке 
                                        break;
                                    case 6:
                                        //int SlaveAddress = 0;
                                        for (int i = 0; i < TmpNumberSlaveAddress; i++)
                                        {
                                            TmpValidSlaves[i] = ResivedBytes[iProcessedBytes];

                                            if (i != (TmpNumberSlaveAddress - 1))
                                            {
                                                CmdLen--;
                                                iProcessedBytes++;
                                                if (iProcessedBytes == ResivedBytes.Length)
                                                    iProcessedBytes = 0;
                                            }
                                        }


                                        Module module;// = App.myApp.sModule;//sScillaObject.Buildings[0].Floors[0].Modules[0];

                                        for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
                                            for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                                                for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                                                {
                                                    if (
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_01) << 24) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_02) << 16) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_03) << 8) +
                                                        (UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_04 == TmpIpAddress
                                                            )
                                                    //Для модуля IP адрес, которого совпадает с отправителем полученной датаграммы заполняем состояние мультидатчиков
                                                    {
                                                        module = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m];


                                                        for (int i = 0; i < module.ModuleDevs.Count; i++)
                                                        {
                                                            if (module.ModuleDevs[i].ModuleDevType == 1) //MultiSensor
                                                                module.ModuleDevs[i].Polling = false;
                                                        }

                                                        bool FindOk;
                                                        for (int s = 0; s < TmpNumberSlaveAddress; s++)
                                                        {
                                                            FindOk = false;
                                                               //Ищем в списке устройств модуля
                                                                for (int i = 0; i < module.ModuleDevs.Count; i++)
                                                            {
                                                                if (module.ModuleDevs[i].ModuleDevType == 1) //MultiSensor
                                                                {
                                                                    if (TmpValidSlaves[s] == module.ModuleDevs[i].SlaveAddress)
                                                                    {
                                                                        module.ModuleDevs[i].Polling = true;
                                                                        FindOk = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            if (FindOk == false)
                                                            {
                                                                MessageBoxResult result = MessageBox.Show(string.Format("The Module's polling array contains slave address {0:d}, which is not present in the current Scilla Configuration.", TmpValidSlaves[s]),
                                                                    string.Format("Warning. Module Ip:{0:d}.{1:d}.{2:d}.{3:d}.", module.DESS_ModuleIpAddress_01, module.DESS_ModuleIpAddress_02, module.DESS_ModuleIpAddress_03, module.DESS_ModuleIpAddress_04), MessageBoxButton.OK,MessageBoxImage.Warning);
                                                                if (result == MessageBoxResult.Yes)
                                                                {
                                                                    ;
                                                                    //Application.Current.Shutdown();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }







                                        stage++;// CmdLen = 1
                                        break;
                                }
                                CmdLen--; stageCmd++;
                                break;
                            case "MVSC"://MultiSensor Valid Slaves Change and save to flash
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 63 
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;

                                    case 1: //IP address от кого получен ответ
                                        TmpIpAddress = ((UInt32)ResivedBytes[iProcessedBytes]) << 24; //CmdLen--;stageCmd++;
                                        break;
                                    case 2:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 16; //CmdLen--; stageCmd++;
                                        break;
                                    case 3:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]) << 8; //CmdLen--; stageCmd++; 
                                        break;
                                    case 4:
                                        TmpIpAddress += ((UInt32)ResivedBytes[iProcessedBytes]); //CmdLen--; stageCmd++;
                                        break;


                                    case 5://ответ MVCC
                                        int SlaveAddress = 0;
                                        int NumberSlaveAddress = 0;
                                        //App.myApp.VMM.MVSC_ValidSlaveCollection_Response.Clear();
                                        NumberSlaveAddress = ResivedBytes[iProcessedBytes];//Число подчиненных устройств
                                        CmdLen--; iProcessedBytes++; if (iProcessedBytes == ResivedBytes.Length) iProcessedBytes = 0;

                                        for (int i = 0; i < NumberSlaveAddress; i++)
                                        {
                                            SlaveAddress = ResivedBytes[iProcessedBytes];
                                            //App.myApp.VMM.MVSC_ValidSlaveCollection_Response.Add(new MVSS_Slave_Response(i + 1, SlaveAddress));

                                            if (i != (NumberSlaveAddress - 1))
                                            {
                                                CmdLen--;
                                                iProcessedBytes++;
                                                if (iProcessedBytes == ResivedBytes.Length)
                                                    iProcessedBytes = 0;
                                            }
                                        }
                                        stage++;// CmdLen = 1
                                        break;
                                }
                                CmdLen--; stageCmd++;
                                break;
                            case "MVSV"://MultiSenspor Valid Slave Verify
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 73 
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                        
                                    case 1: //IP address от кого получен ответ
                                        TmpByte = ResivedBytes[iProcessedBytes]; 
                                        break;
                                    case 2:
                                        TmpByte = ResivedBytes[iProcessedBytes]; 
                                        break;
                                    case 3:
                                        TmpByte = ResivedBytes[iProcessedBytes]; 
                                        break;
                                    case 4:
                                        TmpByte = ResivedBytes[iProcessedBytes]; 
                                        break;

                                        
                                    case 5:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevSlaveAdr = ResivedBytes[iProcessedBytes].ToString();// Адрес устройства
                                        break;
                                   
                                    case 6:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevType = ResivedBytes[iProcessedBytes].ToString();// Тип устройства
                                        break;
                                    
                                    case 7:
                                        TmpByte = ResivedBytes[iProcessedBytes];// SerialNumberFirst Hi
                                        break;
                                    case 8:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevSN_First = (ResivedBytes[iProcessedBytes] + (TmpByte << 8)).ToString();
                                        break;
                                    
                                    case 9:
                                        TmpByte = ResivedBytes[iProcessedBytes];// SerialNumberSecond Lo
                                        break;
                                    case 10:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevSN_Second = (ResivedBytes[iProcessedBytes] + (TmpByte << 8)).ToString();
                                        break;
                                    
                                    case 11:
                                        TmpByte = ResivedBytes[iProcessedBytes];// VerMinor Hi
                                        break;
                                    case 12:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevVerMajor = (ResivedBytes[iProcessedBytes] + (TmpByte << 8)).ToString();
                                        break;
                                    
                                    case 13:
                                        TmpByte = ResivedBytes[iProcessedBytes];// VerMinor Lo
                                        break;
                                    case 14:
                                        App.myApp.VMDlgValidSlave.dlgValidSlaveModuleDevVerMinor = (ResivedBytes[iProcessedBytes] + (TmpByte << 8)).ToString();
                                        stage++;
                                        break;
                                        
                                }
                                CmdLen--; stageCmd++;
                                break;

                            case "MTGS":// MVSG MUltiSensor Valid Slaves Get
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 84 
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 1: //IP address от кого получен ответ
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_01 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 2:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_02 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 3:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_03 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 4:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_04 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 5:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_SlaveAddressReceive = ResivedBytes[iProcessedBytes];// Адрес устройства
                                        break;


                                    case 6://
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 7://
                                        Tmp = ResivedBytes[iProcessedBytes];// Адрес устройства
                                        App.myApp.VMS.TabMSensor_InclAnl_S_o = ((Tmp & 0x0001) == 0x0001) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_T_a = ((Tmp & 0x0002) == 0x0002) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_T_d = ((Tmp & 0x0004) == 0x0004) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_S_e = ((Tmp & 0x0008) == 0x0008) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_Flm = ((Tmp & 0x0010) == 0x0010) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_CO = ((Tmp & 0x0020) == 0x0020) ? true : false;
                                        App.myApp.VMS.TabMSensor_InclAnl_VOC = ((Tmp & 0x0040) == 0x0040) ? true : false;
                                        break;
                                    case 8:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 9:
                                        App.myApp.VMS.TabMSensorThWarnSmogO = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]); //CmdLen--; stageCmd++;
                                        break;
                                    case 10:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 11:
                                        App.myApp.VMS.TabMSensorThAlrmSmogO = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 12:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 13:
                                        App.myApp.VMS.TabMSensorThWarnTempA = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 14:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 15:
                                        App.myApp.VMS.TabMSensorThAlrmTempA = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 16:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 17:
                                        App.myApp.VMS.TabMSensorThWarnTempD = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 18:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 19:
                                        App.myApp.VMS.TabMSensorThAlrmTempD = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 20:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 21:
                                        App.myApp.VMS.TabMSensorThWarnSmogE = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 22:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 23:
                                        App.myApp.VMS.TabMSensorThAlrmSmogE = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;



                                    case 24:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 25:
                                        App.myApp.VMS.TabMSensorThWarnFlamA = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 26:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 27:
                                        App.myApp.VMS.TabMSensorThAlrmFlamA = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 28:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 29:
                                        App.myApp.VMS.TabMSensorThWarnFlamD = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 30:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 31:
                                        App.myApp.VMS.TabMSensorThAlrmFlamD = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 32:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 33:
                                        App.myApp.VMS.TabMSensorThWarnCO = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 34:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 35:
                                        App.myApp.VMS.TabMSensorThAlrmCO = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 36:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 37:
                                        App.myApp.VMS.TabMSensorThWarnVOC = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 38:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 39:
                                        App.myApp.VMS.TabMSensorThAlrmVOC = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;


                                    case 40:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 41:
                                        App.myApp.VMS.TabMSensorThSmogORefU = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                    case 42:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 43:
                                        App.myApp.VMS.TabMSensorThSmogO_LED = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        stage++;
                                        break;
                                }
                                CmdLen--; stageCmd++;
                                break;

                            case "MRDG":// MultiSensor Raw Data Get
                                switch (stageCmd)
                                {
                                    case 0:// ID Параметра 68 
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        break;
                                    case 1: //IP address от кого получен ответ
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_01 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 2:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_02 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 3:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_03 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 4:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_ModuleIpAddress_04 = ResivedBytes[iProcessedBytes]; //CmdLen--; stageCmd++;
                                        break;
                                    case 5:
                                        ParamId = ResivedBytes[iProcessedBytes];//stageCmd++;CmdLen--;
                                        //App.myApp.VMM.MTGS_SlaveAddressReceive = ResivedBytes[iProcessedBytes];// Адрес устройства
                                        break;


                                    case 6:
                                        MRDG_RawData[0] = ResivedBytes[iProcessedBytes];
                                        break;
                                        /*
                                    case 7:
                                        MRDG_RawData[0] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;
                                        */
                                    case 7:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 8:
                                        MRDG_RawData[1] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 9:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 10:
                                        MRDG_RawData[2] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 11:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 12:
                                        MRDG_RawData[3] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 13:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 14:
                                        MRDG_RawData[4] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 15:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 16:
                                        MRDG_RawData[5] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 17:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 18:
                                        MRDG_RawData[6] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 19:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 20:
                                        MRDG_RawData[7] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 21:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 22:
                                        MRDG_RawData[8] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 23:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 24:
                                        MRDG_RawData[9] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 25:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 26:
                                        MRDG_RawData[10] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 27:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 28:
                                        MRDG_RawData[11] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 29:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 30:
                                        MRDG_RawData[12] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 31:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 32:
                                        MRDG_RawData[13] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 33:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 34:
                                        MRDG_RawData[14] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 35:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 36:
                                        MRDG_RawData[15] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 37:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 38:
                                        MRDG_RawData[16] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        break;

                                    case 39:
                                        Tmp = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 40:
                                        MRDG_RawData[17] = (ushort)((Tmp << 8) + ResivedBytes[iProcessedBytes]);
                                        stage++;



                                        DataSmog[0, iReady] = MRDG_RawData[2];
                                        DataSmog[1, iReady] = MRDG_RawData[3];
                                        DataSmog[2, iReady] = MRDG_RawData[4];

                                        //MyGraphMS.DataSmogTr[0] = App.myApp.VM_Modbus.HoldingRegMS004;
                                        //MyGraphMS.DataSmogTr[1] = App.myApp.VM_Modbus.HoldingRegMS005;

                                        DataTemp[iReady] = MRDG_RawData[5];

                                        DataTempDig[iReady] = MRDG_RawData[6];

                                        DataSmogE[iReady] = MRDG_RawData[7];

                                        DataFlame[0, iReady] = MRDG_RawData[8];
                                        DataFlame[1, iReady] = MRDG_RawData[9];
                                        DataFlame[2, iReady] = MRDG_RawData[10];
                                        DataFlame[3, iReady] = MRDG_RawData[11];
                                        DataFlame[4, iReady] = MRDG_RawData[12];
                                        DataFlame[5, iReady] = MRDG_RawData[13];

                                        DataFlame[6, iReady] = MRDG_RawData[14];//Min Value
                                        DataFlameSTD[iReady] = MRDG_RawData[15];//STD

                                        DataCO[iReady] = MRDG_RawData[16];




                                        iReady++;
                                        if (iReady == 100)
                                        {
                                            iReady = 0;
                                            ReadyAllData = true;

                                        }
                                        //GraphCanvasMB.Dispatcher.BeginInvoke((Action)(() => { MyGraphMS.Draw1(); }));

                                        //MyGraphMS.DrawAsync(1);
                                        break;

                                }
                                CmdLen--; stageCmd++;
                                break;

                            case "INFO":
                                switch (stageCmd)
                                {
                                    case 0:
                                        INFO_RawData[0] = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 1:
                                        INFO_RawData[1] = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 2:
                                        INFO_RawData[2] = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 3:
                                        INFO_RawData[3] = ResivedBytes[iProcessedBytes];
                                        break;

                                        
                                    case 4: //Id 9 (0x09) Кнопка пажар
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 5:
                                        INFO_RawData[4] = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 6://Id 10 (0x0A) Кнопка охрана
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 7:
                                        INFO_RawData[5] = ResivedBytes[iProcessedBytes];
                                        break;

                                    case 8: //Id 11 (0x0B) Датчик Дым
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 9:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 10://Id 12 (0x0C) Датчик СO
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 11:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;

                                    case 12:// Id 13 (0x0D) Analog Bus 1
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 13:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 14:// Id 14 (0x0E) Analog Bus 2 
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 15:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 16:// Id 15 (0x0F) Analog Bus 3
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 17:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 18:// Id 16 (10x) Analog Bus 4
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 19:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 20:// Id 17 (0x11) Analog Bus 5
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 21:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 22:// Id 18 (12x) Analog Bus 6
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 23:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 24:// Id 19 (0x13) RelayLight
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 25:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 26:// Id 20 0x14 Relay1
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 27:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 28:// Id 21 0x15 Relay2
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 29:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 30:// Id 22 (0x16) RelaySiren
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 31:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 32:// Id 23 (0x17) RelayAlarm
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 33:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 34:// Id 24 (0x18) RelaySignal
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 35:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 36:// Id 28 (0x1C) Датчик наличия внешнего питания
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 37:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 38:// Id 29 0x1D  Датчик наличия внутреннего питания
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 39:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 40:// Id 31 (0x1F) Датчик вибрации
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 41:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 42:// Id 32 (0x20) Датчик вскрытия
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 43:
                                        TmpByte = ResivedBytes[iProcessedBytes];
                                        break;


                                    case 44:// Id 25 (0x19) Напряжение Акумулятора
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 45:
                                        TmpByte = ResivedBytes[iProcessedBytes];//???? 5 bytes
                                        break;
                                    case 46: break;
                                    case 47: break;
                                    case 48: break;
                                    case 49: break;


                                    case 50:// Id 26 (0x1A) Напряжение шины
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 51:
                                        TmpByte = ResivedBytes[iProcessedBytes];//???? 5 bytes
                                        break;
                                    case 52: break;
                                    case 53: break;
                                    case 54: break;
                                    case 55: break;

                                    case 56:// Id 27 (0x1B) Напряжение датчиков
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 57:
                                        TmpByte = ResivedBytes[iProcessedBytes];//??? 5 bytes
                                        break;
                                    case 58: break;
                                    case 59: break;
                                    case 60: break;
                                    case 61: break;

                                    case 62:// Id 30 (0x1E)  Внешняя температура
                                        ParamId = ResivedBytes[iProcessedBytes];
                                        break;
                                    case 63:
                                        TmpByte = ResivedBytes[iProcessedBytes];//????? 5 bytes
                                        break;
                                    case 64: break;
                                    case 65: break;
                                    case 66: break;
                                    case 67:
                                        /***************/


                                        Module module;// = App.myApp.sModule;//sScillaObject.Buildings[0].Floors[0].Modules[0];
                                        UInt16 TempV;
                                        for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
                                            for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                                                for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                                                {
                                                    if (
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_01) << 24) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_02) << 16) +
                                                        (((UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_03) << 8) +
                                                        (UInt32)App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].DESS_ModuleIpAddress_04 == TmpIpAddress
                                                            )
                                                    //Для модуля IP адрес, которого совпадает с отправителем полученной датаграммы проверим изменение состояния кнопок Fire и Security
                                                    {

                                                        module = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m];
                                                        {
                                                            if (module.sended_MASG_and_INFO_count >= 3)
                                                            {
                                                                string str00 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Name; //module.Owner.Owner.Name; ;
                                                                string str01 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Name;//module.Owner.Name;
                                                                string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                                                string str03 = "All";
                                                                string str04 = "Connection recovered";

                                                                App.myApp.Dispatcher.BeginInvoke((Action)(() =>
                                                                {
                                                                    App.myApp.VMS.ScillaEvents.Insert(0, new ScillaEvent(DateTime.Now,
                                                                                                                    str00,//"Строение 1А (Конференц зал)",
                                                                                                                    str01,//"Технический этаж",
                                                                                                                    str02,//module.Name+"IP:192.168.001.201 Module001", 
                                                                                                                    str03,//"Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", "+ module.ModuleDevs[i].Name, //+"Addr:001 MultiSensor 001 ",
                                                                                                                    str04,
                                                                                                                    "This is comment,"));
                                                                }));
                                                            }
                                                            module.sended_MASG_and_INFO_count = 0;
                                                            module.moduleConnectionOk = true;

                                                            if ((module.BtnFireStage != INFO_RawData[4])&&(INFO_RawData[4]==177))
                                                            {
                                                                string str00 = module.Owner.Owner.Name; ;
                                                                string str01 = module.Owner.Name;
                                                                string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                                                string str03 = "---";
                                                                string str04 = "Fire button";

                                                                App.myApp.Dispatcher.BeginInvoke((Action)(() => {
                                                                    App.myApp.VMS.ScillaEvents.Insert(0, new ScillaEvent(DateTime.Now,
                                                                        str00,//"Строение 1А (Конференц зал)",
                                                                        str01,//"Технический этаж",
                                                                        str02,//module.Name+"IP:192.168.001.201 Module001", 
                                                                        str03,//"Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", "+ module.ModuleDevs[i].Name, //+"Addr:001 MultiSensor 001 ",
                                                                        str04,
                                                                        "This is comment,"));
                                                                }));
                                                            }
                                                            module.BtnFireStage = INFO_RawData[4];

                                                            if ((module.BtnSecurityStage != INFO_RawData[5]) && (INFO_RawData[5] == 177))
                                                            {
                                                                string str00 = module.Owner.Owner.Name; ;
                                                                string str01 = module.Owner.Name;
                                                                string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                                                string str03 = "---";
                                                                string str04 = "Security button";

                                                                App.myApp.Dispatcher.BeginInvoke((Action)(() => {
                                                                    App.myApp.VMS.ScillaEvents.Insert(0, new ScillaEvent(DateTime.Now,
                                                                        str00,//"Строение 1А (Конференц зал)",
                                                                        str01,//"Технический этаж",
                                                                        str02,//module.Name+"IP:192.168.001.201 Module001", 
                                                                        str03,//"Addr: " + module.ModuleDevs[i].SlaveAddress.ToString() + ", "+ module.ModuleDevs[i].Name, //+"Addr:001 MultiSensor 001 ",
                                                                        str04,
                                                                        "This is comment,"));
                                                                }));
                                                            }
                                                            module.BtnSecurityStage = INFO_RawData[5];
                                                        }

                                                    }
                                                }






                                                        /*****************/
                                                        stage++;
                                        break;
                                    
                                }
                                CmdLen--; stageCmd++;
                                break;

                            default:
                                stage = 0;
                                break;
                        }
                        break;

                    case 13: //status
                        stage++;
                        break;
                    case 14://Первый байт. Контрольная сумма 
                        stage++;
                        break;
                    case 15://Второй байт. Контрольная сумма
                        stage = 0;
                        break;

                }

                iProcessedBytes++;
                if (iProcessedBytes == ResivedBytes.Length)
                    iProcessedBytes = 0;

            }


        }




    }
}
