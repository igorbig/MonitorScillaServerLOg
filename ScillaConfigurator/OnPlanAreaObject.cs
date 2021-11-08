using _ScillaConfigurator;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _ScillaConfigurator
{
    public class OnPlanAreaObject : Canvas
    {
        static public ContextMenu CANVAS_CONTTEXT_MENU = null;
        static private FontFamily iconFontFamily = new FontFamily("Segoe MDL2 Assets");
    //public SystemDevice deviceData = null;
    public int deviceId = 0;

    private Shape bg = null;
    private TextBlock icon = null;
    private double _Width = 30;
    private double _Height = 30;
    private double _X = 0;
    private double _Y = 0;
    private double fontSize = 20;
    private int _StrokeThickness = 2;

    public Boolean _LMButtonPressed = false;
    private Point _LMBPoint = new Point(0, 0);

    public void InitOPAO(Shape _bg, String symbol, int _deviceId)
    {
               deviceId = _deviceId;
        Console.WriteLine("InitOPAO " + deviceId);

        bg = _bg;
        bg.StrokeThickness = _StrokeThickness;
        bg.Stroke = Brushes.Black;

        /*if (App.myApp.iLightModule == m && App.myApp.iLightMultiSensor == k)
            bg.Stroke = Brushes.DarkOrange;
        else
            bg.Stroke = Brushes.Black;
        */

        bg.Width = _Width;
        bg.Height = _Height;
        Children.Add(bg);

        icon = new TextBlock();
        icon.Text = symbol;
        icon.FontFamily = iconFontFamily;

        icon.FontSize = fontSize;
        icon.Foreground = Brushes.White;
        icon.Width = fontSize;
        icon.Height = fontSize;
        icon.Margin = new Thickness((_Width - fontSize) * 0.5);
        Children.Add(icon);


            MouseRightButtonUp += new MouseButtonEventHandler(UIElement_mRightButton_up);
            MouseLeftButtonUp += new MouseButtonEventHandler(UIElement_mLeftButton_up);
            MouseLeftButtonDown += new MouseButtonEventHandler(UIElement_mLeftButton_down);


            pageCnf.CANVAS_PLAN_AREA.MouseMove += new MouseEventHandler(UIElement_on_MouseMove);
    }
    public void SetXY(double _x, double _y)
    {
        _X = _x * App.myApp.VMS.ImageZoom - _Width * 0.5;
        _Y = _y * App.myApp.VMS.ImageZoom - _Height * 0.5;
        SetValue(LeftProperty, _X);
        SetValue(TopProperty, _Y);
    }

    public void UIElement_mLeftButton_down(object sender, MouseEventArgs e)
    {
        _LMBPoint = e.GetPosition(this);

        bg.Stroke = Brushes.LightGreen;
        _LMButtonPressed = true;
    }
    public void UIElement_mLeftButton_up(object sender, MouseEventArgs e)
    {
        bg.Stroke = Brushes.DarkBlue;
        _LMButtonPressed = false;
        _LMBPoint = new Point(0, 0);
    }
        /*{-}*/
        public void UIElement_mRightButton_up(object sender, MouseEventArgs e)
        {
   /*+{          bg.Stroke = Brushes.DarkOrange;
            ContextMenu cm = pageCnf.CANVAS_CONTTEXT_MENU;

                 //   cm = pageCnf.CANVAS_CONTTEXT_MENU; //+{

            cm.PlacementTarget = sender as Button;

            cm.IsOpen = true;
            */


        }


        public virtual void UIElement_on_MouseMove(object sender, MouseEventArgs e)
    {
        /*if (_LMButtonPressed)
        {
            Point pt = e.GetPosition(MainWindowLayout.CANVAS_PLAN_AREA);
            Console.WriteLine(pt);
            SetXY(pt.X / App.myApp.VMS.ImageZoom, pt.Y / App.myApp.VMS.ImageZoom);
        }*/
    }
}
}
