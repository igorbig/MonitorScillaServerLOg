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
    /// Interaction logic for tab03_Floor.xaml
    /// </summary>
    public partial class tab03_Floor : Page
    {
        public tab03_Floor()
        {
            InitializeComponent();
            DataContext = App.myApp.VMCnfTab03;//MainWindow.mainWindow;
        }

        private void btnUpdateCnf_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.sFloor != null)
            {
                App.myApp.sFloor.Name = App.myApp.VMCnfTab03.Name;
                App.myApp.sFloor.Comment = App.myApp.VMCnfTab03.Comment;
                App.myApp.sFloor.FloorWidth = App.myApp.VMCnfTab03.FloorWidth;
                App.myApp.sFloor.FloorHeight = App.myApp.VMCnfTab03.FloorHeight;
                App.myApp.sFloor.FloorHorizontalShift = App.myApp.VMCnfTab03.FloorHorizontalShift;
                App.myApp.sFloor.FloorVerticalShift = App.myApp.VMCnfTab03.FloorVerticalShift;
                App.myApp.Plan.Draw();
            }
        }

        private void btnRefreshCnf_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.sFloor != null)
            {
                App.myApp.VMCnfTab03.Name = App.myApp.sFloor.Name;
                App.myApp.VMCnfTab03.Comment = App.myApp.sFloor.Comment;
                App.myApp.VMCnfTab03.FloorWidth = App.myApp.sFloor.FloorWidth;
                App.myApp.VMCnfTab03.FloorHeight = App.myApp.sFloor.FloorHeight;
                App.myApp.VMCnfTab03.FloorHorizontalShift = App.myApp.sFloor.FloorHorizontalShift;
                App.myApp.VMCnfTab03.FloorVerticalShift = App.myApp.sFloor.FloorVerticalShift;
            }
        }






        
        



        private void btnAttachFloorPlan_Click(object sender, RoutedEventArgs e)
        {
            if (App.myApp.sFloor != null)
            {
                Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
                //dl1.FileName = "MYFileSave";
                dl1.DefaultExt = ".png";
                dl1.Filter = "Image documents (.png)|*.png";
                Nullable<bool> result = dl1.ShowDialog();
                if (result == true)
                {
                    string filename = dl1.FileName;

                    App.myApp.sFloor.bmFloorPlan = new BitmapImage(new Uri(@filename, UriKind.Absolute));//@filename
                    App.myApp.sFloor.imageFloorPlan = new Image();
                    App.myApp.sFloor.imageFloorPlan.Source = App.myApp.sFloor.bmFloorPlan;

                    App.myApp.sFloor.imageHeight = App.myApp.sFloor.bmFloorPlan.Height;
                    App.myApp.sFloor.imageWidth = App.myApp.sFloor.bmFloorPlan.Width;

                    if (App.myApp.sFloor != null)
                    {

                        App.myApp.Plan.Draw();
                    }

                    /*
                    double Y = b.Height;
                    double X = b.Width;
                    image.Source = b;

                    image.Height = Y * 4;
                    image.Width = X * 4;
                    */
                }
            }

        }
    }
}
