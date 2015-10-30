using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Map _myMap;

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

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += timer_Tick;
            //timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            //timer.Start();
        }

        //private void timer_Tick(object sender, EventArgs e)
        //{
        //    DispatcherTimer timer = (DispatcherTimer)sender;

        //    if (timer == null)
        //        return;

        //    timer.Stop();

        //    _myMap.AddCar(new Car(null));
        //    Debug.WriteLine("------------Car created");
        //    timer.Interval = new TimeSpan( 0, 0, 0, 0, _myMap.GetRandomInt(1, 3) );
        //    timer.Start();
        //}

        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _myMap.AddCar( new Car(Brushes.Aqua) );
            Debug.WriteLine("------------Car created");
        }

        void myMap_mapUpdatedEvent(IEnumerable<Car> cars)
        {
            mapCanvas.Children.Clear();

            DrawCars(cars);

            mapCanvas.UpdateLayout();
        }

        private void DrawCars(IEnumerable<Car> cars)
        {
            //Rectangle rect = c.Rect;
            foreach (Car c in cars)
            {
                //mapCanvas.Children.Add(new ShapeRectangle
                //{
                //    Height = c.Length,
                //    Width = c.Width,
                //    Margin = new Thickness(c.PosX, c.PosY, 0, 0),
                //    Stroke = c.Color,
                //    Fill = c.Color
                //});

                c.Draw(ref mapCanvas);
            }
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            _myMap.UpdateEnvironnement();
        }
    }
}
