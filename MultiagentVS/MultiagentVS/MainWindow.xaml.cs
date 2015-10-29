using System;
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
        }

        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("---- MOUSE DOWN ----"); 
        }

        void myMap_mapUpdatedEvent(Car v)
        {
            mapCanvas.Children.Clear();

            DrawCar(v);

            mapCanvas.UpdateLayout();
        }

        private void DrawCar(Car c)
        {
            //Rectangle rect = c.Rect;

            mapCanvas.Children.Add(new ShapeRectangle
            {
                Height = c.Length,
                Width = c.Width,
                Margin = new Thickness(c.PosX, c.PosY, 0, 0),
                Stroke = Brushes.OrangeRed,
                Fill = Brushes.OrangeRed
            });
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            _myMap.UpdateEnvironnement();
        }
    }
}
