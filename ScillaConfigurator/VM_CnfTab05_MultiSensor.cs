using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ScillaConfigurator
{

    /*
    public class VM_CnfTab05_MultiSensor : INotifyPropertyChanged
    {
        // TabMSensor
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



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    */
    /*
    public class VM_CnfTab06_RelayUnit : INotifyPropertyChanged
    {
        // TabRelayUnit
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */
    /*
    public class VM_CnfTab07_BusTag : INotifyPropertyChanged
    {
        // TabBusTag
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */
}
