using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Windows;
using MultiagentVS.Model;

namespace MultiagentVS
{
    //public delegate void MapUpdated(FishAgent[] _fish, List<BadZone> _obstacles);
    public delegate void MapUpdated();

    public class Map
    {
        public event MapUpdated mapUpdatedEvent;
        public static short MAXCAR = 15;

        //public static TrafficLight TrafLight;// = new TrafficLight();
        public static XRoad XRoad { get; set; }

        private static int RoadBaseY => 100;
        public static int TotalCars = 0;

        public static List<Road> Roads = new List<Road>
        {
            new Road(0, 0, RoadBaseY, 0)
            {
                Light = new TrafficLight(0)
                {
                    PosX = 435,
                    PosY = 85,
                    CurrentColor = 2
                }
            },
            new Road(1, 0, RoadBaseY + Road.Height, 0)
            {
                Light = new TrafficLight(0)
                {
                    PosX = 435,
                    PosY = 165,
                    CurrentColor = 2
                }
            },
            new Road(2, Math.PI / -2, 0, 500, 0, 610)
            {
                Light = new TrafficLight(Math.PI / -2)
                {
                    PosX = 535,
                    PosY = 165
                }
            },
            new Road(3, Math.PI / 2, 0, 500 - Road.Height)
            {
                Light = new TrafficLight(Math.PI / 2)
                {
                    PosX = 455,
                    PosY = 85
                }
            }
        };

        static Random _randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Map(double _width, double _height)
        {
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;
            _randomGenerator = new Random();

            //win.doUpdateEvent += UpdateEnvironnement;
            ((MainWindow) ((App)Application.Current).MainWindow).doUpdateEvent += UpdateEnvironnement;
        }

        //public void AddCarOnRoad(Car c, ref Road road)
        //{
        //    road.Cars.Add(c);
        //}

        public static int GetRandomInt(int inclMin, int extMax)
        {
            return _randomGenerator.Next(inclMin, extMax);
        }

        public void UpdateEnvironnement()
        {
            UpdateRoads();

            if (mapUpdatedEvent != null)
            {
                mapUpdatedEvent();
            }
        }

        private void UpdateRoads()
        {
            foreach (Road road in Roads)
                road.Update();
        }
    }
}