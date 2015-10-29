using System;
using System.Collections.Generic;
using MultiagentVS.Model;

namespace MultiagentVS
{
    //public delegate void MapUpdated(FishAgent[] _fish, List<BadZone> _obstacles);
    public delegate void MapUpdated(Car voiture);

    public class Map
    {
        public event MapUpdated mapUpdatedEvent;

        private Car voiture = null;

        Random randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Map(double _width, double _height)
        {
            this.MAX_WIDTH = _width;
            this.MAX_HEIGHT = _height;
            this.randomGenerator = new Random();

            voiture = new Car(null);
        }

        public void UpdateEnvironnement()
        {
            this.UpdateCar();

            if (mapUpdatedEvent != null)
            {
                mapUpdatedEvent(this.voiture);
            }
        }

        private void UpdateCar()
        {
            voiture.Update();
        }
    }
}