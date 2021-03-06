﻿using System;
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

    //public delegate void RemoveCar(Car c);

    //public delegate void DoDraw(Canvas mapCanvas);

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static Params Parameters = new Params();

        //static int FPS = Parameters.FPS;
        //public static readonly int Width = 800;
        //public static readonly int Height = 600;
        Map _myMap;
        CarMan _cm;
        public event DoUpdate doUpdateEvent;
        //public event RemoveCar removeCarEvent;

        //public event DoDraw doDrawEvent;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            IsRunning = false;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }



        void MainWindow_Loaded(object _sender, RoutedEventArgs _e)
        {
            mapCanvas.MouseDown += mapCanvas_MouseDown;
            KeyUp += mapCanvas_KeyUp;

            _myMap = new Map(mapCanvas.ActualWidth, mapCanvas.ActualHeight);
            _myMap.mapUpdatedEvent += myMap_mapUpdatedEvent;


            
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
        }

        private ParamWindow _paramWindow = null;

        private void ShowParamWindow()
        {
            if (_paramWindow != null)
                return;

            _paramWindow = new ParamWindow(Parameters);
            _paramWindow.ParamsChangedEvent += Okay;
            _paramWindow.Show();
        }

        private void Okay(ParamWindow sender, Params parameters)
        {
            Parameters = parameters;

            _paramWindow?.Close();
            _paramWindow = null;
            Resume();
        }

        private void mapCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            Key k = e.Key;

            switch (k)
            {
                case Key.Enter:
                    TriggerRunning();
                    break;
                case Key.P:
                    Pause();
                    ShowParamWindow();
                    break;
            }
        }

        private void mapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_cm == null)
            {
                _cm = new CarMan();
                //Map.TrafLight = new TrafficLight();
                Map.XRoad = new XRoad();
                //_dispatcherTimer.Start();
                Resume();
            }
        }

        void myMap_mapUpdatedEvent()
        {
            mapCanvas.Children.Clear();

            this.DrawMap();
            //doDrawEvent?.Invoke(mapCanvas);
            
            mapCanvas.UpdateLayout();
        }

        public bool IsRunning { get; private set; }

        public void Pause()
        {
            _dispatcherTimer.Stop();
            _cm?.Pause();
            IsRunning = false;
        }

        public void TriggerRunning()
        {
            if (IsRunning)
                Pause();
            else
                Resume();
        }

        public void Resume()
        {
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Parameters.FPS);
            _dispatcherTimer.Start();
            _cm?.Start();
            IsRunning = true;
        }

        private void DrawMap(/*IEnumerable<Car> cars*/)
        {
            List<Car> cars = new List<Car>();

            if(cars.Count > 15)
                throw new Exception("Should not happen");


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
                    Map.TotalCars -= 1;

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

            mapCanvas.Children.Add(new TextBox
            {
                Text = Parameters.FPS + " FPS",
                Foreground = Brushes.DarkCyan
            });
        }

        public static void RotateRectangle(ShapeRectangle rec, double angle, PointF middle)
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
