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

    public class PlanArea
    {
        Canvas canvas;
        FontFamily fntSegoe;
        static SolidColorBrush SCB_Black = new SolidColorBrush(Color.FromArgb(255, 0, 0, 48));//Черный;//Черный

        public PlanArea(Canvas cnvs/*, TextBlock textblok*/)
        {
            canvas = cnvs;
            fntSegoe = new FontFamily("Segoe MDL2 Assets");

            //GraphCanvasStaticRedrawNeeded = true;
            //TextBlockFourZero = textblok;
            //rectangleBack = new Rectangle();
            //SCB_001 =
            //SCB_002 =
            //SCB_003 = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));//GridLine Сетка
        }

        public void Draw()
        {
            if (canvas == null )// || canvas.ActualWidth == 0 || canvas.ActualHeight == 0)
                return;
           
            if (App.myApp.sFloor != null && App.myApp.sFloor.imageFloorPlan != null)
            {
                Floor floor = App.myApp.sFloor;

                //Для корректного отображения Scroll bar изменим размеры GraphCanvas 
                //CanvasPlanArea.Height = Y * 2;
                //CanvasPlanArea.Width = X * 2;

                canvas.Children.Clear();

                App.myApp.VMS.strTitlePlanArea = floor.Owner.Name + ". " + floor.Name + ".";
                double W = canvas.ActualWidth;
                double H = canvas.ActualHeight;

                //double h = App.myApp.VMS.ScrollViewerHeight = 0;


                double K = canvas.ActualWidth / App.myApp.sFloor.imageWidth;
                //canvas.Width = canvas.ActualWidth;
                //canvas.Height = App.myApp.sFloor.imageHeight * K;



                //floor.imageFloorPlan.Width = canvas.ActualWidth;
                // floor.imageFloorPlan.Height = App.myApp.sFloor.imageHeight * K;

                /*
                floor.imageFloorPlan.Height =  = App.myApp.sFloor.imageHeight;// * App.myApp.VMS.ImageZoom;
                floor.imageFloorPlan.Width = canvas.Width = App.myApp.sFloor.imageWidth;// * App.myApp.VMS.ImageZoom;
                */

                //canvas.ActualWidth = App.myApp.sFloor.imageFloorPlan.ActualWidth;

                // canvas.ActualWidth * App.myApp.VMS.ImageZoom;

                //App.myApp.sFloor.imageFloorPlan.Width = canvas.ActualWidth/* * App.myApp.VMS.ImageZoom*/;


                //canvas.VisualParent.

                /*
                double w = App.myApp.ScrollViewerCan.ViewportWidth;
                double h = App.myApp.ScrollViewerCan.ViewportHeight;

                canvas.Width = w * App.myApp.VMS.ImageZoom; //App.myApp.sFloor.imageWidth * App.myApp.VMS.ImageZoom;
                canvas.Height = h * App.myApp.VMS.ImageZoom; //App.myApp.sFloor.imageHeight * App.myApp.VMS.ImageZoom;

                App.myApp.sFloor.imageFloorPlan.Width = canvas.Width;
                canvas.Children.Add(App.myApp.sFloor.imageFloorPlan);
                */
                //App.myApp.sFloor.imageFloorPlan.Height =  canvas.Height;


                // App.myApp.sFloor.imageFloorPlan.SetValue(Canvas.LeftProperty, (double) 0.0);
                // App.myApp.sFloor.imageFloorPlan.SetValue(Canvas.TopProperty, (double) 0.0);

                floor.imageFloorPlan.Height = canvas.Height = App.myApp.sFloor.imageHeight * App.myApp.VMS.ImageZoom;
                floor.imageFloorPlan.Width = canvas.Width = App.myApp.sFloor.imageWidth * App.myApp.VMS.ImageZoom;
                canvas.Children.Add(floor.imageFloorPlan);

                TextBlock text;
                Rectangle rect;
                Ellipse el;
                

                for (int m = 0; m < floor.Modules.Count; m++)
                {
                    
                    rect = new Rectangle();
                    rect.Fill = Brushes.Red; // new SolidColorBrush(Color.FromArgb(255, 255, 48, 0)); ;

                    rect.StrokeThickness = 2;
                    if (App.myApp.iLightModule == m)
                        rect.Stroke = Brushes.DarkOrange;//GreenYellow;//Brushes.YellowGreen;
                    else
                        rect.Stroke = Brushes.Black;
                    rect.Width = 30;
                    rect.Height = 30;
                    rect.SetValue(Canvas.LeftProperty, (double)(floor.Modules[m].PositionX * App.myApp.VMS.ImageZoom - 15));
                    rect.SetValue(Canvas.TopProperty, (double)(floor.Modules[m].PositionY * App.myApp.VMS.ImageZoom - 15));
                    canvas.Children.Add(rect);

                    text = new TextBlock();
                    text.Text = "\xE8A1";// floor.Modules[m].ModuleDevs[k].strIcon;// "\xE8EC";//F1 UtcNow "o"
                    text.FontFamily = fntSegoe;
                    text.FontSize = 20;
                    text.Foreground = Brushes.White;
                    text.Width = 20;//marginX1;
                    text.Height = 20;
                    text.SetValue(Canvas.LeftProperty, (double)(floor.Modules[m].PositionX * App.myApp.VMS.ImageZoom - 10));// marginX1 - pointFourZero.X
                    text.SetValue(Canvas.TopProperty, (double)(floor.Modules[m].PositionY * App.myApp.VMS.ImageZoom - 10));
                    text.TextAlignment = TextAlignment.Center;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    canvas.Children.Add(text);


                    for (int k = 0; k < floor.Modules[m].ModuleDevs.Count; k++)
                    {
                        el = null;
                        el = new Ellipse();
                        el.Width = 30;
                        el.Height = 30;
                        el.StrokeThickness = 2;
                        if (App.myApp.iLightModule == m && App.myApp.iLightMultiSensor == k)
                            el.Stroke = Brushes.DarkOrange;
                        else
                            el.Stroke = Brushes.Black;
                        el.SetValue(Canvas.LeftProperty, (double)(floor.Modules[m].ModuleDevs[k].PositionX * App.myApp.VMS.ImageZoom - 15));
                        el.SetValue(Canvas.TopProperty, (double)(floor.Modules[m].ModuleDevs[k].PositionY * App.myApp.VMS.ImageZoom - 15));
                        switch(floor.Modules[m].ModuleDevs[k].ModuleDevType)
                        {
                            case 1:
                                //el.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
                                el.Fill = Brushes.Green;
                                break;
                            case 2:
                                el.Fill = Brushes.MediumBlue;
                                break;
                            case 3:
                                el.Fill = Brushes.Purple ;
                                break;
                        }
                        canvas.Children.Add(el);
                        
                       
                        text = new TextBlock();
                        text.Text = floor.Modules[m].ModuleDevs[k].strIcon;// "\xE8EC";//F1 UtcNow "o"
                        text.FontFamily = fntSegoe;
                        text.FontSize = 20;
                        text.Foreground = Brushes.White;
                        text.Width = 20;//marginX1;
                        text.Height = 20;
                        text.SetValue(Canvas.LeftProperty, (double)(floor.Modules[m].ModuleDevs[k].PositionX * App.myApp.VMS.ImageZoom - 10));// marginX1 - pointFourZero.X
                        text.SetValue(Canvas.TopProperty, (double)(floor.Modules[m].ModuleDevs[k].PositionY * App.myApp.VMS.ImageZoom - 10));
                        text.TextAlignment = TextAlignment.Center;
                        text.VerticalAlignment = VerticalAlignment.Center;
                        canvas.Children.Add(text);
                    }
                }


                App.myApp.RulerX.Draw();
                App.myApp.RulerY.Draw();
            }
            else
            {
                canvas.Children.Clear();
 //-{}               App.myApp.VMS.strTitlePlanArea = "Item with attached Floor Plan is not selected";
                App.myApp.VMS.strTitlePlanArea = "Обьект с прикрепленным планом этажа не выбран";
                App.myApp.RulerX.Draw();
                App.myApp.RulerY.Draw();
            }
            /*
            Image drawing = new Image();
            drawing.src = "draw.png"; // can also be a remote URL e.g. http://
            drawing.onload = function() {
                context.drawImage(drawing, 0, 0);
            
            };
            */
        }
    }
    public class RulerX_Area
    {
        public Canvas canvas;
        public RulerX_Area(Canvas cnvs/*, TextBlock textblok*/)
        {
            canvas = cnvs;
        }
        public void Draw()
        {
            if (canvas == null || canvas.ActualWidth == 0 || canvas.ActualHeight == 0)
                return;

            canvas.Children.Clear();

            Rectangle rectangle;
            rectangle = new Rectangle();
            rectangle.Fill = Brushes.WhiteSmoke;
            rectangle.Width = canvas.ActualWidth;
            rectangle.Height = canvas.ActualHeight;
            canvas.Children.Add(rectangle);

            Floor floor = App.myApp.sFloor;
            if (floor != null && floor.imageFloorPlan != null)
            {

                //Нарисуем ос OX 
                Line line;
                line = new Line();
                line.X1 = 0;
                line.X2 = canvas.ActualWidth;
                line.Y1 = canvas.ActualHeight;
                line.Y2 = canvas.ActualHeight;
                line.Stroke = Brushes.Gray;
                line.StrokeThickness = 1;
                line.StrokeEndLineCap = PenLineCap.Triangle;
                canvas.Children.Add(line);
                
                //Нарисуем риски на ось OX

                double realImageWidth = App.myApp.sFloor.imageWidth * App.myApp.VMS.ImageZoom;
                //floor.imageFloorPlan.Width  в мерах;

                for (int i = 0; i < floor.FloorWidth + 1; i++)
                {
                    line = new Line();
                    line.X1 = realImageWidth / floor.FloorWidth * i - (int)App.myApp.sFloor.ScrollX;
                    line.X2 = realImageWidth / floor.FloorWidth * i  - (int)App.myApp.sFloor.ScrollX;
                    line.Y1 = canvas.ActualHeight;
                    if (i % 10 == 0)
                    {
                        line.Y2 = canvas.ActualHeight - 8;
                        line.Stroke = Brushes.DarkGray;
                    }
                    else
                    {
                        line.Y2 = canvas.ActualHeight - 3;
                        line.Stroke = Brushes.Gray;
                    }
                    line.StrokeThickness = 1;
                    line.StrokeEndLineCap = PenLineCap.Round;
                    canvas.Children.Add(line);
                }


                TextBlock text;
                for (int i = 0; i < floor.FloorWidth + 1; i++)
                {
                    text = new TextBlock();
                    text.Text = (i- (int)floor.FloorHorizontalShift).ToString();//F1 UtcNow "o"
                    text.FontSize = 12;
                    text.Foreground = Brushes.Gray; ;
                    text.Width = canvas.ActualWidth / 10;//marginX1;
                    text.SetValue(Canvas.LeftProperty, realImageWidth / floor.FloorWidth * i - (int)App.myApp.sFloor.ScrollX);// marginX1 - pointFourZero.X
                    text.TextAlignment = TextAlignment.Left;

                    text.Height = 20;
                    text.VerticalAlignment = VerticalAlignment.Top;
                    text.SetValue(Canvas.TopProperty, (double)0);

                    canvas.Children.Add(text);
                }
            }
        }
    }

    public class RulerY_Area
    {
        public Canvas canvas;
        public RulerY_Area(Canvas cnv/*, TextBlock textblok*/)
        {
            canvas = cnv;
        }
        public void Draw()
        {
            if (canvas == null || canvas.ActualWidth == 0 || canvas.ActualHeight == 0)
                return;

            canvas.Children.Clear();

            Rectangle rectangle;
            rectangle = new Rectangle();
            rectangle.Fill = Brushes.WhiteSmoke;
            rectangle.Width = canvas.ActualWidth;
            rectangle.Height = canvas.ActualHeight;
            canvas.Children.Add(rectangle);

            Floor floor = App.myApp.sFloor;
            if (floor != null && floor.imageFloorPlan != null)
            {
                //Нарисуем ос OX 
                Line line;
                line = new Line();
                line.X1 = canvas.ActualWidth;
                line.X2 = canvas.ActualWidth;
                line.Y1 = 0;
                line.Y2 = canvas.ActualHeight;
                line.Stroke = Brushes.Gray;//Brushes.Gray;
                line.StrokeThickness = 1;
                line.StrokeEndLineCap = PenLineCap.Triangle;
                canvas.Children.Add(line);

                //Нарисуем риски на ось OY

                double realImageHeight = App.myApp.sFloor.imageHeight * App.myApp.VMS.ImageZoom;
                //floor.imageFloorPlan.Width  в мерах;

                for (int i = 0; i < floor.FloorHeight + 1; i++)
                {
                    line = new Line();
                    line.Y1 = realImageHeight / floor.FloorHeight * i - (int)App.myApp.sFloor.ScrollY;
                    line.Y2 = realImageHeight / floor.FloorHeight * i - (int)App.myApp.sFloor.ScrollY;
                    
                    line.X1 = canvas.ActualWidth;
                    if (i % 10 == 0)
                    {
                        line.X2 = canvas.ActualWidth - 8;
                        line.Stroke = Brushes.Gray;//DarkGray;
                    }
                    else
                    {
                        line.X2 = canvas.ActualWidth - 3;
                        line.Stroke = Brushes.Gray;// Gray;
                    }
                    line.StrokeThickness = 1;
                    line.StrokeEndLineCap = PenLineCap.Round;
                    canvas.Children.Add(line);
                }
               
                TextBlock text2;
                for (int i = 0; i < floor.FloorHeight + 1; i++)
                {
                    text2 = new TextBlock();
                    text2.Text = (i - (int)floor.FloorVerticalShift).ToString();//F1 UtcNow "o"
                    text2.FontSize = 12;
                    text2.Foreground = Brushes.Gray; ;
                    text2.Width = canvas.ActualWidth;//marginX1;
                    //text2.Height = 50;
                    text2.SetValue(Canvas.TopProperty, realImageHeight / floor.FloorHeight * i - (int)App.myApp.sFloor.ScrollY);// marginX1 - pointFourZero.X
                    text2.TextAlignment = TextAlignment.Left;

                    text2.Height = 20;
                    text2.VerticalAlignment = VerticalAlignment.Center;

                   // text2.RenderTransformOrigin = rot_cen;
                    text2.RenderTransform = new RotateTransform(270.0);

                    canvas.Children.Add(text2);
                }
               
            }
        }
    }
}
