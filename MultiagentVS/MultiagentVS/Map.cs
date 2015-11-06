using System;
using System.Collections.Generic;
using System.Net.Mime;
using MultiagentVS.Model;

namespace MultiagentVS
{
    //public delegate void MapUpdated(FishAgent[] _fish, List<BadZone> _obstacles);
    public delegate void MapUpdated();

    public class Map
    {
        public event MapUpdated mapUpdatedEvent;
        public static readonly short MAXCAR = 10;

        //public static TrafficLight TrafLight;// = new TrafficLight();
        public static XRoad XRoad { get; set; }

        private static int RoadBaseY => 100;

        public static List<Road> Roads = new List<Road>
        {
            new Road(Math.PI, RoadBaseY, 0, 800),
            new Road(0, RoadBaseY + Road.Height, 0),
            //new Road(Math.PI / 4, RoadBaseY + Road.Height, -10),
            new Road(Math.PI / -2, 0, 500, 0, 610),
            new Road(Math.PI / 2, 0, 500 - Road.Height, 0, 0)
        };

        static Random _randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Map(double _width, double _height)
        {
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;
            _randomGenerator = new Random();

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