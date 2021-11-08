using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

using System.Drawing;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using System.IO;

using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.Sockets;
using System.Net;
using System.Globalization;
using _ScillaConfigurator.dlg;

namespace _ScillaConfigurator
{

    public class ScillaObject : INotifyPropertyChanged
    {

        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); } }

        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; NotifyPropertyChanged("IsExpanded"); } }

        private string _Name ="";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Region = "";
        public string Region { get { return _Region; } set { _Region = value; NotifyPropertyChanged("Region"); } }

        private string _City = "";
        public string City { get { return _City; } set { _City = value; NotifyPropertyChanged("City"); } }

        private string _Build = "";
        public string Build { get { return _Build; } set { _Build = value; NotifyPropertyChanged("Build"); } }

        private string _Person = "";
        public string Person { get { return _Person; } set { _Person = value; NotifyPropertyChanged("Person"); } }

        private string _TelN = "";
        public string TelN { get { return _TelN; } set { _TelN = value; NotifyPropertyChanged("TelN"); } }

        private bool _Polling = true;
        public bool Polling { get { return _Polling; } set { _Polling = value; NotifyPropertyChanged("Polling"); } }
        public ObservableCollection<Building> Buildings { get; set; }
        public ICommand OnCommandScillaObject { get; set; }
        public ScillaObject(string name)
        {
            Name = name;
            Buildings = new ObservableCollection<Building>();
            OnCommandScillaObject = new CommandScillaObject(OnCommandScillaObjectExec);

           
        }

        public void Write(BinaryWriter writer/*System.IO.FileStream fileStream*/)
        {
            writer.Write(Name);
            writer.Write(Region);
            writer.Write(City);
            writer.Write(Build);
            writer.Write(Person);
            writer.Write(TelN);
            writer.Write(Polling);

            writer.Write(Buildings.Count);
            for (int i = 0; i < Buildings.Count; i++)
                Buildings[i].Write(writer);
        }

        public void Read(BinaryReader reader/*System.IO.FileStream fileStream*/)
        {
            Name = reader.ReadString();
            Region = reader.ReadString();
            City = reader.ReadString();
            Build = reader.ReadString();
            Person = reader.ReadString();
            TelN = reader.ReadString();
            Polling = reader.ReadBoolean();

            int NumberOfBuilding = reader.ReadInt32(); ;//Number of building
            
            Buildings.Clear();

            for (int k = 0; k < NumberOfBuilding; k++)
            {
                Building b = new Building("", this);
                b.Read(reader);
                Buildings.Add(b);
            }
        }

       



        private void OnCommandScillaObjectExec(object parameter)
        {
            if (parameter.ToString() == "cmdAddBuilding")
            {
                Buildings.Add(new Building(String.Format("Building {0:d2}", Buildings.Count + 1), this));
            }
           
            if (parameter.ToString() == "cmdOpenObjectConf")
            {
                Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();

               //-{} dl1.Title = "Open Scilla Object Configuration";
                dl1.Title = " Открыть конфигурацию объекта Scilla...";
                dl1.DefaultExt = ".cnf";
       //-{}         dl1.Filter = "Scilla config documents (.cnf)|*.cnf";
                dl1.Filter = "Документ конфигурации Scilla (.cnf)|*.cnf";
                Nullable<bool> result = dl1.ShowDialog();

                if (result == true)
                {
                    if (File.Exists(dl1.FileName))
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(dl1.FileName, FileMode.Open)))
                        {
                            Read(reader);
                        }
                        App.myApp.sBuilding = null;
                        App.myApp.sFloor = null;
                        App.myApp.sModule = null;
                        App.myApp.Plan.Draw();
                    }
                }
            }

            if (parameter.ToString() == "cmdSaveObjectConf")
            {
                Microsoft.Win32.SaveFileDialog dl1 = new Microsoft.Win32.SaveFileDialog();
                dl1.Title = "Save Scilla Object Configuration As";
                dl1.FileName = App.myApp.VMS.ScillaObjects[0].Name;
                dl1.DefaultExt = ".cnf";
                dl1.Filter = "Scilla config documents (.cnf)|*.cnf";
                Nullable<bool> result = dl1.ShowDialog();

                if (result == true)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(dl1.FileName, FileMode.Create)))
                    {
                            Write(writer);
                    }
                }
            }

            if (parameter.ToString() == "cmdNewObjectConf")
            {
                App.myApp.VMS.ScillaObjects.Add(new ScillaObject("Scilla Object"));
                App.myApp.VMS.ScillaObjects.Remove(this);
                App.myApp.sBuilding = null;
                App.myApp.sFloor = null;
                App.myApp.sModule = null;
                App.myApp.Plan.Draw();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class Building : INotifyPropertyChanged
    {
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); } }

        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; NotifyPropertyChanged("IsExpanded"); } }


        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

        private bool _Polling = true;
        public bool Polling { get { return _Polling; } set { _Polling = value; NotifyPropertyChanged("Polling"); } }

        private ScillaObject Owner;
        public ObservableCollection<Floor> Floors { get; set; }
        public ICommand OnCommandBuilding { get; set; }
        public Building(String name, ScillaObject owner)
        {
            Owner = owner;
            Name = name;
            Floors = new ObservableCollection<Floor>();
            OnCommandBuilding = new CommandBuilding(OnCommandBuildingExec);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);

            writer.Write(Floors.Count);
            for (int i = 0; i < Floors.Count; i++)
                Floors[i].Write(writer);
        }

        public void Read(BinaryReader reader)
        {
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();

            int NumberOfFloor = reader.ReadInt32(); ;//Number of building
            Floors.Clear();

            for (int k = 0; k < NumberOfFloor; k++)
            {
                Floor b = new Floor("", this);
                b.Read(reader);
                Floors.Add(b);
            }
        }
        private void OnCommandBuildingExec(object parameter)
        {
            if (parameter.ToString() == "cmdAddFloor")
            {
                Floors.Add(new Floor(String.Format("Floor {0:d2}", Floors.Count + 1), this));

            }
            if (parameter.ToString() == "cmdInsertBuilding")
            {
                int j = Owner.Buildings.IndexOf(this);
                if (j != -1)
                    Owner.Buildings.Insert(j, new Building((String.Format("Building {0:d2}", Owner.Buildings.Count + 1)), Owner));
            }

            if (parameter.ToString() == "cmdMoveUpBuilding")
            {
                int j = Owner.Buildings.IndexOf(this);
                if (j != -1 && j > 0)
                    Owner.Buildings.Move(j, j - 1);
            }
            if (parameter.ToString() == "cmdMoveDownBuilding")
            {
                int j = Owner.Buildings.IndexOf(this);
                if (j != -1 && j < Owner.Buildings.Count - 1)
                    Owner.Buildings.Move(j, j + 1);
            }

            if (parameter.ToString() == "cmdDeleteBuilding")
            {
                Owner.Buildings.Remove(this);
            }
          
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Floor : INotifyPropertyChanged
    {

        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); } }

        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; NotifyPropertyChanged("IsExpanded"); } }

        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

        private bool _Polling = true;
        public bool Polling { get { return _Polling; } set { _Polling = value; NotifyPropertyChanged("Polling"); } }

        public Building Owner;
        
        //BitmapImage b = new BitmapImage(new Uri(@filename, UriKind.Absolute));//@filename
        public Image imageFloorPlan = null;
        public BitmapImage bmFloorPlan = null;
        public double imageHeight;
        public double imageWidth;

        private double _FloorWidth = 10.0;
        public double FloorWidth { get { return _FloorWidth; } set { _FloorWidth = value; NotifyPropertyChanged("FloorWidth"); } }

        private double _FloorHeight = 10.0;
        public double FloorHeight { get { return _FloorHeight; } set { _FloorHeight = value; NotifyPropertyChanged("FloorHeight"); } }

        private double _FloorHorizontalShift = 0.0;
        public double FloorHorizontalShift { get { return _FloorHorizontalShift; } set { _FloorHorizontalShift = value; NotifyPropertyChanged("FloorHorizontalShift"); } }

        private double _FloorVerticalShift = 0.0;
        public double FloorVerticalShift { get { return _FloorVerticalShift; } set { _FloorVerticalShift = value; NotifyPropertyChanged("FloorVerticalShift"); } }

        //для переключения
        public double Zoom = 1;
        public double ScrollX = 0;
        public double ScrollY = 0;

        //byte[] xByte;



        public ObservableCollection<Module> Modules { get; set; }
        public ICommand OnCommandFloor { get; set; }
        public Floor(string name, Building owner)
        {
            Owner = owner;
            Name = name;
            Modules = new ObservableCollection<Module>();
            OnCommandFloor = new CommandFloor(OnCommandFlorExec);
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);

            writer.Write(FloorWidth);
            writer.Write(FloorHeight);
            writer.Write(FloorHorizontalShift);
            writer.Write(FloorVerticalShift);

            /*********************************************************************************************************************************/
            if (bmFloorPlan != null)
            {
                writer.Write(true);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmFloorPlan));
                MemoryStream stream = new MemoryStream();
                encoder.Save(stream);
                byte[] bytes = stream.ToArray();
                writer.Write(bytes.Length);
                writer.Write(bytes, 0, (Int32)bytes.Length);
            }
            else
                writer.Write(false);
            /*********************************************************************************************************************************/
            writer.Write(Modules.Count);
            for (int i = 0; i < Modules.Count; i++)
                Modules[i].Write(writer);

        }

        public void Read(BinaryReader reader)
        {
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();

            FloorWidth = reader.ReadDouble();
            FloorHeight = reader.ReadDouble();
            FloorHorizontalShift = reader.ReadDouble();
            FloorVerticalShift = reader.ReadDouble();

            /************************************************************************************************************************************/
            bool planOk = reader.ReadBoolean();
            if (planOk == true)
            {
                int lenght = reader.ReadInt32();
                byte[] bytes = reader.ReadBytes(lenght);
                //byte[] bytes = File.ReadAllBytes(@"E:\Image.jpg");

                bmFloorPlan = new BitmapImage();

                bmFloorPlan.BeginInit();
                bmFloorPlan.StreamSource = new MemoryStream(bytes);
                bmFloorPlan.EndInit();

                imageHeight = bmFloorPlan.Height;
                imageWidth = bmFloorPlan.Width;

                imageFloorPlan = new Image();
                imageFloorPlan.Source = bmFloorPlan;
            }
            /************************************************************************************************************************************/

            int NumberOfModule = reader.ReadInt32(); ;//Number of building
            Modules.Clear();

            for (int k = 0; k < NumberOfModule; k++)
            {
                Module b = new Module("", this);
                b.Read(reader);
                Modules.Add(b);
            }
        }

        private void OnCommandFlorExec(object parameter)
        {
            if (parameter.ToString() == "cmdAddModule")
            {
                Modules.Add(new Module((String.Format("Module {0:d3}", Modules.Count + 1)), this));
                App.myApp.Plan.Draw();
            }
           
            if (parameter.ToString() == "cmdInsertFloor")
            {
                int j = Owner.Floors.IndexOf(this);
                if (j != -1)
                    Owner.Floors.Insert(j, new Floor((String.Format("Floor {0:d2}", Owner.Floors.Count + 1)), Owner));
            }
            
            if (parameter.ToString() == "cmdMoveUpFloor")
            {
                int j = Owner.Floors.IndexOf(this);
                if (j != -1 && j > 0)
                    Owner.Floors.Move(j, j - 1);
            }

            if (parameter.ToString() == "cmdMoveDownDeleteFloor")
            {
                int j = Owner.Floors.IndexOf(this);
                if (j != -1 && j < Owner.Floors.Count - 1)
                    Owner.Floors.Move(j, j + 1);
            }

            if (parameter.ToString() == "cmdDeleteFloor")
            {
                Owner.Floors.Remove(this);
                //MessageBox.Show("cmdDeleteFloor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum MdState {MS_NotSet = 0, MS_Ok = 1, MS_Warning = 2, MS_Alarm = 3, MS_ConnectError = 4, MS_NotPolled = 5 };//Используется для отображения и Module и ModuleDev в TreeView...


    public class ModuleStageToColorString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = "Silver";
            switch ((MdState)value)
            {
                case MdState.MS_NotSet: str = "Silver";
                    break;
                case MdState.MS_Ok: str = "Green";
                    break;
                case MdState.MS_Warning:
                    str = "Yellow";
                    break;
                case MdState.MS_Alarm:
                    str = "Red";
                    break;
                case MdState.MS_ConnectError:
                    str = "Black";
                    break;
                case MdState.MS_NotPolled:
                    str = "White";
                    break;
            }
                return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ModuleConnectionOkToSumbolString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? "\uE8CE" : "\uE8CD" ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ModuleConnectionOkToSumbolStringColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? "ForestGreen" : "Red";// "LimeGreen"
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Module : INotifyPropertyChanged
    {

        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); } }

        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; NotifyPropertyChanged("IsExpanded"); } }


        public Floor Owner;

        public byte BtnFireStage = 178;//178 - отжата, 177 - нажата
        public byte BtnSecurityStage = 178;//178 - отжата, 177 - нажата

        private MdState _moduleState = MdState.MS_NotSet;
        public MdState moduleState { get { return _moduleState; } set { _moduleState = value; NotifyPropertyChanged("moduleState"); } }

        private bool _moduleConnectionOk = true;// 
        public bool moduleConnectionOk { get { return _moduleConnectionOk; } set { _moduleConnectionOk = value; NotifyPropertyChanged("moduleConnectionOk"); } }

        

        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

        private bool _Polling = true;
        public bool Polling { get { return _Polling; } set { _Polling = value; NotifyPropertyChanged("Polling"); } }

        private double _PositionX = 30;
        public double PositionX { get { return _PositionX; } set { _PositionX = value; NotifyPropertyChanged("PositionX"); } }

        private double _PositionY = 30;
        public double PositionY { get { return _PositionY; } set { _PositionY = value; NotifyPropertyChanged("PositionY"); } }


        private byte _DESS_ModuleIpAddress_01 = 0xC0;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte DESS_ModuleIpAddress_01
        { get { return _DESS_ModuleIpAddress_01; } set { _DESS_ModuleIpAddress_01 = value; NotifyPropertyChanged("DESS_ModuleIpAddress_01"); } }
        
        private byte _DESS_ModuleIpAddress_02 = 0xA8;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte DESS_ModuleIpAddress_02
        { get { return _DESS_ModuleIpAddress_02; } set { _DESS_ModuleIpAddress_02 = value; NotifyPropertyChanged("DESS_ModuleIpAddress_02"); } }
        
        private byte _DESS_ModuleIpAddress_03 = 0x01;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte DESS_ModuleIpAddress_03
        { get { return _DESS_ModuleIpAddress_03; } set { _DESS_ModuleIpAddress_03 = value; NotifyPropertyChanged("DESS_ModuleIpAddress_03"); } }
        
        private byte _DESS_ModuleIpAddress_04 = 0x64;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte DESS_ModuleIpAddress_04
        { get { return _DESS_ModuleIpAddress_04; } set { _DESS_ModuleIpAddress_04 = value; NotifyPropertyChanged("DESS_ModuleIpAddress_04"); } }


        public UInt32 getIpAddress()
        {
            return ( (((UInt32)_DESS_ModuleIpAddress_01)<<24) + (((UInt32)_DESS_ModuleIpAddress_02) << 16) + (((UInt32)_DESS_ModuleIpAddress_03) << 8) + (UInt32)_DESS_ModuleIpAddress_04);
        }


        private byte _DESS_ModuleMacAddress_01 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_01
        { get { return _DESS_ModuleMacAddress_01; } set { _DESS_ModuleMacAddress_01 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_01"); } }
        private byte _DESS_ModuleMacAddress_02 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_02
        { get { return _DESS_ModuleMacAddress_02; } set { _DESS_ModuleMacAddress_02 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_02"); } }
        private byte _DESS_ModuleMacAddress_03 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_03
        { get { return _DESS_ModuleMacAddress_03; } set { _DESS_ModuleMacAddress_03 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_03"); } }
        private byte _DESS_ModuleMacAddress_04 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_04
        { get { return _DESS_ModuleMacAddress_04; } set { _DESS_ModuleMacAddress_04 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_04"); } }
        private byte _DESS_ModuleMacAddress_05 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_05
        { get { return _DESS_ModuleMacAddress_05; } set { _DESS_ModuleMacAddress_05 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_05"); } }
        private byte _DESS_ModuleMacAddress_06 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte DESS_ModuleMacAddress_06
        { get { return _DESS_ModuleMacAddress_06; } set { _DESS_ModuleMacAddress_06 = value; NotifyPropertyChanged("DESS_ModuleMacAddress_06"); } }

        ///
        private int _DESS_ModulePort = 0x913;// 
        public int DESS_ModulePort { get { return _DESS_ModulePort; } set { _DESS_ModulePort = value; NotifyPropertyChanged("DESS_ModulePort"); } }

        private byte _DESS_ServerIpAddress_01 = 192;// { 192, 168, 1, 10 };
        public byte DESS_ServerIpAddress_01
        { get { return _DESS_ServerIpAddress_01; } set { _DESS_ServerIpAddress_01 = value; NotifyPropertyChanged("DESS_ServerIpAddress_01"); } }
        private byte _DESS_ServerIpAddress_02 = 168;// { 192, 168, 1, 10 };
        public byte DESS_ServerIpAddress_02
        { get { return _DESS_ServerIpAddress_02; } set { _DESS_ServerIpAddress_02 = value; NotifyPropertyChanged("DESS_ServerIpAddress_02"); } }
        private byte _DESS_ServerIpAddress_03 = 1;// { 192, 168, 1, 10 };
        public byte DESS_ServerIpAddress_03
        { get { return _DESS_ServerIpAddress_03; } set { _DESS_ServerIpAddress_03 = value; NotifyPropertyChanged("DESS_ServerIpAddress_03"); } }
        private byte _DESS_ServerIpAddress_04 = 10;// { 192, 168, 1, 10 };
        public byte DESS_ServerIpAddress_04
        { get { return _DESS_ServerIpAddress_04; } set { _DESS_ServerIpAddress_04 = value; NotifyPropertyChanged("DESS_ServerIpAddress_04"); } }

        private int _DESS_ServerPort = 2424;// 
        public int DESS_ServerPort { get { return _DESS_ServerPort; } set { _DESS_ServerPort = value; NotifyPropertyChanged("DESS_ServerPort"); } }

        private string _DESS_Tel = "012345678901";// 
        public string DESS_Tel { get { return _DESS_Tel; } set { _DESS_Tel = value; NotifyPropertyChanged("DESS_Tel"); } }
        
        private bool _DESS_TelAvailable = false;// 
        public bool DESS_TelAvailable { get { return _DESS_TelAvailable; } set { _DESS_TelAvailable = value; NotifyPropertyChanged("DESS_TelAvailable"); } }

        public uint sended_MASG_and_INFO_count = 0;
        public ObservableCollection<ModuleDev> ModuleDevs { get; set; }
        public ICommand OnCommandModule { get; set; }
        public Module(string name, Floor owner)
        {
            Owner = owner;
            Name = name;
            ModuleDevs = new ObservableCollection<ModuleDev>();
            OnCommandModule = new CommandModule(OnCommandModuleExec);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);
            writer.Write(PositionX);
            writer.Write(PositionY);

            writer.Write(DESS_ModuleIpAddress_01);
            writer.Write(DESS_ModuleIpAddress_02);
            writer.Write(DESS_ModuleIpAddress_03);
            writer.Write(DESS_ModuleIpAddress_04);

            writer.Write(DESS_ModuleMacAddress_01);
            writer.Write(DESS_ModuleMacAddress_02);
            writer.Write(DESS_ModuleMacAddress_03);
            writer.Write(DESS_ModuleMacAddress_04);
            writer.Write(DESS_ModuleMacAddress_05);
            writer.Write(DESS_ModuleMacAddress_06);

            writer.Write(DESS_ModulePort);

            writer.Write(DESS_ServerIpAddress_01);
            writer.Write(DESS_ServerIpAddress_02);
            writer.Write(DESS_ServerIpAddress_03);
            writer.Write(DESS_ServerIpAddress_04);

            writer.Write(DESS_ServerPort);

            writer.Write(DESS_TelAvailable);
            writer.Write(DESS_Tel);

            writer.Write(ModuleDevs.Count);
            for (int i = 0; i < ModuleDevs.Count; i++)
            {
                writer.Write(ModuleDevs[i].ModuleDevType);
                ModuleDevs[i].Write(writer);
            }
        }
        public void Read(BinaryReader reader)
        {
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();
            PositionX = reader.ReadDouble();
            PositionY = reader.ReadDouble();

            DESS_ModuleIpAddress_01 = reader.ReadByte();
            DESS_ModuleIpAddress_02 = reader.ReadByte();
            DESS_ModuleIpAddress_03 = reader.ReadByte();
            DESS_ModuleIpAddress_04 = reader.ReadByte();

            DESS_ModuleMacAddress_01 = reader.ReadByte();
            DESS_ModuleMacAddress_02 = reader.ReadByte();
            DESS_ModuleMacAddress_03 = reader.ReadByte();
            DESS_ModuleMacAddress_04 = reader.ReadByte();
            DESS_ModuleMacAddress_05 = reader.ReadByte();
            DESS_ModuleMacAddress_06 = reader.ReadByte();

            DESS_ModulePort = reader.ReadInt32();

            DESS_ServerIpAddress_01 = reader.ReadByte();
            DESS_ServerIpAddress_02 = reader.ReadByte();
            DESS_ServerIpAddress_03 = reader.ReadByte();
            DESS_ServerIpAddress_04 = reader.ReadByte();

            _DESS_ServerPort = reader.ReadInt32();

            DESS_TelAvailable= reader.ReadBoolean();
            DESS_Tel= reader.ReadString();

            int NumberOfModulesDev = reader.ReadInt32(); ;//Number of building
            ModuleDevs.Clear();

            int ModuleDevType;
            for (int k = 0; k < NumberOfModulesDev; k++)
            {
                ModuleDevType = reader.ReadInt32();
                switch(ModuleDevType)
                {
                    case 1:
                        MultiSensor m = new MultiSensor("", this);
                        m.Read(reader);
                        ModuleDevs.Add(m);
                        break;
                    case 2:
                        RelayUnit r = new RelayUnit("", this);
                        r.Read(reader);
                        ModuleDevs.Add(r);
                        break;
                    case 3:
                        BusTag b = new BusTag("", this);
                        b.Read(reader);
                        ModuleDevs.Add(b);
                        break;
                }
            }
        }
        private void OnCommandModuleExec(object parameter)
        {

            if (parameter.ToString() == "cmdAddMultiSensor")
            {
                for(int i=0; i< ((Keyboard.Modifiers == ModifierKeys.Control) ? 10 : 1); i++)
                    ModuleDevs.Add(new MultiSensor((String.Format("MultiSensor {0:d3}", CalcCount(1) + 1)), this));
                App.myApp.Plan.Draw();
            }

            if (parameter.ToString() == "cmdAddRelayUnit")
            {
                ModuleDevs.Add(new RelayUnit((String.Format("RelayUnit {0:d3}", CalcCount(2) + 1)), this));
                App.myApp.Plan.Draw();
            }

            if (parameter.ToString() == "cmdAddBusTag")
            {
                ModuleDevs.Add(new BusTag((String.Format("BusTag {0:d3}", CalcCount(3) + 1)), this));
                App.myApp.Plan.Draw();
            }
            
            if (parameter.ToString() == "cmdInsertModule")
            {
                int j = Owner.Modules.IndexOf(this);
                if (j != -1)
                    Owner.Modules.Insert(j, new Module((String.Format("Module {0:d3}", Owner.Modules.Count + 1)), Owner));
                App.myApp.Plan.Draw();
            }
           
            if (parameter.ToString() == "cmdMoveUpModule")
            {
                int j = Owner.Modules.IndexOf(this);
                if (j != -1 && j > 0)
                    Owner.Modules.Move(j, j - 1);
            }

            if (parameter.ToString() == "cmdMoveDownModule")
            {
                int j = Owner.Modules.IndexOf(this);
                if (j != -1 && j < Owner.Modules.Count - 1)
                    Owner.Modules.Move(j, j + 1);
            }

            if (parameter.ToString() == "cmdDeleteModule")
            {
                Owner.Modules.Remove(this);
                App.myApp.Plan.Draw();
            }
        }
        
        private int CalcCount(int type)
        {
            int count = 0;
            for (int i = 0; i < ModuleDevs.Count; i++)
                if (ModuleDevs[i].ModuleDevType == type)
                    count++;
            return count;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /******************************************************************************************/
      

        static UdpClient appListener=null;
        public Task taskUDP_Listner = null;
        public bool stopListner = false;

        public static byte[] ResivedBytes = new byte[2048];
       public static int iResivedBytes = 0;
       public static bool FlagResivedBytesFull = false;

        static int iProcessedBytes = 0;
        

      
        /*{+*/
        public void UDP_listening_PI1(Module module)
        {
            if (appListener == null)
                appListener = new UdpClient(App.myApp.serverListenerPort);//2424

            string strAddress = DESS_ModuleIpAddress_01.ToString() + "." + DESS_ModuleIpAddress_02.ToString() + "." + DESS_ModuleIpAddress_03.ToString() + "." + DESS_ModuleIpAddress_04.ToString();
            IPAddress localAddr = IPAddress.Parse(strAddress); //("192.168.1.200")
            IPEndPoint groupEP = new IPEndPoint(localAddr, App.myApp.serverListenerPort); //IPAddress.Any


            //String str = "";
            //String str1, str2;

        //    module.ListenerOk = true;
       //     module.moduleState = ModuleState.MS_Ok;

            try
            {
                while (stopListner == false)
                {
                    
                    byte[] bytes = appListener.Receive(ref groupEP);//Console.WriteLine("Waiting for broadcast");

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

                    //-{} Recive();
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine(exc);
            }
            finally
            {
                appListener.Close();
            }

        }
        /*}*/

        /*+{*/
        static int stage = 0;
        static int stageCmd = 0;

        static int PackageNumber = 0;
        static int CmdLen = 0;
        static byte[] Cmd = new byte[4];
        static int CmdId;
        static byte ParamId;

        static byte TmpByte;
        static UInt32 TmpIpAddress;
        static UInt32[] MASG_RawData = new UInt32[248];
        /*}*/
        
        

/**/
        public void SendCmd(short CmdLen, String STX, String CMD, int ParamID, byte[] ParamData, int nButes,bool Broadcast)
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
            string strAddress = DESS_ModuleIpAddress_01.ToString() + "." + DESS_ModuleIpAddress_02.ToString() + "." + DESS_ModuleIpAddress_03.ToString() + "." + DESS_ModuleIpAddress_04.ToString();


            IPAddress remoteIPAddress;
            if (Broadcast==true)
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


       /* */


    }
    /****************************************************************************************************************************************************************************************************************************************/
    
    public abstract class ModuleDev : INotifyPropertyChanged
    {


        private MdState _moduleDevState = MdState.MS_NotSet;
        public MdState moduleDevState { get { return _moduleDevState; } set { _moduleDevState = value; NotifyPropertyChanged("moduleDevState"); } }

        private int _moduleDevErrorCount = 0;
        public int moduleDevErrorCount { get { return _moduleDevErrorCount; } set { _moduleDevErrorCount = value; NotifyPropertyChanged("moduleDevErrorCount"); } }


        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); } }


        private bool _IsExpanded = true;
        public bool IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; NotifyPropertyChanged("IsExpanded"); } }

        public ModuleDev()
        {
            OnCommandMultiSensor = new CommandModule(OnCommandMultiSensorExec);
        }
        public ICommand OnCommandMultiSensor { get; set; }

        private SolidColorBrush _mdSolidColorBrush=Brushes.Black;
        public SolidColorBrush mdSolidColorBrush { get { return _mdSolidColorBrush; } set { _mdSolidColorBrush = value; NotifyPropertyChanged("mdSolidColorBrush");}}


        private string _strIcon; 
        public string strIcon { get { return _strIcon; } set { _strIcon = value; NotifyPropertyChanged("strIcon"); } }

        private int _SlaveAddress = 247;
        public int SlaveAddress { get { return _SlaveAddress; } set { _SlaveAddress = value; NotifyPropertyChanged("SlaveAddress"); } }

        public int ModuleDevType = 0; //0-not defined 1 -MultiSensor 2 - RelayUnit

       
        public Module Owner;

        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

        private bool _Polling = true;
        public bool Polling { get { return _Polling; } set { _Polling = value; NotifyPropertyChanged("Polling"); } }

        private double _PositionX = 30;
        public double PositionX { get { return _PositionX; } set { _PositionX = value; NotifyPropertyChanged("PositionX"); } }

        private double _PositionY = 30;
        public double PositionY { get { return _PositionY; } set { _PositionY = value; NotifyPropertyChanged("PositionY"); } }



        public virtual void Write(BinaryWriter writer) { }


        private void OnCommandMultiSensorExec(object parameter)
        {
            if (parameter.ToString() == "cmdInsertMultiSensor")
            {
                int j = Owner.ModuleDevs.IndexOf(this);
                if (j != -1)
                    Owner.ModuleDevs.Insert(j, new MultiSensor((String.Format("MultiSensor {0:d3}", CalcCount(1) + 1)), Owner));
                App.myApp.Plan.Draw();
            }
            if (parameter.ToString() == "cmdInsertRelayUnit")
            {
                int j = Owner.ModuleDevs.IndexOf(this);
                if (j != -1)
                    Owner.ModuleDevs.Insert((int)j, new RelayUnit((String.Format("RelayModule {0:d3}", CalcCount(2) + 1)), Owner));
                App.myApp.Plan.Draw();
            }
            if (parameter.ToString() == "cmdInsertBusTag")
            {
                int? j = Owner.ModuleDevs.IndexOf(this);
                if (j != -1)
                    Owner.ModuleDevs.Insert((int)j, new BusTag((String.Format("MultiSensor {0:d3}", CalcCount(3) + 1)), Owner));
                App.myApp.Plan.Draw();
            }


            if (parameter.ToString() == "cmdMoveUpModuleDev")
            {
                int j = Owner.ModuleDevs.IndexOf(this);
                if (j != -1 && j > 0)
                    Owner.ModuleDevs.Move(j, j - 1);
            }
            if (parameter.ToString() == "cmdMoveDownModuleDev")
            {
                int j = Owner.ModuleDevs.IndexOf(this); 
                if (j != -1 && j < Owner.ModuleDevs.Count - 1)
                    Owner.ModuleDevs.Move(j, j + 1);
            }
            if (parameter.ToString() == "cmdDeleteModuleDev")
            {
                Owner.ModuleDevs.Remove(this);
                App.myApp.Plan.Draw();
            }
            if (parameter.ToString() == "cmdInfoModuleDev")//+{
            {
                WindowInfoDevice wndinfoDevice = new WindowInfoDevice();
                if(wndinfoDevice.ShowDialog()==true)
                {

                }

            }//}
        }
        private int CalcCount(int type)
        {
            int count = 0;
            for (int i = 0; i < Owner.ModuleDevs.Count; i++)
                if (Owner.ModuleDevs[i].ModuleDevType == type)
                    count++;
            return count;
        }
        /*
        private int? SelectedIndex()
        {
            int? count = null;
            for (int i = 0; i < Owner.ModuleDevs.Count; i++)
                if (Owner.ModuleDevs[i].IsSelected == true)
                {
                    count = i; return count; //break;
                }
            return count;
        }
        */
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    /****************************************************************************************************************************************************************************************************************************************/

    //public enum MultiSensorState { MS_NotSet = 0, MS_Ok = 1, MS_Warning = 2, MS_Alarm = 3 };

    public class MultiSensor : ModuleDev

    {
        public UInt16[] InputReg = new UInt16[24];
        /*
        public UInt16 InputReg_06 = 0;
        public UInt16 InputReg_07 = 0;
        public UInt16 InputReg_08 = 0;
        */


        private bool _InclAnl_S_o = true;
        public bool InclAnl_S_o { get { return _InclAnl_S_o; } set { _InclAnl_S_o = value; NotifyPropertyChanged("InclAnl_S_o"); } }

        private bool _InclAnl_T_a = true;
        public bool InclAnl_T_a { get { return _InclAnl_T_a; } set { _InclAnl_T_a = value; NotifyPropertyChanged("InclAnl_T_a"); } }

        private bool _InclAnl_T_d = true;
        public bool InclAnl_T_d { get { return _InclAnl_T_d; } set { _InclAnl_T_d = value; NotifyPropertyChanged("InclAnl_T_d"); } }

        private bool _InclAnl_S_e = true;
        public bool InclAnl_S_e { get { return _InclAnl_S_e; } set { _InclAnl_S_e = value; NotifyPropertyChanged("InclAnl_S_e"); } }

        private bool _InclAnl_Flm = true;
        public bool InclAnl_Flm { get { return _InclAnl_Flm; } set { _InclAnl_Flm = value; NotifyPropertyChanged("InclAnl_Flm"); } }

        private bool _InclAnl_CO = true;
        public bool InclAnl_CO { get { return _InclAnl_CO; } set { _InclAnl_CO = value; NotifyPropertyChanged("InclAnl_CO"); } }

        private bool _InclAnl_VOC = true;
        public bool InclAnl_VOC { get { return _InclAnl_VOC; } set { _InclAnl_VOC = value; NotifyPropertyChanged("InclAnl_VOC"); } }


        private UInt16 _ThWarnSmogO=1000;
        public UInt16 ThWarnSmogO { get { return _ThWarnSmogO; } set { _ThWarnSmogO = value; NotifyPropertyChanged("ThWarnSmogO"); } }
        private UInt16 _ThAlrmSmogO=1500;
        public UInt16 ThAlrmSmogO { get { return _ThAlrmSmogO; } set { _ThAlrmSmogO = value; NotifyPropertyChanged("ThAlrmSmogO"); } }

        private UInt16 _ThSmogORefU=0;
        public UInt16 ThSmogORefU { get { return _ThSmogORefU; } set { _ThSmogORefU = value; NotifyPropertyChanged("ThSmogORefU"); } }
        private UInt16 _ThSmogO_LED=0;
        public UInt16 ThSmogO_LED { get { return _ThSmogO_LED; } set { _ThSmogO_LED = value; NotifyPropertyChanged("ThSmogO_LED"); } }

        private UInt16 _ThWarnTempA=2800;
        public UInt16 ThWarnTempA { get { return _ThWarnTempA; } set { _ThWarnTempA = value; NotifyPropertyChanged("ThWarnTempA"); } }
        private UInt16 _ThAlrmTempA=3000;
        public UInt16 ThAlrmTempA { get { return _ThAlrmTempA; } set { _ThAlrmTempA = value; NotifyPropertyChanged("ThAlrmTempA"); } }

        private UInt16 _ThWarnTempD=60;
        public UInt16 ThWarnTempD { get { return _ThWarnTempD; } set { _ThWarnTempD = value; NotifyPropertyChanged("ThWarnTempD"); } }
        private UInt16 _ThAlrmTempD=80;
        public UInt16 ThAlrmTempD { get { return _ThAlrmTempD; } set { _ThAlrmTempD = value; NotifyPropertyChanged("ThAlrmTempD"); } }

        private UInt16 _ThWarnSmogE=2400;
        public UInt16 ThWarnSmogE { get { return _ThWarnSmogE; } set { _ThWarnSmogE = value; NotifyPropertyChanged("ThWarnSmogE"); } }
        private UInt16 _ThAlrmSmogE=2500;
        public UInt16 ThAlrmSmogE { get { return _ThAlrmSmogE; } set { _ThAlrmSmogE = value; NotifyPropertyChanged(""); } }

        private UInt16 _ThWarnFlamA=2900;
        public UInt16 ThWarnFlamA { get { return _ThWarnFlamA; } set { _ThWarnFlamA = value; NotifyPropertyChanged("ThWarnFlamA"); } }
        private UInt16 _ThAlrmFlamA=2800;
        public UInt16 ThAlrmFlamA { get { return _ThAlrmFlamA; } set { _ThAlrmFlamA = value; NotifyPropertyChanged("ThAlrmFlamA"); } }

        private UInt16 _ThWarnFlamD=1800;
        public UInt16 ThWarnFlamD { get { return _ThWarnFlamD; } set { _ThWarnFlamD = value; NotifyPropertyChanged("ThWarnFlamD"); } }
        private UInt16 _ThAlrmFlamD=2000;
        public UInt16 ThAlrmFlamD { get { return _ThAlrmFlamD; } set { _ThAlrmFlamD = value; NotifyPropertyChanged("ThAlrmFlamD"); } }

        private UInt16 _ThWarnCO=2000;
        public UInt16 ThWarnCO { get { return _ThWarnCO; } set { _ThWarnCO = value; NotifyPropertyChanged("ThWarnCO"); } }
        private UInt16 _ThAlrmCO=4000;
        public UInt16 ThAlrmCO { get { return _ThAlrmCO; } set { _ThAlrmCO = value; NotifyPropertyChanged("ThAlrmCO"); } }

        private UInt16 _ThWarnVOC=2000;
        public UInt16 ThWarnVOC { get { return _ThWarnVOC; } set { _ThWarnVOC = value; NotifyPropertyChanged("ThWarnVOC"); } }
        private UInt16 _ThAlrmVOC=4000;
        public UInt16 ThAlrmVOC { get { return _ThAlrmVOC; } set { _ThAlrmVOC = value; NotifyPropertyChanged("ThAlrmVOC"); } }



        public MultiSensor(string name, Module owner)
        {
            strIcon = "\xE93E";
            mdSolidColorBrush = Brushes.Green;
            ModuleDevType = 1;
            Owner = owner;
            Name = name;
          
        }

       
        public override void Write(BinaryWriter writer)
        {
            writer.Write(ModuleDevType);
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(SlaveAddress);

            writer.Write(InclAnl_S_o);
            writer.Write(InclAnl_T_a);
            writer.Write(InclAnl_T_d);
            writer.Write(InclAnl_S_e);
            writer.Write(InclAnl_Flm);
            writer.Write(InclAnl_CO);
            writer.Write(InclAnl_VOC);

            writer.Write(ThWarnSmogO);
            writer.Write(ThAlrmSmogO);
            writer.Write(ThSmogORefU);
            writer.Write(ThSmogO_LED);

            writer.Write(ThWarnTempA);
            writer.Write(ThAlrmTempA);

            writer.Write(ThWarnTempD);
            writer.Write(ThAlrmTempD);

            writer.Write(ThWarnSmogE);
            writer.Write(ThAlrmSmogE);

            writer.Write(ThWarnFlamA);
            writer.Write(ThAlrmFlamA);

            writer.Write(ThWarnFlamD);
            writer.Write(ThAlrmFlamD);

            writer.Write(ThWarnCO);
            writer.Write(ThAlrmCO);

            writer.Write(ThWarnVOC);
            writer.Write(ThAlrmVOC);
        }
        public void Read(BinaryReader reader)
        {
            ModuleDevType = reader.ReadInt32();
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();
            PositionX = reader.ReadDouble();
            PositionY = reader.ReadDouble();
            SlaveAddress = reader.ReadInt32();

            InclAnl_S_o = reader.ReadBoolean();
            InclAnl_T_a= reader.ReadBoolean();
            InclAnl_T_d= reader.ReadBoolean();
            InclAnl_S_e= reader.ReadBoolean();
            InclAnl_Flm= reader.ReadBoolean();
            InclAnl_CO= reader.ReadBoolean();
            InclAnl_VOC= reader.ReadBoolean();

            ThWarnSmogO = reader.ReadUInt16();
            ThAlrmSmogO = reader.ReadUInt16();
            ThSmogORefU = reader.ReadUInt16();
            ThSmogO_LED = reader.ReadUInt16();

            ThWarnTempA = reader.ReadUInt16();
            ThAlrmTempA = reader.ReadUInt16();

            ThWarnTempD = reader.ReadUInt16();
            ThAlrmTempD = reader.ReadUInt16();

            ThWarnSmogE = reader.ReadUInt16();
            ThAlrmSmogE = reader.ReadUInt16();

            ThWarnFlamA = reader.ReadUInt16();
            ThAlrmFlamA = reader.ReadUInt16();

            ThWarnFlamD = reader.ReadUInt16();
            ThAlrmFlamD = reader.ReadUInt16();

            ThWarnCO = reader.ReadUInt16();
            ThAlrmCO = reader.ReadUInt16();

            ThWarnVOC = reader.ReadUInt16();
            ThAlrmVOC = reader.ReadUInt16();

        }
       
        /*
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        */
    }

    /****************************************************************************************************************************************************************************************************************************************/
    public class RelayUnit : ModuleDev
    {
        public RelayUnit(string name, Module owner)
        {
            strIcon = "\xE8F9";
            mdSolidColorBrush = Brushes.MediumBlue;
            ModuleDevType = 2;
            Owner = owner;
            Name = name;
            //OnCommandMultiSensor = new CommandModule(OnCommandMultiSensorExec);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(ModuleDevType);
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(SlaveAddress);
        }

        public void Read(BinaryReader reader)
        {
            ModuleDevType = reader.ReadInt32();
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();
            PositionX = reader.ReadDouble();
            PositionY = reader.ReadDouble();
            SlaveAddress = reader.ReadInt32();
        }
    }

    /****************************************************************************************************************************************************************************************************************************************/
    public class BusTag : ModuleDev
    {
        public UInt16[] InputReg = new UInt16[24];
        private bool _Input1 = true;
        public bool Input1 { get { return _Input1; } set { _Input1 = value; NotifyPropertyChanged("_Input1"); } }

        private bool _Input2 = true;
        public bool Input2 { get { return _Input2; } set { _Input2 = value; NotifyPropertyChanged("_Input2"); } }


        private bool _Input3 = true;
        public bool Input3 { get { return _Input3; } set { _Input3 = value; NotifyPropertyChanged("_Input3"); } }


        private bool _Input4 = true;
        public bool Input4 { get { return _Input4; } set { _Input4 = value; NotifyPropertyChanged("_Input4"); } }


        private bool _Relay = true;
        public bool Relay { get { return _Relay; } set { _Relay = value; NotifyPropertyChanged("_Relay"); } }


        public BusTag(string name, Module owner)
        {
            strIcon = "\xE8EC";
            mdSolidColorBrush = Brushes.Purple;
            ModuleDevType = 3;
            Owner = owner;
            Name = name;
            //OnCommandMultiSensor = new CommandModule(OnCommandMultiSensorExec);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(ModuleDevType);
            writer.Write(Name);
            writer.Write(Comment);
            writer.Write(Polling);
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(SlaveAddress);
            writer.Write(Input1);
            writer.Write(Input2);
            writer.Write(Input3);
            writer.Write(Input4);
            writer.Write(Relay);
        }

        public void Read(BinaryReader reader)
        {
            ModuleDevType = reader.ReadInt32();
            Name = reader.ReadString();
            Comment = reader.ReadString();
            Polling = reader.ReadBoolean();
            PositionX = reader.ReadDouble();
            PositionY = reader.ReadDouble();
            SlaveAddress = reader.ReadInt32();

        /*-{   Input1 = reader.ReadBoolean();
            Input2 = reader.ReadBoolean();
            Input3 = reader.ReadBoolean();
            Input4 = reader.ReadBoolean();
            Relay = reader.ReadBoolean();}*/

        }
    }

    public class ViewModelCnf : INotifyPropertyChanged
    {
        public ViewModelCnf()
        {
            scillaObjects = new ObservableCollection<ScillaObject>();
            scillaObjects.Add(new ScillaObject("Scilla Object"));

        }

        private ObservableCollection<ScillaEvent> _ScillaEvents = new ObservableCollection<ScillaEvent>();
        public ObservableCollection<ScillaEvent> ScillaEvents
        { get { return this._ScillaEvents; } }




        private ObservableCollection<ScillaObject> scillaObjects;
        public ObservableCollection<ScillaObject> ScillaObjects
        { get { return this.scillaObjects; } }


        private string _strTitlePlanArea = "Item with attached Floor Plan is not selected";
        public string strTitlePlanArea { get { return _strTitlePlanArea; } set { _strTitlePlanArea = value; NotifyPropertyChanged("strTitlePlanArea"); } }

        private double _ImageZoom = 1;
        public double ImageZoom { get { return _ImageZoom; }    set { _ImageZoom = value; NotifyPropertyChanged("ImageZoom");} }


        private int _PollingCounter = 0;
        public int PollingCounter { get { return _PollingCounter; } set { _PollingCounter = value; NotifyPropertyChanged("PollingCounter"); } }



        private bool _UDP_Listener = false;// 10 значений 0-9
        public bool UDP_Listener { get { return _UDP_Listener; } set { _UDP_Listener = value; NotifyPropertyChanged("UDP_Listener"); } }
        /// </summary>
        /// 

        /*********************************************************************************************************************************/
        /*                        ДЛЯ ВКЛАДКИ МОДУЛИ
        /*********************************************************************************************************************************/
        private string _TabModuleName = "";
        public string TabModuleName { get { return _TabModuleName; } set { _TabModuleName = value; NotifyPropertyChanged("TabModuleName"); } }

        private string _TabModuleComment = "";
        public string TabModuleComment { get { return _TabModuleComment; } set { _TabModuleComment = value; NotifyPropertyChanged("TabModuleComment"); } }


        private double _TabModulePositionX = 0;
        public double TabModulePositionX { get { return _TabModulePositionX; } set { _TabModulePositionX = value; NotifyPropertyChanged("TabModulePositionX"); } }

        private double _TabModulePositionY = 0;
        public double TabModulePositionY { get { return _TabModulePositionY; } set { _TabModulePositionY = value; NotifyPropertyChanged("TabModulePositionY"); } }


        private byte _TabModuleIpAddress_01 = 0xC0;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte TabModuleIpAddress_01
        { get { return _TabModuleIpAddress_01; } set { _TabModuleIpAddress_01 = value; NotifyPropertyChanged("TabModuleIpAddress_01"); } }

        private byte _TabModuleIpAddress_02 = 0xA8;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte TabModuleIpAddress_02
        { get { return _TabModuleIpAddress_02; } set { _TabModuleIpAddress_02 = value; NotifyPropertyChanged("TabModuleIpAddress_02"); } }

        private byte _TabModuleIpAddress_03 = 0x01;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte TabModuleIpAddress_03
        { get { return _TabModuleIpAddress_03; } set { _TabModuleIpAddress_03 = value; NotifyPropertyChanged("TabModuleIpAddress_03"); } }

        private byte _TabModuleIpAddress_04 = 0x64;//new byte[4] { 0xC0, 0xA8, 0x01, 0x64 };
        public byte TabModuleIpAddress_04
        { get { return _TabModuleIpAddress_04; } set { _TabModuleIpAddress_04 = value; NotifyPropertyChanged("TabModuleIpAddress_04"); } }


        private byte _TabModuleMacAddress_01 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_01
        { get { return _TabModuleMacAddress_01; } set { _TabModuleMacAddress_01 = value; NotifyPropertyChanged("TabModuleMacAddress_01"); } }
        private byte _TabModuleMacAddress_02 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_02
        { get { return _TabModuleMacAddress_02; } set { _TabModuleMacAddress_02 = value; NotifyPropertyChanged("TabModuleMacAddress_02"); } }
        private byte _TabModuleMacAddress_03 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_03
        { get { return _TabModuleMacAddress_03; } set { _TabModuleMacAddress_03 = value; NotifyPropertyChanged("TabModuleMacAddress_03"); } }
        private byte _TabModuleMacAddress_04 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_04
        { get { return _TabModuleMacAddress_04; } set { _TabModuleMacAddress_04 = value; NotifyPropertyChanged("TabModuleMacAddress_04"); } }
        private byte _TabModuleMacAddress_05 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_05
        { get { return _TabModuleMacAddress_05; } set { _TabModuleMacAddress_05 = value; NotifyPropertyChanged("TabModuleMacAddress_05"); } }
        private byte _TabModuleMacAddress_06 = 0xAA;//, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte TabModuleMacAddress_06
        { get { return _TabModuleMacAddress_06; } set { _TabModuleMacAddress_06 = value; NotifyPropertyChanged("TabModuleMacAddress_06"); } }

        ///
        private int _TabModulePort = 0x913;// 
        public int TabModulePort { get { return _TabModulePort; } set { _TabModulePort = value; NotifyPropertyChanged("TabModulePort"); } }

        private byte _TabServerIpAddress_01 = 192;// { 192, 168, 1, 10 };
        public byte TabModuleServerIpAddress_01
        { get { return _TabServerIpAddress_01; } set { _TabServerIpAddress_01 = value; NotifyPropertyChanged("TabModuleServerIpAddress_01"); } }
        private byte _TabServerIpAddress_02 = 168;// { 192, 168, 1, 10 };
        public byte TabModuleServerIpAddress_02
        { get { return _TabServerIpAddress_02; } set { _TabServerIpAddress_02 = value; NotifyPropertyChanged("TabModuleServerIpAddress_02"); } }
        private byte _TabServerIpAddress_03 = 1;// { 192, 168, 1, 10 };
        public byte TabModuleServerIpAddress_03
        { get { return _TabServerIpAddress_03; } set { _TabServerIpAddress_03 = value; NotifyPropertyChanged("TabModuleServerIpAddress_03"); } }
        private byte _TabServerIpAddress_04 = 10;// { 192, 168, 1, 10 };
        public byte TabModuleServerIpAddress_04
        { get { return _TabServerIpAddress_04; } set { _TabServerIpAddress_04 = value; NotifyPropertyChanged("TabModuleServerIpAddress_04"); } }

        private int _TabServerPort = 2424;// 
        public int TabModuleServerPort { get { return _TabServerPort; } set { _TabServerPort = value; NotifyPropertyChanged("TabModuleServerPort"); } }


        private string _TabTel = "012345678901";// 
        public string TabModuleTel { get { return _TabTel; } set { _TabTel = value; NotifyPropertyChanged("TabModuleTel"); } }
        private bool _TabTelAvailable = false;// 
        public bool TabModuleTelAvailable { get { return _TabTelAvailable; } set { _TabTelAvailable = value; NotifyPropertyChanged("TabModuleTelAvailable"); } }

        /************************************************/
        /* TabMSensor*/
        private string _TabMSensorName = "";
        public string TabMSensorName { get { return _TabMSensorName; } set { _TabMSensorName = value; NotifyPropertyChanged("TabMSensorName"); } }

        private string _TabMSensorComment = "";
        public string TabMSensorComment { get { return _TabMSensorComment; } set { _TabMSensorComment = value; NotifyPropertyChanged("TabMSensorComment"); } }

        private double _TabMSensorPositionX = 0;
        public double TabMSensorPositionX { get { return _TabMSensorPositionX; } set { _TabMSensorPositionX = value; NotifyPropertyChanged("TabMSensorPositionX"); } }

        private double _TabMSensorPositionY = 0;
        public double TabMSensorPositionY { get { return _TabMSensorPositionY; } set { _TabMSensorPositionY = value; NotifyPropertyChanged("TabMSensorPositionY"); } }

        private int _TabMSensorSlaveAddress = 247;
        public int TabMSensorSlaveAddress { get { return _TabMSensorSlaveAddress; } set { _TabMSensorSlaveAddress = value; NotifyPropertyChanged("TabMSensorSlaveAddress"); } }






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







        private UInt16 _TabMSensorThWarnSmogO = 1000;
        public UInt16 TabMSensorThWarnSmogO { get { return _TabMSensorThWarnSmogO; } set { _TabMSensorThWarnSmogO = value; NotifyPropertyChanged("TabMSensorThWarnSmogO"); } }

        private UInt16 _TabMSensorThAlrmSmogO = 1500;
        public UInt16 TabMSensorThAlrmSmogO { get { return _TabMSensorThAlrmSmogO; } set { _TabMSensorThAlrmSmogO = value; NotifyPropertyChanged("TabMSensorThAlrmSmogO"); } }

        private UInt16 _TabMSensorThSmogORefU = 0;
        public UInt16 TabMSensorThSmogORefU { get { return _TabMSensorThSmogORefU; } set { _TabMSensorThSmogORefU = value; NotifyPropertyChanged("TabMSensorThSmogORefU"); } }

        private UInt16 _TabMSensorThSmogO_LED = 0;
        public UInt16 TabMSensorThSmogO_LED { get { return _TabMSensorThSmogO_LED; } set { _TabMSensorThSmogO_LED = value; NotifyPropertyChanged("TabMSensorThSmogO_LED"); } }

        private UInt16 _TabMSensorThWarnTempA = 2800;
        public UInt16 TabMSensorThWarnTempA { get { return _TabMSensorThWarnTempA; } set { _TabMSensorThWarnTempA = value; NotifyPropertyChanged("TabMSensorThWarnTempA"); } }

        private UInt16 _TabMSensorThAlrmTempA = 3000;
        public UInt16 TabMSensorThAlrmTempA { get { return _TabMSensorThAlrmTempA; } set { _TabMSensorThAlrmTempA = value; NotifyPropertyChanged("TabMSensorThAlrmTempA"); } }

        private UInt16 _TabMSensorThWarnTempD = 60;
        public UInt16 TabMSensorThWarnTempD { get { return _TabMSensorThWarnTempD; } set { _TabMSensorThWarnTempD = value; NotifyPropertyChanged("TabMSensorThWarnTempD"); } }

        private UInt16 _TabMSensorThAlrmTempD = 80;
        public UInt16 TabMSensorThAlrmTempD { get { return _TabMSensorThAlrmTempD; } set { _TabMSensorThAlrmTempD = value; NotifyPropertyChanged("TabMSensorThAlrmTempD"); } }

        private UInt16 _TabMSensorThWarnSmogE = 2400;
        public UInt16 TabMSensorThWarnSmogE { get { return _TabMSensorThWarnSmogE; } set { _TabMSensorThWarnSmogE = value; NotifyPropertyChanged("TabMSensorThWarnSmogE"); } }

        private UInt16 _TabMSensorThAlrmSmogE = 2500;
        public UInt16 TabMSensorThAlrmSmogE { get { return _TabMSensorThAlrmSmogE; } set { _TabMSensorThAlrmSmogE = value; NotifyPropertyChanged("TabMSensorThAlrmSmogE"); } }

        private UInt16 _TabMSensorThWarnFlamA = 2900;
        public UInt16 TabMSensorThWarnFlamA { get { return _TabMSensorThWarnFlamA; } set { _TabMSensorThWarnFlamA = value; NotifyPropertyChanged("TabMSensorThWarnFlamA"); } }

        private UInt16 _TabMSensorThAlrmFlamA = 2800;
        public UInt16 TabMSensorThAlrmFlamA { get { return _TabMSensorThAlrmFlamA; } set { _TabMSensorThAlrmFlamA = value; NotifyPropertyChanged("TabMSensorThAlrmFlamA"); } }

        private UInt16 _TabMSensorThWarnFlamD = 1800;
        public UInt16 TabMSensorThWarnFlamD { get { return _TabMSensorThWarnFlamD; } set { _TabMSensorThWarnFlamD = value; NotifyPropertyChanged("TabMSensorThWarnFlamD"); } }

        private UInt16 _TabMSensorThAlrmFlamD = 2000;
        public UInt16 TabMSensorThAlrmFlamD { get { return _TabMSensorThAlrmFlamD; } set { _TabMSensorThAlrmFlamD = value; NotifyPropertyChanged("TabMSensorThAlrmFlamD"); } }

        private UInt16 _TabMSensorThWarnCO = 2000;
        public UInt16 TabMSensorThWarnCO { get { return _TabMSensorThWarnCO; } set { _TabMSensorThWarnCO = value; NotifyPropertyChanged("TabMSensorThWarnCO"); } }

        private UInt16 _TabMSensorThAlrmCO = 4000;
        public UInt16 TabMSensorThAlrmCO { get { return _TabMSensorThAlrmCO; } set { _TabMSensorThAlrmCO = value; NotifyPropertyChanged("TabMSensorThAlrmCO"); } }

        private UInt16 _TabMSensorThWarnVOC = 2000;
        public UInt16 TabMSensorThWarnVOC { get { return _TabMSensorThWarnVOC; } set { _TabMSensorThWarnVOC = value; NotifyPropertyChanged("TabMSensorThWarnVOC"); } }

        private UInt16 _TabMSensorThAlrmVOC = 4000;
        public UInt16 TabMSensorThAlrmVOC { get { return _TabMSensorThAlrmVOC; } set { _TabMSensorThAlrmVOC = value; NotifyPropertyChanged("TabMSensorThAlrmVOC"); } }
        /*************************************************************************************/

        private string _TabRelayUnitName = "";
        public string TabRelayUnitName { get { return _TabRelayUnitName; } set { _TabRelayUnitName = value; NotifyPropertyChanged("TabRelayUnitName"); } }

        private string _TabRelayUnitComment = "";
        public string TabRelayUnitComment { get { return _TabRelayUnitComment; } set { _TabRelayUnitComment = value; NotifyPropertyChanged("TabRelayUnitComment"); } }

        private double _TabRelayUnitPositionX = 0;
        public double TabRelayUnitPositionX { get { return _TabRelayUnitPositionX; } set { _TabRelayUnitPositionX = value; NotifyPropertyChanged("TabRelayUnitPositionX"); } }

        private double _TabRelayUnitPositionY = 0;
        public double TabRelayUnitPositionY { get { return _TabRelayUnitPositionY; } set { _TabRelayUnitPositionY = value; NotifyPropertyChanged("TabRelayUnitPositionY"); } }

        private int _TabRelayUnitSlaveAddress = 247;
        public int TabRelayUnitSlaveAddress { get { return _TabRelayUnitSlaveAddress; } set { _TabRelayUnitSlaveAddress = value; NotifyPropertyChanged("TabRelayUnitSlaveAddress"); } }


        /********************************************************************************************/
        private string _TabBusTagName = "";
        public string TabBusTagName { get { return _TabBusTagName; } set { _TabBusTagName = value; NotifyPropertyChanged("TabBusTagName"); } }

        private string _TabBusTagComment = "";
        public string TabBusTagComment { get { return _TabBusTagComment; } set { _TabBusTagComment = value; NotifyPropertyChanged("TabBusTagComment"); } }

        private double _TabBusTagPositionX = 0;
        public double TabBusTagPositionX { get { return _TabBusTagPositionX; } set { _TabBusTagPositionX = value; NotifyPropertyChanged("TabBusTagPositionX"); } }

        private double _TabBusTagPositionY = 0;
        public double TabBusTagPositionY { get { return _TabBusTagPositionY; } set { _TabBusTagPositionY = value; NotifyPropertyChanged("TabBusTagPositionY"); } }

        private int _TabBusTagSlaveAddress = 247;
        public int TabBusTagSlaveAddress { get { return _TabBusTagSlaveAddress; } set { _TabBusTagSlaveAddress = value; NotifyPropertyChanged("TabBusTagSlaveAddress"); } }

        private bool _TabBusTagInput1 = true;
    /*-{ }*/   public bool TabBusTagInput1 { get { return _TabBusTagInput1; } set { _TabBusTagInput1 = value; NotifyPropertyChanged("TabBusTagInput1"); } }

        private bool _TabBusTagInput2 = true;
        public bool TabBusTagInput2 { get { return _TabBusTagInput2; } set { _TabBusTagInput2 = value; NotifyPropertyChanged("TabBusTagInput2"); } }

        private bool _TabBusTagInput3 = true;
        public bool TabBusTagInput3 { get { return _TabBusTagInput3; } set { _TabBusTagInput3 = value; NotifyPropertyChanged("TabBusTagInput3"); } }

        private bool _TabBusTagInput4 = true;
        public bool TabBusTagInput4 { get { return _TabBusTagInput4; } set { _TabBusTagInput4 = value; NotifyPropertyChanged("TabBusTagInput4"); } }

   //     private bool _TabMSensor_InclAnl_Flm = true;
  //      public bool TabMSensor_InclAnl_Flm { get { return _TabMSensor_InclAnl_Flm; } set { _TabMSensor_InclAnl_Flm = value; NotifyPropertyChanged("TabMSensor_InclAnl_Flm"); } }
  
        private bool _TabBusTagRelay = true;
        public bool TabBusTagRelay { get { return _TabBusTagRelay; } set { _TabBusTagRelay = value; NotifyPropertyChanged("TabBusTagRelay"); } }

        /*
        private double _ScrollViewerHeight=0;
        public double ScrollViewerHeight { get { return _ScrollViewerHeight; } set { _ScrollViewerHeight = value; NotifyPropertyChanged("ScrollViewerHeight"); } }
        private double _ScrollViewerWidth=0;
        public double ScrollViewerWidth { get { return _ScrollViewerWidth; } set { _ScrollViewerWidth = value; NotifyPropertyChanged("ScrollViewerWidth"); } }

        */


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }



      


    }

    public class CommandScillaObject : ICommand
    {
        Action<object> _execute;
        public CommandScillaObject(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) =>
            _execute(parameter);
    }

    public class CommandBuilding : ICommand
    {
        Action<object> _execute;
        public CommandBuilding(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) =>
            _execute(parameter);
    }

    public class CommandFloor : ICommand
    {
        Action<object> _execute;
        public CommandFloor(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) =>
            _execute(parameter);
    }


    public class CommandModule : ICommand
    {
        Action<object> _execute;
        public CommandModule(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) =>
            _execute(parameter);
    }

    public class CommandMultiSensor : ICommand
    {
        Action<object> _execute;
        public CommandMultiSensor(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) =>
            _execute(parameter);

    }



    public class ScillaEvent : INotifyPropertyChanged
    {




        private DateTime _EventDateTime;
        public DateTime EventDateTime { get { return _EventDateTime; } set { _EventDateTime = value; NotifyPropertyChanged("EventDateTime"); } }
       
        private string _Building;
        public string Building { get { return _Building; } set { _Building = value; NotifyPropertyChanged("Building"); } }

        private string _Floor;
        public string Floor { get { return _Floor; } set { _Floor = value; NotifyPropertyChanged("Floor"); } }
        
        private string _Module;
        public string Module { get { return _Module; } set { _Module = value; NotifyPropertyChanged("Module"); } }


        private string _ModuleDev;
        public string ModuleDev { get { return _ModuleDev; } set { _ModuleDev = value; NotifyPropertyChanged("ModuleDev"); } }



        private string _Stage;
        public string Stage { get { return _Stage; } set { _Stage = value; NotifyPropertyChanged("Stage"); } }

        
        
        private string _Comment;
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

  

        public ScillaEvent(DateTime aEventDateTime, string aBuilding, string aFloor, string aModule,string aModuleDev,string aStage, string aComment)
        {
            _EventDateTime = aEventDateTime;
            _Building = aBuilding;
            _Floor = aFloor;
            _Module = aModule;
            _ModuleDev = aModuleDev;
            _Stage = aStage;
            _Comment = aComment;
        }
        
public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }


}
