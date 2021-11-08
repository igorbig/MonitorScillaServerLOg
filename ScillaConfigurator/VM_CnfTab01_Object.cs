using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ScillaConfigurator
{
    public class VM_CnfTab01_Object : INotifyPropertyChanged
    {

        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name");}}

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
