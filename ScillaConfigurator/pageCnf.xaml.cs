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

using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//using static _ScillaConfigurator.ViewModelCnf;

namespace _ScillaConfigurator
{
    /// <summary>
    /// Interaction logic for pageCnf.xaml
    /// </summary>

  
    public partial class pageCnf : Page
    {

        static public Timer timerPollingModules;
      
        static private Boolean canDragModules = true;
        static public Canvas CANVAS_PLAN_AREA = null;
        static public ContextMenu CANVAS_CONTTEXT_MENU = null;
        static public TabControl TB_PROPERTIES = null;

        public ContextMenu CanvasContextMenu { get; }

        /*{+*/

        public pageCnf()
        {
            InitializeComponent();
            CANVAS_PLAN_AREA = CanvasPlanArea;
  //       CANVAS_CONTTEXT_MENU = CanvasContextMenu;

            this.DataContext = App.myApp.VMS;
              App.myApp.Plan = new PlanArea(CanvasPlanArea);
            App.myApp.RulerX = new RulerX_Area(CanvasRulerX);
            App.myApp.RulerY = new RulerY_Area(CanvasRulerY);
            App.myApp.ScrollViewerCan = ScrlViewerPlan;

            this.Unloaded += this.OnUnloaded;
            this.Loaded += this.OnLoaded;
           
  
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            if (File.Exists("C:\\Projects_WPF\\Scilla\\Test Object.cnf"))
            {
                using (BinaryReader reader = new BinaryReader(File.Open("C:\\Projects_WPF\\Scilla\\Test Object.cnf", FileMode.Open)))
                {
                    App.myApp.VMS.ScillaObjects[0].Read(reader);
                }
                App.myApp.sBuilding = null;
                App.myApp.sFloor = null;
                App.myApp.sModule = null;
                App.myApp.Plan.Draw();
            }
            

        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (timerPollingModules != null)
            {
                timerPollingModules.Dispose();
                timerPollingModules = null;
            }
        }
  
        private void CanvasPlanArea_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(CanvasPlanArea);
            tbXY.Text = String.Format("X:{0,10:F2}    Y:{1,10:F2}    ZOOM:{2,10:F2}", pt.X/ App.myApp.VMS.ImageZoom, pt.Y/ App.myApp.VMS.ImageZoom, App.myApp.VMS.ImageZoom);

            if (App.myApp.sFloor != null)
            {
                if (CanvasPlanArea.IsMouseCaptured == true)
                {
                    if(App.myApp.iDragMultiSensor!=null && App.myApp.iDragModule != null)
                    {
                        App.myApp.sFloor.Modules[(int)App.myApp.iDragModule].ModuleDevs[(int)App.myApp.iDragMultiSensor].PositionX = pt.X / App.myApp.VMS.ImageZoom;
                        App.myApp.sFloor.Modules[(int)App.myApp.iDragModule].ModuleDevs[(int)App.myApp.iDragMultiSensor].PositionY = pt.Y / App.myApp.VMS.ImageZoom;
                        App.myApp.Plan.Draw();
                    }
                    else
                    if (App.myApp.iDragMultiSensor == null && App.myApp.iDragModule != null)
                    {
                        App.myApp.sFloor.Modules[(int)App.myApp.iDragModule].PositionX = pt.X / App.myApp.VMS.ImageZoom;
                        App.myApp.sFloor.Modules[(int)App.myApp.iDragModule].PositionY = pt.Y / App.myApp.VMS.ImageZoom;
                        App.myApp.Plan.Draw();
                    }
                }
                else
                {
                    int? iModule = null;
                    int? iMultiSensor = null;

                    //Переберем все Modules
                    for (int m = 0; m < App.myApp.sFloor.Modules.Count; m++)
                    {
                        if (App.myApp.sFloor.Modules[m].PositionX > (pt.X - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].PositionX < (pt.X + 15) / App.myApp.VMS.ImageZoom &&
                            App.myApp.sFloor.Modules[m].PositionY > (pt.Y - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].PositionY < (pt.Y + 15) / App.myApp.VMS.ImageZoom)
                        {
                            iModule = m;
                            break;
                        }

                    }

                    //Переберем все MultiSensor
                    if (iModule == null)
                        for (int m = 0; m < App.myApp.sFloor.Modules.Count; m++)
                            for (int k = 0; k < App.myApp.sFloor.Modules[m].ModuleDevs.Count; k++)
                            {
                                if (App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionX > (pt.X - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionX < (pt.X + 15) / App.myApp.VMS.ImageZoom &&
                               App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionY > (pt.Y - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionY < (pt.Y + 15) / App.myApp.VMS.ImageZoom)
                                {
                                    iModule = m;
                                    iMultiSensor = k;
                                    break;
                                }
                            }


                    if (iModule != App.myApp.iLightModule || iMultiSensor != App.myApp.iLightMultiSensor)
                    {
                        
                        App.myApp.iLightModule = iModule;//+{Указатель на объекте
                        App.myApp.iLightMultiSensor = iMultiSensor;
                        App.myApp.Plan.Draw();
                           

                    }

                }
            }
        }

      
        private void CanvasPlanArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(CanvasPlanArea);
            tbXY.Text = String.Format("X:{0,10:F2}    Y:{1,10:F2}    ZOOM:{2,10:F2}", pt.X / App.myApp.VMS.ImageZoom, pt.Y / App.myApp.VMS.ImageZoom, App.myApp.VMS.ImageZoom);

            if (App.myApp.sFloor != null)
            {
                int? iModule = null;
                int? iMultiSensor = null;

                for (int m = 0; m < App.myApp.sFloor.Modules.Count; m++)
                {
                    if (App.myApp.sFloor.Modules[m].PositionX > (pt.X - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].PositionX < (pt.X + 15) / App.myApp.VMS.ImageZoom &&
                        App.myApp.sFloor.Modules[m].PositionY > (pt.Y - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].PositionY < (pt.Y + 15) / App.myApp.VMS.ImageZoom)
                    {
                        iModule = m;
                        break;
                    }
                }

                if (iModule != null)
                    App.myApp.sFloor.Modules[(int)iModule].IsSelected = true;

                else
                {
                    for (int m = 0; m < App.myApp.sFloor.Modules.Count; m++)
                        for (int k = 0; k < App.myApp.sFloor.Modules[m].ModuleDevs.Count; k++)
                        {
                            if (App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionX > (pt.X - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionX < (pt.X + 15) / App.myApp.VMS.ImageZoom &&
                           App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionY > (pt.Y - 15) / App.myApp.VMS.ImageZoom && App.myApp.sFloor.Modules[m].ModuleDevs[k].PositionY < (pt.Y + 15) / App.myApp.VMS.ImageZoom)
                            {
                                iModule = m;
                                iMultiSensor = k;
                                break;
                            }
                        }
                    if (iMultiSensor != null)
                        App.myApp.sFloor.Modules[(int)iModule].ModuleDevs[(int)iMultiSensor].IsSelected = true;
                }


                    App.myApp.iLightModule = iModule;
                    App.myApp.iLightMultiSensor = iMultiSensor;
                    App.myApp.Plan.Draw();

                if (iMultiSensor != null || iModule != null)
                {
                    App.myApp.iDragModule = iModule;
                    App.myApp.iDragMultiSensor = iMultiSensor;
                    Mouse.Capture(CanvasPlanArea);
                }
            }
        }

        private void CanvasPlanArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Mouse.Captured
            Mouse.Capture(null);
            App.myApp.iDragModule = null;
            App.myApp.iDragMultiSensor = null;
        /*
        if (this.current.InputElement != null)

            this.current.InputElement.ReleaseMouseCapture();
        */
    }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           

            TreeView tv = (TreeView)sender;
            Type type = (Type)tv.SelectedItem.GetType();//Name
                                                        //tv.Items.CurrentItem.
                                                        //MessageBox.Show( tv.Items.CurrentItem.ToString );


            int j = tv.Items.IndexOf(tv.SelectedItem);
            //tv.Items[0].I

            string str1 = tv.SelectedItem.ToString();

            string str2 = tv.SelectedValue.ToString();


            App.myApp.sScillaObject = null;
            App.myApp.sBuilding = null;
            App.myApp.sFloor = null;
            App.myApp.sModule = null;
            App.myApp.sMultiSensor = null;



            if (type.Name == "ScillaObject")
            {
                App.myApp.sScillaObject = (ScillaObject)tv.SelectedItem;
                
                App.myApp.VMCnfTab01.Name = App.myApp.sScillaObject.Name;
                App.myApp.VMCnfTab01.Region = App.myApp.sScillaObject.Region;
                App.myApp.VMCnfTab01.City = App.myApp.sScillaObject.City;
                App.myApp.VMCnfTab01.Build = App.myApp.sScillaObject.Build;
                App.myApp.VMCnfTab01.Person = App.myApp.sScillaObject.Person;
                App.myApp.VMCnfTab01.TelN = App.myApp.sScillaObject.TelN;
              
                tbProperties.SelectedIndex = 1;
            }
            else
            if (type.Name == "Building")
            {
                App.myApp.sBuilding = (Building)tv.SelectedItem;

                App.myApp.VMCnfTab02.Name = App.myApp.sBuilding.Name;
                App.myApp.VMCnfTab02.Comment = App.myApp.sBuilding.Comment;

                tbProperties.SelectedIndex = 2;//
            }
            else
            if (type.Name == "Floor")
            {
                App.myApp.sFloor = (Floor)tv.SelectedItem;

                App.myApp.VMCnfTab03.Name = App.myApp.sFloor.Name;
                App.myApp.VMCnfTab03.Comment = App.myApp.sFloor.Comment;
                App.myApp.VMCnfTab03.FloorWidth = App.myApp.sFloor.FloorWidth;
                App.myApp.VMCnfTab03.FloorHeight = App.myApp.sFloor.FloorHeight;
                App.myApp.VMCnfTab03.FloorHorizontalShift = App.myApp.sFloor.FloorHorizontalShift;
                App.myApp.VMCnfTab03.FloorVerticalShift = App.myApp.sFloor.FloorVerticalShift;


                App.myApp.Plan.Draw();
                tbProperties.SelectedIndex = 3;//
            }
            else
            if (type.Name == "Module")
            {

                App.myApp.sFloor = ((Module)tv.SelectedItem).Owner;

                App.myApp.sModule = (Module)tv.SelectedItem;

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

                App.myApp.Plan.Draw();
                tbProperties.SelectedIndex = 4;//
            }
            else
            if (type.Name == "MultiSensor")
            {
                App.myApp.sFloor = ((Module)(((MultiSensor)tv.SelectedItem).Owner)).Owner;
                App.myApp.sMultiSensor = (MultiSensor)tv.SelectedItem;

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
                App.myApp.VMS.TabMSensor_InclAnl_CO = App.myApp.sMultiSensor.InclAnl_CO;
                App.myApp.VMS.TabMSensor_InclAnl_VOC = App.myApp.sMultiSensor.InclAnl_VOC;

                App.myApp.VMS.TabMSensorThWarnSmogO = App.myApp.sMultiSensor.ThWarnSmogO;
                App.myApp.VMS.TabMSensorThAlrmSmogO = App.myApp.sMultiSensor.ThAlrmSmogO;
                App.myApp.VMS.TabMSensorThSmogORefU = App.myApp.sMultiSensor.ThSmogORefU;
                App.myApp.VMS.TabMSensorThSmogO_LED = App.myApp.sMultiSensor.ThSmogO_LED;

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
            
                App.myApp.Plan.Draw();
                tbProperties.SelectedIndex = 5;//
            }


            else
            if (type.Name == "RelayUnit")
            {
                App.myApp.sFloor = ((Module)(((RelayUnit)tv.SelectedItem).Owner)).Owner;
                App.myApp.sRelayUnit = (RelayUnit)tv.SelectedItem;

                App.myApp.VMS.TabRelayUnitName = App.myApp.sRelayUnit.Name;
                App.myApp.VMS.TabRelayUnitComment = App.myApp.sRelayUnit.Comment;

                App.myApp.VMS.TabRelayUnitPositionX = App.myApp.sRelayUnit.PositionX;
                App.myApp.VMS.TabRelayUnitPositionY = App.myApp.sRelayUnit.PositionY;

                App.myApp.VMS.TabRelayUnitSlaveAddress = App.myApp.sRelayUnit.SlaveAddress;

                App.myApp.Plan.Draw();
                tbProperties.SelectedIndex = 6;//
            }

            else
            if (type.Name == "BusTag")
            {
                App.myApp.sFloor = ((Module)(((BusTag)tv.SelectedItem).Owner)).Owner;
                App.myApp.sBusTag = (BusTag)tv.SelectedItem;

                App.myApp.VMS.TabBusTagName = App.myApp.sBusTag.Name;
                App.myApp.VMS.TabBusTagComment = App.myApp.sBusTag.Comment;

                App.myApp.VMS.TabBusTagPositionX = App.myApp.sBusTag.PositionX;
                App.myApp.VMS.TabBusTagPositionY = App.myApp.sBusTag.PositionY;

                App.myApp.VMS.TabBusTagSlaveAddress = App.myApp.sBusTag.SlaveAddress;

                App.myApp.VMS.TabBusTagInput1 = App.myApp.sBusTag.Input1;
                App.myApp.VMS.TabBusTagInput2 = App.myApp.sBusTag.Input2;
                App.myApp.VMS.TabBusTagInput3 = App.myApp.sBusTag.Input3;
                App.myApp.VMS.TabBusTagRelay = App.myApp.sBusTag.Relay;

                App.myApp.Plan.Draw();
                tbProperties.SelectedIndex = 7;//
            }

            else
                tbProperties.SelectedIndex = 0;
            
            //TreeView1.Items.RemoveAt(TreeView1.Items.IndexOf(TreeView1.SelectedItem));
        }

     
        //bool Expanded = false;
        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                {
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].IsExpanded = false;
                    }
                    App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].IsExpanded = false;
                }
                App.myApp.VMS.ScillaObjects[0].Buildings[b].IsExpanded = false;
            }
            App.myApp.VMS.ScillaObjects[0].IsExpanded = false;



            /*            for (int i = 0; i < App.myApp.VMS.ScillaObjects[0].Buildings.Count; i++)
                            App.myApp.VMS.ScillaObjects[0].Buildings[i].IsExpanded = false;
            */
        }



        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                {
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].IsExpanded = true;
                    }
                    App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].IsExpanded = true;
                }
                App.myApp.VMS.ScillaObjects[0].Buildings[b].IsExpanded = true;
            }
            App.myApp.VMS.ScillaObjects[0].IsExpanded = true;
        }

        

        private void btnBuildingExpand_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                {
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].IsExpanded = false;
                    }
                    App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].IsExpanded = false;
                }
                App.myApp.VMS.ScillaObjects[0].Buildings[b].IsExpanded = false;
            }
            App.myApp.VMS.ScillaObjects[0].IsExpanded = true;


        }

        private void btnFloorExpand_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                {
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].IsExpanded = false;
                    }
                    App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].IsExpanded = false;
                }
                App.myApp.VMS.ScillaObjects[0].Buildings[b].IsExpanded = true;
            }
            App.myApp.VMS.ScillaObjects[0].IsExpanded = true;
        }

        private void btnModuleExpand_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                {
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].IsExpanded = false;
                    }
                    App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].IsExpanded = true;
                }
                App.myApp.VMS.ScillaObjects[0].Buildings[b].IsExpanded = true;
            }
            App.myApp.VMS.ScillaObjects[0].IsExpanded = true;
        }

        /****************************************************************************************************************/
        private void SldrPlanZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(App.myApp.sFloor !=null)
                App.myApp.Plan.Draw();
        }

        private void btnZoomDown_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.ImageZoom-=0.25;
            if (App.myApp.VMS.ImageZoom < 1.0)
                App.myApp.VMS.ImageZoom = 1.0;
            App.myApp.Plan.Draw();
        }

        private void btnZoomUp_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.ImageZoom+=0.25;
            if(App.myApp.VMS.ImageZoom > 5)
                App.myApp.VMS.ImageZoom = 5;
            App.myApp.Plan.Draw();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            ScrollViewer c = sender as ScrollViewer;

            if (App.myApp.sFloor != null)
            {
                App.myApp.sFloor.ScrollX = e.HorizontalOffset;
                App.myApp.sFloor.ScrollY = e.VerticalOffset;
            }

            App.myApp.RulerX.Draw();
            App.myApp.RulerY.Draw();
        }
        private void CanvasPlanArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double zoom = App.myApp.VMS.ImageZoom;
                zoom += e.Delta / 100 * 0.1;
                if (zoom < 1.0)
                    zoom = 1.0;
                if (zoom > 5)
                    zoom = 5;
                App.myApp.VMS.ImageZoom = zoom;
                App.myApp.Plan.Draw();
            }
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            App.myApp.Plan.Draw();
        }
        /*
        private void btnStartUDP_Listener_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.udpServerListner == null)
            {
                App.myApp.udpServerListner = new UdpClient(App.myApp.serverListenerPort);//IpEndPoint
                //В этой строчке нужно перехватить исключение если порт уже занят

                App.myApp.udpServerListner.BeginReceive(new AsyncCallback(App.ReceiveCallback), App.myApp.udpServerListner);
                btnStartUDP_Listener.Content = "Stop UDP Listener";
                App.myApp.VMS.UDP_Listener = true;
            }
            else
            {
                App.myApp.udpServerListner.Close();
                App.myApp.udpServerListner = null;
                btnStartUDP_Listener.Content = "Start UDP Listener";
                App.myApp.VMS.UDP_Listener = false;
            }
        }
        */
        private void btnStartUDP_Listener_Checked(object sender, RoutedEventArgs e)
        {
            if (App.myApp.udpServerListner == null)
            {
                App.myApp.udpServerListner = new UdpClient(App.myApp.serverListenerPort);//IpEndPoint
                //В этой строчке нужно перехватить исключение если порт уже занят

                App.myApp.udpServerListner.BeginReceive(new AsyncCallback(App.ReceiveCallback), App.myApp.udpServerListner);
 //-{}               btnStartUDP_Listener.Content = "Stop UDP Listener";
                btnStartUDP_Listener.Content = "Остановить опрос";
                App.myApp.VMS.UDP_Listener = true;
            }
        }

        private void btnStartUDP_Listener_Unchecked(object sender, RoutedEventArgs e)
        {
            if (App.myApp.udpServerListner != null)
            {
                App.myApp.udpServerListner.Close();
                App.myApp.udpServerListner = null;
                btnStartUDP_Listener.Content = "Опросить сеть";
                App.myApp.VMS.UDP_Listener = false;
            }
        }
        /*
        public static void ReceiveCallback(IAsyncResult ar)
        {

            UdpClient udpServerListner = ar.AsyncState as UdpClient;
            IPEndPoint IpEndPointServerListenerPort = new IPEndPoint(IPAddress.Any, App.myApp.serverListenerPort);

            try
            {

                byte[] bytes = udpServerListner.EndReceive(ar, ref IpEndPointServerListenerPort);
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
        udpServerListner.BeginReceive(new AsyncCallback(ReceiveCallback), udpServerListner);//udpState
    }
            catch(Exception e)
            {;
            }

            
        }
*/

        private void btnGetStage_Click(object sender, RoutedEventArgs e)
        {

            GetStage();
            System.Media.SystemSounds.Beep.Play();

            //Brodcast отрабатывается нормально 
            //App.myApp.sModule.SendCmd(CmdLen, "ISSR", "MASG", 65, null, 0, true);
        }

        private void GetStage()
        {
            Module module;
            short CmdLen;// + 1 + 1;// CmdId + ParamId + ParamData
            int ParamID;
            //App.myApp.sModule.SendCmd(CmdLen, "ISSR", "MASG", 65, null, 0, false);

            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
            {
                if (App.myApp.VMS.ScillaObjects[0].Buildings[b].Polling == true)
                {
                    for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                    {
                        if(App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Polling == true)
                        {
                            for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                            {
                                if (App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].Polling == true)
                                {
                                    //App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].SendCmd(CmdLen, "ISSR", "MASG", 65, null, 0, false);
                                    App.myApp.SendCmd(App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].getIpAddress(), CmdLen=4, "ISSR", "MASG", ParamID = 65, null, 0, false);
                                    App.myApp.SendCmd(App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].getIpAddress(), CmdLen=4, "ISSR", "INFO", ParamID = 19, null, 0, false);


                                    if (App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].sended_MASG_and_INFO_count >= 3)
                                    {
                                        module = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m];

                                        if (App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].moduleConnectionOk == true)
                                        {
                                            string str00 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Name; //module.Owner.Owner.Name; ;
                                            string str01 = App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Name;//module.Owner.Name;
                                            string str02 = "IP: " + module.DESS_ModuleIpAddress_01.ToString() + "." + module.DESS_ModuleIpAddress_02.ToString() + "." + module.DESS_ModuleIpAddress_03.ToString() + "." + module.DESS_ModuleIpAddress_04.ToString() + ", " + module.Name;
                                            string str03 = "All";
                                            string str04 = "Connection Error";


                                            App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].moduleConnectionOk = false;

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
                                    }
                                    else
                                    {
                                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].sended_MASG_and_INFO_count++;
                                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].moduleConnectionOk = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnResetStage_Click(object sender, RoutedEventArgs e)
        {
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                        for (int i = 0; i < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].ModuleDevs.Count; i++)
                        {
                            App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].ModuleDevs[i].moduleDevState = MdState.MS_NotSet;
                            App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].ModuleDevs[i].moduleDevErrorCount=0;
                        }

            App.myApp.VMS.PollingCounter = 0;
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            //App.myApp.VMS.ScillaEvents.Add(new ScillaEvent(DateTime.Now, "Строение 1А (Конференц зал)", "Технический этаж", "IP:192.168.001.201 Module001", "Addr:001 MultiSensor 001 ","","This is comment,"));
            App.myApp.VMS.ScillaEvents.Insert(0, new ScillaEvent(DateTime.Now, "Строение 1А (Конференц зал)", "Технический этаж", "IP:192.168.001.201 Module001", "Addr:001 MultiSensor 001 ", "", "This is comment,"));
        }

        private void btnDelEvent_Click(object sender, RoutedEventArgs e)
        {
            int i = EventDataGrid.SelectedIndex;
            if(i>=0 && i< App.myApp.VMS.ScillaEvents.Count)
                App.myApp.VMS.ScillaEvents.Remove(App.myApp.VMS.ScillaEvents[i]);
        }

        private void btnClearEvents_Click(object sender, RoutedEventArgs e)
        {
            App.myApp.VMS.ScillaEvents.Clear();
        }

       

        private void btnStartPolling_Checked(object sender, RoutedEventArgs e)
        {
            btnStartPolling.Content = "Остановить опрос";
            for (int b = 0; b < App.myApp.VMS.ScillaObjects[0].Buildings.Count; b++)
                for (int f = 0; f < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors.Count; f++)
                    for (int m = 0; m < App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules.Count; m++)
                    {
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].sended_MASG_and_INFO_count = 0;
                        App.myApp.VMS.ScillaObjects[0].Buildings[b].Floors[f].Modules[m].moduleConnectionOk = true;
                        
                    }
                 StartPollingMultiSensor(true);
 
    }

        private void btnStartPolling_Unchecked(object sender, RoutedEventArgs e)
        {
            btnStartPolling.Content = "Начать опрос";
            StartPollingMultiSensor(false);
   
        }

        public void StartPollingMultiSensor(bool start)
        {
            if (start)
            {
                // Делегат для типа Timer
                TimerCallback timerPollingCallback = new TimerCallback(DoPolling);
                //Класс Timer из пространства имен System.Threading предлагает ключевую функциональность. 
                //В его конструкторе можно пере давать делегат, который должен вызываться через указываемый интервал времени.
                //pageModbus.RunType = 3;
                timerPollingModules = new Timer(timerPollingCallback, null, 1000, 1000);
                //btnStartTimer.Content = "Stop Timer";
            }
            else
            {
                timerPollingModules.Dispose();
                timerPollingModules = null;
                //btnStartTimer.Content = "Start Timer";
            }

            void DoPolling(object state)
            {
                GetStage();
                System.Media.SystemSounds.Beep.Play();
                App.myApp.VMS.PollingCounter++;
                /*//+{
                int ParamId = 67;
                byte[] ParamData = new byte[1];
                ParamData[0] = (byte)App.myApp.VMS.TabMSensorSlaveAddress; //(byte)App.myApp.VMM.MVSV_SlaveAddressSend;
                short CmdLen = 4 + 1 + 1;// CmdId(4) + ParamId(1) + ParamData(1)

                App.myApp.SendCmd(App.myApp.sMultiSensor.Owner.getIpAddress(), CmdLen, "ISSR", "MRDG", ParamId, ParamData, ParamData.Length, false);
                GraphCanvasMB.Dispatcher.BeginInvoke((Action)(() => { MyGraphMS.DrawAsync(VM_DlgDrawRawData.MyGraphTabControlSelectedIndex); }));
                GraphCanvasMB.Dispatcher.BeginInvoke((Action)(() => { StageLabelSetup(); }));
                //MyGraphMB.Draw1();
                }*/
            }

        }
    }
}
