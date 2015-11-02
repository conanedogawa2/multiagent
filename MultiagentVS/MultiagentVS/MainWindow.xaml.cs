using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MultiagentVS.Model;
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
        public static readonly int Width = 800;
        public static readonly int Height = 600;
        Map _myMap;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }



        void MainWindow_Loaded(object _sender, RoutedEventArgs _e)
        {
            mapCanvas.MouseDown += mapCanvas_MouseDown;
            mapCanvas.KeyDown += mapCanvas_KeyDown;

            _myMap = new Map(mapCanvas.ActualWidth, mapCanvas.ActualHeight);
            _myMap.mapUpdatedEvent += myMap_mapUpdatedEvent;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            dispatcherTimer.Start();

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += timer_Tick;
            //timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            //timer.Start();
        }

        private void mapCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        //private void timer_Tick(object sender, EventArgs e)
        //{
        //    DispatcherTimer timer = (DispatcherTimer)sender;

        //    if (timer == null)
        //        return;

        //    timer.Stop();

        //    _myMap.AddCarOnRoad(new Car(null));
        //    Debug.WriteLine("------------Car created");
        //    timer.Interval = new TimeSpan( 0, 0, 0, 0, _myMap.GetRandomInt(1, 3) );
        //    timer.Start();
        //}

        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Road road = Map.LeftToRight;
            float carX = 0;

            if (e.ChangedButton == MouseButton.Right)
            {
                road = Map.RightToLeft;
                carX = Width;
            }

            Car frontCar = road.LastCar,
                car = new Car(Brushes.Aqua, road, carX, frontCar);

            Debug.WriteLine("------------Car " + car.Id +  " created behind " + frontCar?.Id + " on road having PosY=" + road.PosY);
        }

        void myMap_mapUpdatedEvent(IEnumerable<Car> cars)
        {
            mapCanvas.Children.Clear();

            this.DrawMap(/*cars*/);

            mapCanvas.UpdateLayout();
        }

        private void DrawMap(/*IEnumerable<Car> cars*/)
        {
            // TODO: pass cars as param
            List<Car> cars = Map.LeftToRight.Cars.Concat(Map.RightToLeft.Cars).ToList();
            int index = 0, max = cars.Count;

            Map.LeftToRight.Draw(ref mapCanvas);
            Map.RightToLeft.Draw(ref mapCanvas);
            Car c;

            for (; index < max; index++)
            {
                c = cars.ElementAt(index);

                if (c.IsOutOfMap())
                {
                    cars.RemoveAt(index);
                    max--;

                    if (max == index)
                        return;

                    index--;
                }
                else
                    c.Draw(ref mapCanvas);
            }

            //Rectangle rect = c.Rect;
            //foreach (Car c in cars)
            //{
            //    c.Draw(ref mapCanvas);
            //}
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            _myMap.UpdateEnvironnement();
        }
    }
}
