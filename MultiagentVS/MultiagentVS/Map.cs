using System;
using System.Collections.Generic;
using MultiagentVS.Model;

namespace MultiagentVS
{
    //public delegate void MapUpdated(FishAgent[] _fish, List<BadZone> _obstacles);
    public delegate void MapUpdated();

    public class Map
    {
        public event MapUpdated mapUpdatedEvent;
        public static readonly short MAXCAR = 10;

        private static int RoadBaseY => 100;

        public static List<Road> Roads = new List<Road>
        {
            new Road(Math.PI, RoadBaseY),
            new Road(0, RoadBaseY + Road.Height),
            new Road(Math.PI / 4, RoadBaseY + Road.Height, -10)
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