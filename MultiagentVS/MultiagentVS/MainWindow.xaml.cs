using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    public delegate void DoUpdate();
    //public delegate void DoDraw(Canvas mapCanvas);

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        static int FPS = 60;
        //public static readonly int Width = 800;
        //public static readonly int Height = 600;
        Map _myMap;
        CarMan _cm;
        public event DoUpdate doUpdateEvent;
        //public event DoDraw doDrawEvent;
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();

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

            
            this._dispatcherTimer.Tick += dispatcherTimer_Tick;
            this._dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
        }
        
        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_cm == null)
            {
                _cm = new CarMan();
                //Map.TrafLight = new TrafficLight();
                Map.XRoad = new XRoad();
                _dispatcherTimer.Start();
            }
        }

        void myMap_mapUpdatedEvent()
        {
            mapCanvas.Children.Clear();

            this.DrawMap();
            //doDrawEvent?.Invoke(mapCanvas);
            
            mapCanvas.UpdateLayout();
        }

        private void DrawMap(/*IEnumerable<Car> cars*/)
        {
            List<Car> cars = new List<Car>();

            foreach (Road road in Map.Roads)
            {
                cars = cars.Concat(road.Cars).ToList();
                road.Draw(mapCanvas);
            }

            int index = 0, max = cars.Count;

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
                    c.Draw(mapCanvas);
            }
            Map.TotalCars = cars.Count;

            if (Map.XRoad != null)
                Map.XRoad.Draw(mapCanvas);
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

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            //_myMap.UpdateEnvironnement();

            // map must subscribe to this event
            doUpdateEvent?.Invoke();
        }
    }
}
