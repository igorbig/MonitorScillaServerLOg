using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ScillaConfigurator
{
    public class VM_CnfTab02_Building : INotifyPropertyChanged
    {
        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged("Name"); } }


        private string _Comment = "";
        public string Comment { get { return _Comment; } set { _Comment = value; NotifyPropertyChanged("Comment"); } }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
