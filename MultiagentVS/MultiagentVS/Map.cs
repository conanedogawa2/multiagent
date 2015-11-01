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
        public static List<Car> Voitures = new List<Car>();

        Random _randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Map(double _width, double _height)
        {
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;
            _randomGenerator = new Random();

            Voitures = new List<Car>();
            //Voitures = new List<Car>
            //{
            //    { new Car(null) }
            //};

        }

        public void AddCar(Car c)
        {
            Voitures.Add(c);
            
            if(Voitures.Count >= MAXCAR)
                Voitures.RemoveAt(0);
        }

        public int GetRandomInt(int inclMin, int extMax)
        {
            return _randomGenerator.Next(inclMin, extMax);
        }

        public void UpdateEnvironnement()
        {
            UpdateCar();

            if (mapUpdatedEvent != null)
            {
                mapUpdatedEvent(Voitures);
            }
        }

        private void UpdateCar()
        {
            foreach (Car voiture in Voitures)
                voiture.Update();
        }
    }
}