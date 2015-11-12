using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MultiagentVS.Model
{
    public class CarMan
    {
        public int MsInterval { get; set; }
        public int SInterval { get; set;}

        private static DispatcherTimer _dispatcherTimer = new DispatcherTimer();


        public CarMan()
        {
            MsInterval = Map.GetRandomInt(0, 999);
            SInterval = Map.GetRandomInt(1, 5);

            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, SInterval, MsInterval);
            _dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            CreateCar();

            MsInterval = Map.GetRandomInt(10, 999);
            SInterval = Map.GetRandomInt(0, 1);

            ((DispatcherTimer) _sender).Interval = new TimeSpan(0, 0, 0, SInterval, MsInterval);
            ((DispatcherTimer)_sender).Start();

            //_dispatcherTimer.Stop();
        }

        private static void CreateCar()
        {
            if (Map.TotalCars >= Map.MAXCAR)
                return;

            Road road;
            double carX = 0;
            int nbRoad = Map.Roads.Count;
            int rand = Map.GetRandomInt(0, nbRoad);
            var carColor = RandomBrush();

            road = Map.Roads[rand];
            //road = Map.Roads[3];

            Map.TotalCars ++;

            // do road.AddCar
            Car frontCar = road.LastCar,
                car = new Car(carColor, road, frontCar);

            //Debug.WriteLine("------------Car " + car.Id + " created behind " + frontCar?.Id + " on road having PosY=" + road.PosY);
        }

        private static SolidColorBrush RandomBrush()
        {
            var r = new Random();
            var properties = typeof(Brushes).GetProperties();
            var count = properties.Count();

            var colour = properties
                        .Select(x => new { Property = x, Index = r.Next(count) })
                        .OrderBy(x => x.Index)
                        .First();

            return (SolidColorBrush)colour.Property.GetValue(colour, null);
        }
    }
}
