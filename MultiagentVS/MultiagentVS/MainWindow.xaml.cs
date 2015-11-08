using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MultiagentVS.Model;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Drawing.Rectangle;
using ShapeRectangle = System.Windows.Shapes.Rectangle;
using Line = System.Windows.Shapes.Line;

namespace MultiagentVS
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static int FPS = 60;
        //public static readonly int Width = 800;
        //public static readonly int Height = 600;
        Map _myMap;
        CarMan _cm;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }



        void MainWindow_Loaded(object _sender, RoutedEventArgs _e)
        {
            mapCanvas.MouseDown += mapCanvas_MouseDown;

            _myMap = new Map(mapCanvas.ActualWidth, mapCanvas.ActualHeight);
            _myMap.mapUpdatedEvent += myMap_mapUpdatedEvent;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            dispatcherTimer.Start();
        }
        
        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_cm == null)
            {
                _cm = new CarMan();
                //Map.TrafLight = new TrafficLight();
                Map.XRoad = new XRoad();
            }
        }

        void myMap_mapUpdatedEvent()
        {
            mapCanvas.Children.Clear();

            this.DrawMap();

            mapCanvas.UpdateLayout();
        }

        private void DrawMap(/*IEnumerable<Car> cars*/)
        {
            // TODO: pass cars as param
            //List<Car> cars = Map.LeftToRight.Cars.Concat(Map.RightToLeft.Cars).ToList();


            List<Car> cars = new List<Car>();

            foreach (Road road in Map.Roads)
            {
                cars = cars.Concat(road.Cars).ToList();
                road.Draw(ref mapCanvas);
            }

            int index = 0, max = cars.Count;

            //Map.LeftToRight.Draw(ref mapCanvas);
            //Map.RightToLeft.Draw(ref mapCanvas);
            //Map.TopToBottom.Draw(ref mapCanvas);
            Car c;

            for (; index < max; index++)
            {
                c = cars.ElementAt(index);

                if (c.IsOutOfMap())
                {
                    cars.RemoveAt(index);
                    //Map.TotalCars -= 1;

                    max--;

                    if (max == index)
                        return;

                    index--;
                }
                else
                    c.Draw(ref mapCanvas);
            }
            Map.TotalCars = cars.Count;

            if(Map.XRoad != null)
                Map.XRoad.Draw(ref mapCanvas);
        }

        public static void RotateRectangle(ref ShapeRectangle rec, double angle, PointF middle)
        {
            RotateTransform rt = new RotateTransform
            {
                CenterX = middle.X,
                CenterY = middle.Y,
                Angle = angle
            };

            rec.LayoutTransform = rt;
        }

        public static void RotateCar(ref Car car, double angle, PointF middle)
        {

        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            _myMap.UpdateEnvironnement();
        }
    }
}
