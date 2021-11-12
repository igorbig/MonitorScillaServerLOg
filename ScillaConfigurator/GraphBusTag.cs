using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace _ScillaConfigurator
{
    class GraphBusTag
    {
        /*
        static public Canvas GraphCanvas = null;
        static public TextBlock TextBlockFourZero = null;
        */
        static SolidColorBrush SCB_001 = new SolidColorBrush(Color.FromArgb(255, 0, 0, 48));//Черный;//Черный
        static SolidColorBrush SCB_002 = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));//GridLine Сетка;//GridLine Сетка
        static SolidColorBrush[] SCB = new SolidColorBrush[] { new SolidColorBrush(Color.FromArgb(255, 0x87,0xCE,0xFA)),
                                                               new SolidColorBrush(Color.FromArgb(255, 100, 255,100)),
                                                               new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)) };
        static SolidColorBrush[] SCB_T = new SolidColorBrush[] { new SolidColorBrush(Color.FromArgb(255, 0x7f, 255, 0)) ,
                                                                 new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)) };


        //static public int iReady = 0;
        //static public bool ReadyAllData = false;

        //public static int[,] DataSmog = new int[3, 100];
        public static int[] DataInput1Tr = new int[2];

        //public static int[] DataTemp = new int[100];
        public static int[] DataInput2Tr = new int[2];

        // public static int[] DataTempDig = new int[100];
        public static int[] DataInput3Tr = new int[2];

        //public static int[] DataSmogE = new int[100];
     //   public static int[] DataInput4Tr = new int[2];

        //public static int[,] DataFlame = new int[8, 100];
        //public static int[] DataFlameSTD = new int[100];
 //       public static int[] DataFlameTr1 = new int[2];
 //       public static int[] DataFlameTr2 = new int[2];

        //public static int[] DataCO = new int[100];
  //      public static int[] DataCOTr = new int[2];

        public void DrawGraph(Canvas GraphCanvas, int TabControlSelectedIndex)
        {

            int marginX1, marginX2, marginY1, marginY2; //отступы до поля графика слева, справа, сверху, снизу

            int dashLength = 3; //длинна риски на осях Ординат и Абсцисс в пикселях
            int minTextOffset = 3; //Минимальное расстояние от текста до границы ... справа, слева   



            //Canvas GraphCanvas = App.GraphCanvas;
            TextBlock TextBlockFourZero = App.TextBlockFourZero;

            if (GraphCanvas == null || GraphCanvas.ActualWidth == 0 || GraphCanvas.ActualHeight == 0)
                return;


            //return;
            //измерим высоту и ширину TextBlock, содержащего четыре нуля - "0000"
            double FourZeroWidth = TextBlockFourZero.ActualWidth;
            double FourZeroHeight = TextBlockFourZero.ActualHeight;

            GraphCanvas.Children.Clear();
            //return;
            //Закрасим фон графика
            Rectangle rectangle;
            rectangle = new Rectangle();
            rectangle.Fill = SCB_001;
            rectangle.Width = GraphCanvas.ActualWidth;
            rectangle.Height = GraphCanvas.ActualHeight;
            //return;
            GraphCanvas.Children.Add(rectangle);
            //App.myApp.Dispatcher.BeginInvoke((Action)(() => { GraphCanvas.Children.Add(rectangle); }));
            //return;
            marginX1 = (int)FourZeroWidth + 2 * minTextOffset + dashLength;
            marginX2 = (int)(GraphCanvas.ActualWidth - FourZeroWidth);
            marginY1 = minTextOffset + (int)(FourZeroHeight / 2);
            marginY2 = (int)GraphCanvas.ActualHeight - (2 * minTextOffset + (int)FourZeroHeight + dashLength);

            //return;
            Line line;
            //Вертикальные линии - временная разметка
            for (int i = 0; i < 11; i++)
            {
                line = new Line();
                line.UseLayoutRounding = true;
                line.X1 = (marginX1 + (marginX2 - marginX1) * i / 10);
                line.X2 = (marginX1 + (marginX2 - marginX1) * i / 10);
                line.Y1 = marginY1;
                line.Y2 = marginY2 + dashLength;
                line.Stroke = SCB_002;
                line.StrokeDashArray = new DoubleCollection() { 4, 2 }; //dashValues;
                line.StrokeThickness = 0.7;//1;//0.8;
                GraphCanvas.Children.Add(line);
                line = null;
            }
            //return;
            //Горизонтальные линии
            switch (TabControlSelectedIndex)
            {
                case 2://Temperature (digital)
                       //Горизонтальные линии - амплитудная разметка
                    for (int i = 0; i < 11; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1 - dashLength;
                        line.X2 = marginX2;
                        line.Y1 = marginY2 - (marginY2 - marginY1) * i / 10;
                        line.Y2 = marginY2 - (marginY2 - marginY1) * i / 10;
                        line.Stroke = SCB_002;
                        line.StrokeDashArray = new DoubleCollection() { 4, 2 }; //dashValues;
                        line.StrokeThickness = 0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    break;
                default:
                    //Горизонтальные линии - амплитудная разметка
                    for (int i = 0; i < 9; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1 - dashLength;
                        line.X2 = marginX2;
                        line.Y1 = marginY2 - (marginY2 - marginY1) * i / 8;
                        line.Y2 = marginY2 - (marginY2 - marginY1) * i / 8;
                        line.Stroke = SCB_002;
                        line.StrokeDashArray = new DoubleCollection() { 4, 2 }; //dashValues;
                        line.StrokeThickness = 0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    break;
            }
            //return;
            //Нарисуем подписи шкалы для 0Y слева
            TextBlock text2;
            switch (TabControlSelectedIndex)
            {
                case 2://Temperature (digital)
                       //Горизонтальные линии - амплитудная разметка
                       //Line GridLineH;
                    for (int i = 0; i < 11; i++)
                    {
                        text2 = new TextBlock();
                        text2.Text = (0 + i * 10).ToString();//F1
                        text2.FontSize = 12;
                        text2.Foreground = SCB_002;

                        text2.Width = FourZeroWidth;
                        text2.SetValue(Canvas.LeftProperty, (double)(marginX1 - dashLength - minTextOffset - FourZeroWidth));// marginX1 - pointFourZero.X
                        text2.TextAlignment = TextAlignment.Right;

                        text2.Height = 20;
                        text2.VerticalAlignment = VerticalAlignment.Center;
                        text2.SetValue(Canvas.TopProperty, (double)((marginY2 - (marginY2 - marginY1) * i / 10) - text2.Height / 2));

                        GraphCanvas.Children.Add(text2);
                    }
                    break;
                default:
                    for (int i = 0; i < 9; i++)//11
                    {
                        text2 = new TextBlock();
                        text2.Text = (0 + i * 512).ToString();//F1
                        text2.FontSize = 12;
                        text2.Foreground = SCB_002;

                        text2.Width = FourZeroWidth;
                        text2.SetValue(Canvas.LeftProperty, (double)(marginX1 - dashLength - minTextOffset - FourZeroWidth));// marginX1 - pointFourZero.X
                        text2.TextAlignment = TextAlignment.Right;

                        text2.Height = 20;
                        text2.VerticalAlignment = VerticalAlignment.Center;
                        text2.SetValue(Canvas.TopProperty, (double)((marginY2 - (marginY2 - marginY1) * i / 8) - text2.Height / 2));

                        GraphCanvas.Children.Add(text2);
                    }

                    break;
            }

            //Нарисуем подписи шкалы для 0X Время внизу
            for (int i = 0; i < 11; i++)
            {
                text2 = new TextBlock();
                //text2.Text = time.ToString("hh:mm:ss");//F1 UtcNow "o"
                text2.Text = (10 * i).ToString(); //F1 UtcNow "o"
                text2.FontSize = 12;
                text2.Foreground = SCB_002;

                text2.Width = (marginX2 - marginX1) / 10;//marginX1;
                text2.SetValue(Canvas.LeftProperty, (double)(marginX1 + ((marginX2 - marginX1) * i / 10) - (marginX2 - marginX1) / 10 / 2));// marginX1 - pointFourZero.X
                text2.TextAlignment = TextAlignment.Center;

                text2.Height = 20;
                text2.VerticalAlignment = VerticalAlignment.Top;
                text2.SetValue(Canvas.TopProperty, (double)(marginY2 + dashLength + minTextOffset));

                GraphCanvas.Children.Add(text2);
                //text2 = null;
                //time = time - timespan[App.myApp.SVM.iSweep];
            }


            //Заголовки обозначения
        //    String[] strForSmog = new String[] { "LED ON", "LED OFF", "LED ON - LED OFF" };
      //      String[] strForFlame = new String[] { "Flame detector: 1, 2, 3, 4, 5, 6", "Last 3 AVR Min Value", "Last 3 AVR Standart Deviation" };
            switch (TabControlSelectedIndex)
            {
                case 0://Input1 (optical)
                    text2 = new TextBlock();
                    text2.Text = "Input1"; //F1 UtcNow "o"
                    text2.FontSize = 12;
                    text2.Foreground = SCB[2];
                    text2.Width = 200;//marginX1;
                    text2.SetValue(Canvas.LeftProperty, (double)(marginX2 - 10 - 200));// marginX1 - pointFourZero.X
                    text2.TextAlignment = TextAlignment.Right;
                    text2.VerticalAlignment = VerticalAlignment.Top;
                    text2.SetValue(Canvas.TopProperty, (double)(marginY1));
                    GraphCanvas.Children.Add(text2);
                    break;

             
   //-{}             case 1://Temperature (analog)
                case 1://Input2(analog)

                    text2 = new TextBlock();
                    text2.Text = "Input2"; //F1 UtcNow "o"
                    text2.FontSize = 12;
                    text2.Foreground = SCB[2];
                    text2.Width = 200;//marginX1;
                    text2.SetValue(Canvas.LeftProperty, (double)(marginX2 - 10 - 200));// marginX1 - pointFourZero.X
                    text2.TextAlignment = TextAlignment.Right;
                    text2.VerticalAlignment = VerticalAlignment.Top;
                    text2.SetValue(Canvas.TopProperty, (double)(marginY1));
                    GraphCanvas.Children.Add(text2);
                    break;
    
                case 2://Input3
                    text2 = new TextBlock();
                    text2.Text = "Input3"; //F1 UtcNow "o"
                    text2.FontSize = 12;
                    text2.Foreground = SCB[2];
                    text2.Width = 200;//marginX1;
                    text2.SetValue(Canvas.LeftProperty, (double)(marginX2 - 10 - 200));// marginX1 - pointFourZero.X
                    text2.TextAlignment = TextAlignment.Right;
                    text2.VerticalAlignment = VerticalAlignment.Top;
                    text2.SetValue(Canvas.TopProperty, (double)(marginY1));
                    GraphCanvas.Children.Add(text2);

                    break;
                case 3://Relay
                    text2 = new TextBlock();
                    text2.Text = "Input4"; //F1 UtcNow "o"
                    text2.FontSize = 12;
                    text2.Foreground = SCB[2];
                    text2.Width = 200;//marginX1;
                    text2.SetValue(Canvas.LeftProperty, (double)(marginX2 - 10 - 200));// marginX1 - pointFourZero.X
                    text2.TextAlignment = TextAlignment.Right;
                    text2.VerticalAlignment = VerticalAlignment.Top;
                    text2.SetValue(Canvas.TopProperty, (double)(marginY1));
                    GraphCanvas.Children.Add(text2);
                    break;
             }




            line = new Line();
            line.UseLayoutRounding = true;
            line.X1 = marginX1 + App.iReady * (marginX2 - marginX1) / 100;
            line.X2 = marginX1 + App.iReady * (marginX2 - marginX1) / 100;
            line.Y1 = marginY1;//(marginY2) - DataInput1[i] * (marginY2 - marginY1) / 4096;
            line.Y2 = marginY2;//(marginY2) - DataInput1[i] * (marginY2 - marginY1) / 4096;
            line.Stroke = Brushes.White;
            line.StrokeDashArray = new DoubleCollection() { 2, 2 }; //dashValues;
            line.StrokeThickness = 1.0;//0.7;//0.8;
            GraphCanvas.Children.Add(line);
            line = null;


            Ellipse el = null;
            int start, finish;
            start = 0;
            finish = App.iReady;
            if (App.ReadyAllData == true)
                finish = 100;

            switch (TabControlSelectedIndex)
            {
                case 0://input1
                    for (int i = 0; i < 2; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1;
                        line.X2 = marginX2;
                        line.Y1 = (marginY2) - DataInput1Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Y2 = (marginY2) - DataInput1Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Stroke = SCB_T[i];
                        line.StrokeDashArray = new DoubleCollection() { 6, 4 }; //dashValues;
                        line.StrokeThickness = 2.0;//0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    for (int i = start; i < finish; i++)
                    {
                        el = new Ellipse();
                        el.Width = 4;
                        el.Height = 4;
                        el.SetValue(Canvas.LeftProperty, (double)(marginX1 - 2) + i * (marginX2 - marginX1) / 100);
    //-{}
                        el.SetValue(Canvas.TopProperty, (double)(marginY2 - 2) - App.DataInput1[i] * (marginY2 - marginY1) / 4096);
                        el.Fill = SCB[2];//SCB_Line[indexData];
                        GraphCanvas.Children.Add(el);
                    }

                    break;

                case 1://Temperature (analog)//input2

                    for (int i = 0; i < 2; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1;
                        line.X2 = marginX2;
                        line.Y1 = (marginY2) - DataInput2Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Y2 = (marginY2) - DataInput2Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Stroke = SCB_T[i];
                        line.StrokeDashArray = new DoubleCollection() { 6, 4 }; //dashValues;
                        line.StrokeThickness = 2.0;//0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    for (int i = start; i < finish; i++)
                    {
                        el = new Ellipse();
                        el.Width = 4;
                        el.Height = 4;
                        el.SetValue(Canvas.LeftProperty, (double)(marginX1 - 2) + i * (marginX2 - marginX1) / 100);
                                 el.SetValue(Canvas.TopProperty, (double)(marginY2 - 2) - App.DataInput2[i] * (marginY2 - marginY1) / 4096);
                        el.Fill = SCB[2];//SCB_Line[indexData];
                        GraphCanvas.Children.Add(el);
                    }
                    break;
                case 2://Temperature (digital)//input3
                    for (int i = 0; i < 2; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1;
                        line.X2 = marginX2;
                        line.Y1 = (marginY2) - DataInput3Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Y2 = (marginY2) - DataInput3Tr[i] * (marginY2 - marginY1) / 4096;
                        line.Stroke = SCB_T[i];
                        line.StrokeDashArray = new DoubleCollection() { 6, 4 }; //dashValues;
                        line.StrokeThickness = 2.0;//0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    for (int i = start; i < finish; i++)
                    {
                        el = new Ellipse();
                        el.Width = 4;
                        el.Height = 4;
                        el.SetValue(Canvas.LeftProperty, (double)(marginX1 - 2) + i * (marginX2 - marginX1) / 100);
                        el.SetValue(Canvas.TopProperty, (double)(marginY2 - 2) - App.DataInput3[i] * (marginY2 - marginY1) / 4096);
                        el.Fill = SCB[2];//SCB_Line[indexData];
                        GraphCanvas.Children.Add(el);
                    }
                    break;

                  case 3://Flame detector  relay

                    for (int i = 0; i < 2; i++)
                    {
                        line = new Line();
                        line.UseLayoutRounding = true;
                        line.X1 = marginX1;
                        line.X2 = marginX2;
                      //  line.Y1 = (marginY2) - DataInput2[i] * (marginY2 - marginY1) / 4096;
                    //    line.Y2 = (marginY2) - DataInput2[i] * (marginY2 - marginY1) / 4096;
                        line.Stroke = SCB_T[i];
                        line.StrokeDashArray = new DoubleCollection() { 6, 4 }; //dashValues;
                        line.StrokeThickness = 2.0;//0.7;//0.8;
                        GraphCanvas.Children.Add(line);
                        line = null;
                    }
                    for (int i = start; i < finish; i++)
                    {
                        el = new Ellipse();
                        el.Width = 4;
                        el.Height = 4;
                        el.SetValue(Canvas.LeftProperty, (double)(marginX1 - 2) + i * (marginX2 - marginX1) / 100);
                     //   el.SetValue(Canvas.TopProperty, (double)(marginY2 - 2) - App.DataInput4[i] * (marginY2 - marginY1) / 4096);
                        el.Fill = SCB[2];//SCB_Line[indexData];
                        GraphCanvas.Children.Add(el);
                    }

                    break;

              }
        }
    }
}
