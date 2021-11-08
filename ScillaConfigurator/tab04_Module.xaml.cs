using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _ScillaConfigurator
{
    /// <summary>
    /// Interaction logic for tab04_Module.xaml
    /// </summary>
    public partial class tab04_Module : Page
    {
        public tab04_Module()
        {
            InitializeComponent();
            //DataContext = App.myApp.VMCnfTab04;//MainWindow.mainWindow;
            DataContext = App.myApp.VMS;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.sModule.Name = App.myApp.VMS.TabModuleName;
            App.myApp.sModule.Comment = App.myApp.VMS.TabModuleComment;

            App.myApp.sModule.PositionX = App.myApp.VMS.TabModulePositionX;
            App.myApp.sModule.PositionY = App.myApp.VMS.TabModulePositionY;

            App.myApp.sModule.DESS_ModuleIpAddress_01 = App.myApp.VMS.TabModuleIpAddress_01;
            App.myApp.sModule.DESS_ModuleIpAddress_02 = App.myApp.VMS.TabModuleIpAddress_02;
            App.myApp.sModule.DESS_ModuleIpAddress_03 = App.myApp.VMS.TabModuleIpAddress_03;
            App.myApp.sModule.DESS_ModuleIpAddress_04 = App.myApp.VMS.TabModuleIpAddress_04;

            App.myApp.sModule.DESS_ModuleMacAddress_01 = App.myApp.VMS.TabModuleMacAddress_01;
            App.myApp.sModule.DESS_ModuleMacAddress_02 = App.myApp.VMS.TabModuleMacAddress_02;
            App.myApp.sModule.DESS_ModuleMacAddress_03 = App.myApp.VMS.TabModuleMacAddress_03;
            App.myApp.sModule.DESS_ModuleMacAddress_04 = App.myApp.VMS.TabModuleMacAddress_04;
            App.myApp.sModule.DESS_ModuleMacAddress_05 = App.myApp.VMS.TabModuleMacAddress_05;
            App.myApp.sModule.DESS_ModuleMacAddress_06 = App.myApp.VMS.TabModuleMacAddress_06;

            App.myApp.sModule.DESS_ModulePort = App.myApp.VMS.TabModulePort;

            App.myApp.sModule.DESS_ServerIpAddress_01 = App.myApp.VMS.TabModuleServerIpAddress_01;
            App.myApp.sModule.DESS_ServerIpAddress_02 = App.myApp.VMS.TabModuleServerIpAddress_02;
            App.myApp.sModule.DESS_ServerIpAddress_03 = App.myApp.VMS.TabModuleServerIpAddress_03;
            App.myApp.sModule.DESS_ServerIpAddress_04 = App.myApp.VMS.TabModuleServerIpAddress_04;

            App.myApp.sModule.DESS_ServerPort = App.myApp.VMS.TabModuleServerPort;

            App.myApp.sModule.DESS_Tel = App.myApp.VMS.TabModuleTel;
            App.myApp.sModule.DESS_TelAvailable = App.myApp.VMS.TabModuleTelAvailable;





            App.myApp.Plan.Draw();
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.TabModuleName = App.myApp.sModule.Name;
            App.myApp.VMS.TabModuleComment = App.myApp.sModule.Comment;

            App.myApp.VMS.TabModulePositionX = App.myApp.sModule.PositionX;
            App.myApp.VMS.TabModulePositionY = App.myApp.sModule.PositionY;

            App.myApp.VMS.TabModuleIpAddress_01 = App.myApp.sModule.DESS_ModuleIpAddress_01;
            App.myApp.VMS.TabModuleIpAddress_02 = App.myApp.sModule.DESS_ModuleIpAddress_02;
            App.myApp.VMS.TabModuleIpAddress_03 = App.myApp.sModule.DESS_ModuleIpAddress_03;
            App.myApp.VMS.TabModuleIpAddress_04 = App.myApp.sModule.DESS_ModuleIpAddress_04;

            App.myApp.VMS.TabModuleMacAddress_01 = App.myApp.sModule.DESS_ModuleMacAddress_01;
            App.myApp.VMS.TabModuleMacAddress_02 = App.myApp.sModule.DESS_ModuleMacAddress_02;
            App.myApp.VMS.TabModuleMacAddress_03 = App.myApp.sModule.DESS_ModuleMacAddress_03;
            App.myApp.VMS.TabModuleMacAddress_04 = App.myApp.sModule.DESS_ModuleMacAddress_04;
            App.myApp.VMS.TabModuleMacAddress_05 = App.myApp.sModule.DESS_ModuleMacAddress_05;
            App.myApp.VMS.TabModuleMacAddress_06 = App.myApp.sModule.DESS_ModuleMacAddress_06;

            App.myApp.VMS.TabModulePort = App.myApp.sModule.DESS_ModulePort;

            App.myApp.VMS.TabModuleServerIpAddress_01 = App.myApp.sModule.DESS_ServerIpAddress_01;
            App.myApp.VMS.TabModuleServerIpAddress_02 = App.myApp.sModule.DESS_ServerIpAddress_02;
            App.myApp.VMS.TabModuleServerIpAddress_03 = App.myApp.sModule.DESS_ServerIpAddress_03;
            App.myApp.VMS.TabModuleServerIpAddress_04 = App.myApp.sModule.DESS_ServerIpAddress_04;

            App.myApp.VMS.TabModuleServerPort = App.myApp.sModule.DESS_ServerPort;

            App.myApp.VMS.TabModuleTel = App.myApp.sModule.DESS_Tel;
            App.myApp.VMS.TabModuleTelAvailable = App.myApp.sModule.DESS_TelAvailable;
        }


        /*
        async void myDelay()
        {
            await Task.Delay(5000);
            btnStartListener.Content = "";

        }
        */
        /*
        private void btnStartListener_Click(object sender, RoutedEventArgs e)
        {

            BeginReceiveMessages(App.myApp.sModule);
            //myDelay();

          
            App.myApp.sModule.stopListner = false;
            App.myApp.sModule.taskUDP_Listner = Task.Factory.StartNew(()=>App.myApp.sModule.UDP_listening_PI1(App.myApp.sModule));
            
            //App.myApp.VMM.ListnerOk = true;
        }
    */
        /*
        private void btnStopListener_Click(object sender, RoutedEventArgs e)
        {
            //App.myApp.sModule.stopListner = true;
        }
        */



        private void btnMASG_Click(object sender, RoutedEventArgs e)
        {

            short CmdLen = 4;// + 1 + 1;// CmdId + ParamId + ParamData
            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "MASG", 65, null, 0, false);
            /*ВТОРОЙ ВАРИАНТ*/
            //ReceiveMessage(App.myApp.sModule);
            //ТРЕТИЙ ВАРИАНТ

        }
       


        //public struct UdpState
        //{
        //    public UdpClient udpServerListner;
        //    //public IPEndPoint IpEndPointServerListenerPort;
        //    public Module module;
        //}

        public static bool messageReceived = false;


        //static UdpState udpState;// = new UdpState();
        //static UdpClient udpServerListner = null;
        public static void BeginReceiveMessages(Module module)
        {
            // Receive a message and write it to the console.
            //string strModuleAddr = module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString();
            //IPAddress ModuleAddr = IPAddress.Parse(strModuleAddr); //("192.168.1.200")
            //IPEndPoint IpEndPointServerListenerPort = new IPEndPoint(/*ModuleAddr*/ IPAddress.Any /*IPAddress.Broadcast*/, App.myApp.serverListenerPort); //IPAddress.Any

            //IPEndPoint e = new IPEndPoint(IPAddress.Any, s_listenPort);
            /*
            if (App.myApp.udpServerListner == null)
            {
                App.myApp.udpServerListner = new UdpClient(App.myApp.serverListenerPort);//IpEndPoint
                //udpState = new UdpState();
            }
            */
            //udpState.udpServerListner = App.myApp.udpServerListner;
            //udpState.IpEndPointServerListenerPort = IpEndPointServerListenerPort;
            //udpState.module = module;


            //App.myApp.udpServerListner.BeginReceive(new AsyncCallback(App.ReceiveCallback), App.myApp.udpServerListner);
            //udpServerListner.BeginReceive(new AsyncCallback(ReceiveCallback), udpState);


            //short CmdLen = 4;// + 1 + 1;// CmdId + ParamId + ParamData
            //App.myApp.sModule.SendCmd(CmdLen, "ISSR", "MASG", 65, null, 0, false);

            //udpClient.Client.SendTimeout = 100;

           

        }
        /*
        public static void ReceiveCallback(IAsyncResult ar)
        {
            
            UdpClient udpServerListner = ar.AsyncState as UdpClient;
            //UdpClient udpServerListner = ((UdpState)(ar.AsyncState)).udpServerListner;
            //Module module = ((UdpState)(ar.AsyncState)).module;

            //IPEndPoint IpEndPointServerListenerPort = ((UdpState)(ar.AsyncState)).IpEndPointServerListenerPort;
            IPEndPoint IpEndPointServerListenerPort = new IPEndPoint(IPAddress.Any, App.myApp.serverListenerPort);


            byte[] bytes = udpServerListner.EndReceive(ar, ref IpEndPointServerListenerPort);
            //string receiveString = Encoding.ASCII.GetString(receiveBytes);

            for (int i = 0; i < bytes.Length; i++)
            {
                Module.ResivedBytes[Module.iResivedBytes] = bytes[i];
                Module.iResivedBytes++;
                if (Module.iResivedBytes == Module.ResivedBytes.Length)
                {
                    Module.iResivedBytes = 0;
                    Module.FlagResivedBytesFull = true;
                }
            }
            Module.Recive();//module

        //Console.WriteLine($"Received: {receiveString}");

        udpServerListner.BeginReceive(new AsyncCallback(ReceiveCallback), udpServerListner);//udpState
        }
        */
        private void btnGetStage_Click(object sender, RoutedEventArgs e)
        {
            short CmdLen = 4;// + 1 + 1;// CmdId + ParamId + ParamData
            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(),CmdLen, "ISSR", "MASG", 65, null, 0, false);
        }

        private void btnBeepModule_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[4];
            int ParamId = 80;//
            ParamData[0] = 0;
            ParamData[1] = 0;
            ParamData[2] = 0;
            ParamData[3] =  1;

            short CmdLen = 4 + 1 + 4;// CmdId(4) + ParamId(1) + ParamData(3)

            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "MASB", ParamId, ParamData, ParamData.Length, false);
        }

        private void btnGetDescriptor_Click(object sender, RoutedEventArgs e)
        {
            short CmdLen = 4;// + 1 + 1;// CmdId + ParamId + ParamData
            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "DESG", 19, null, 0,false);
        }

        private void btnSetDescriptor_Click(object sender, RoutedEventArgs e)
        {

            
             byte[] ParamData = new byte[38];

            int ParamId = 1;// здесь не используется, засунут в ParamData

            ParamData[0] = 1;
            ParamData[1] = App.myApp.VMS.TabModuleIpAddress_01;
            ParamData[2] = App.myApp.VMS.TabModuleIpAddress_02;
            ParamData[3] = App.myApp.VMS.TabModuleIpAddress_03;
            ParamData[4] = App.myApp.VMS.TabModuleIpAddress_04;

            ParamData[5] = 2;
            ParamData[6] = App.myApp.VMS.TabModuleMacAddress_01;
            ParamData[7] = App.myApp.VMS.TabModuleMacAddress_02;
            ParamData[8] = App.myApp.VMS.TabModuleMacAddress_03;
            ParamData[9] = App.myApp.VMS.TabModuleMacAddress_04;
            ParamData[10] = App.myApp.VMS.TabModuleMacAddress_05;
            ParamData[11] = App.myApp.VMS.TabModuleMacAddress_06;

            ParamData[12] = 3;
            
            ParamData[13] = (byte)(App.myApp.VMS.TabModulePort >> 8);
            ParamData[14] = (byte)(App.myApp.VMS.TabModulePort);
            

            ParamData[15] = 4;
            ParamData[16] = App.myApp.VMS.TabModuleServerIpAddress_01;
            ParamData[17] = App.myApp.VMS.TabModuleServerIpAddress_02;
            ParamData[18] = App.myApp.VMS.TabModuleServerIpAddress_03;
            ParamData[19] = App.myApp.VMS.TabModuleServerIpAddress_04;

            ParamData[20] = 5;
            ParamData[21] = (byte)(App.myApp.VMS.TabModuleServerPort >> 8);
            ParamData[22] = (byte)(App.myApp.VMS.TabModuleServerPort);

            char[] ch = new char[50];
            int n = ch.Length;
            ParamData[23] = 6;
            ch = App.myApp.VMS.TabModuleTel.ToCharArray();

            n = ch.Length;
            for (int i = 0; i < 12; i++)
                if(i< ch.Length)
                    ParamData[24 + i] = (byte)(ch[i]-'0');
                else
                    ParamData[24 + i] = 0;


            ParamData[36] = 7;
            ParamData[37] = (byte)((App.myApp.VMS.TabModuleTelAvailable == true) ? 1 : 0);

            short CmdLen = 4 + 1 + 4 + 1 + 6 + 1 + 2 + 1 + 4 + 1 + 2 + 1 + 12 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "DESS", ParamId, ParamData, ParamData.Length,false);
             

        }

        private void btnResetDescriptor_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.TabModuleIpAddress_01 = 0;
            App.myApp.VMS.TabModuleIpAddress_02 = 0;
            App.myApp.VMS.TabModuleIpAddress_03 = 0;
            App.myApp.VMS.TabModuleIpAddress_04 = 0;

            App.myApp.VMS.TabModuleMacAddress_01 = 0;
            App.myApp.VMS.TabModuleMacAddress_02 = 0;
            App.myApp.VMS.TabModuleMacAddress_03 = 0;
            App.myApp.VMS.TabModuleMacAddress_04 = 0;
            App.myApp.VMS.TabModuleMacAddress_05 = 0;
            App.myApp.VMS.TabModuleMacAddress_06 = 0;

            App.myApp.VMS.TabModulePort = 0;

            App.myApp.VMS.TabModuleServerIpAddress_01 = 0;
            App.myApp.VMS.TabModuleServerIpAddress_02 = 0;
            App.myApp.VMS.TabModuleServerIpAddress_03 = 0;
            App.myApp.VMS.TabModuleServerIpAddress_04 = 0;

            App.myApp.VMS.TabModuleServerPort = 0;

            App.myApp.VMS.TabModuleTel = "";
            App.myApp.VMS.TabModuleTelAvailable = false;
        }

        private void btnGetPollingArray_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[256];

            int ParamId = 62;
            int slaveCount = 0;
        
            ParamData[0] = (byte)0;//flag - GET
            ParamData[1] = (byte)slaveCount;

            short CmdLen = (short)(4 + 1 + 1 + (1 + slaveCount));// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "MVSS", ParamId, ParamData, slaveCount + 1 + 1, false);
        }

        private void btnSetPollingArray_Click(object sender, RoutedEventArgs e)
        {
            byte[] ParamData = new byte[256];

            int ParamId = 62;
            int slaveCount = 0;
                
            for (int i = 0; i < App.myApp.sModule.ModuleDevs.Count; i++)
            {
                if (App.myApp.sModule.ModuleDevs[i].ModuleDevType==1  && App.myApp.sModule.ModuleDevs[i].Polling == true)
                {
                    ParamData[slaveCount + 2] = (byte)(App.myApp.sModule.ModuleDevs[i].SlaveAddress);
                    slaveCount++;
                }
            }

            ParamData[0] = (byte)1;////flag - SET
            ParamData[1] = (byte)slaveCount;

            short CmdLen = (short)(4 + 1 + 1 + (1 + slaveCount));// CmdId(4) + ParamId(1) + ParamData(1)

            App.myApp.SendCmd(App.myApp.sModule.getIpAddress(), CmdLen, "ISSR", "MVSS", ParamId, ParamData, slaveCount + 1 + 1,false);
        }

        /*ВТОРОЙ ВАРИАНТ*/

        /*
        private async Task ReceiveMessage(Module module)
        {
            using (var udpClient = new UdpClient(App.myApp.serverListenerPort))
            {

                var receivedResult = await udpClient.ReceiveAsync();
                byte[] bytes = receivedResult.Buffer;
                for (int i = 0; i < bytes.Length; i++)
                {
                    module.ResivedBytes[module.iResivedBytes] = bytes[i];
                    module.iResivedBytes++;
                    if (module.iResivedBytes == module.ResivedBytes.Length)
                    {
                        module.iResivedBytes = 0;
                        module.FlagResivedBytesFull = true;
                    }
                }

                module.Recive(module);
            }
        }
        */

    }   
}
