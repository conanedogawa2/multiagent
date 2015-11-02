using System;
using System.Collections.Generic;
using MultiagentVS.Model;

namespace MultiagentVS
{
    //public delegate void MapUpdated(FishAgent[] _fish, List<BadZone> _obstacles);
    public delegate void MapUpdated(IEnumerable<Car> voitures);

    public class Map
    {
        public event MapUpdated mapUpdatedEvent;
        public static readonly short MAXCAR = 10;

        private static int RoadBaseY => 100;
        public static Road RightToLeft = new Road(RoadBaseY, Math.PI);
        public static Road LeftToRight = new Road(RoadBaseY + Road.Height, 0);

        Random _randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Map(double _width, double _height)
        {
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;
            _randomGenerator = new Random();

        }

        public void AddCarOnRoad(Car c, ref Road road)
        {
            road.Cars.Add(c);
        }

        public int GetRandomInt(int inclMin, int extMax)
        {
            return _randomGenerator.Next(inclMin, extMax);
        }

        public void UpdateEnvironnement()
        {
            this.UpdateRoads();

            if (mapUpdatedEvent != null)
            {
                mapUpdatedEvent(LeftToRight.Cars);
            }
        }

        private void UpdateRoads()
        {
            LeftToRight.Update();
            RightToLeft.Update();
        }
    }
}