using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ScillaConfigurator
{
    public class VM_CnfTab03_Floor : INotifyPropertyChanged
    {
        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }

        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }

        private double _FloorWidth = 60.0;
        public double FloorWidth { get { return _FloorWidth; } set { _FloorWidth = value; NotifyPropertyChanged("FloorWidth"); } }

        private double _FloorHeight = 40.0;
        public double FloorHeight { get { return _FloorHeight; } set { _FloorHeight = value; NotifyPropertyChanged("FloorHeight"); } }

        private double _FloorHorizontalShift = 10.0;
        public double FloorHorizontalShift { get { return _FloorHorizontalShift; } set { _FloorHorizontalShift = value; NotifyPropertyChanged("FloorHorizontalShift"); } }

        private double _FloorVerticalShift = 10.0;
        public double FloorVerticalShift { get { return _FloorVerticalShift; } set { _FloorVerticalShift = value; NotifyPropertyChanged("FloorVerticalShift"); } }





        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
