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
        public CarMan()
        {
            int MsInterval = Map.GetRandomInt(100, 999);
            int SInterval = Map.GetRandomInt(0, 2);

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, SInterval, MsInterval);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            CreateCar();

            int MsInterval = Map.GetRandomInt(10, 999);
            int SInterval = Map.GetRandomInt(0, 1);
            ((DispatcherTimer) _sender).Interval = new TimeSpan(0, 0, 0, SInterval, MsInterval);
            ((DispatcherTimer)_sender).Start();
        }

        public static void CreateCar()
        {
            Road road;
            double carX = 0;
            int nbRoad = Map.Roads.Count;
            int rand = Map.GetRandomInt(0, nbRoad);
            var carColor = RandomBrush();

            road = Map.Roads[rand];
            if (rand == 0)
            {
                carX = Application.Current.MainWindow.Width;
            }


            Car frontCar = road.LastCar,
                car = new Car(carColor, road, carX, frontCar);

            Debug.WriteLine("------------Car " + car.Id + " created behind " + frontCar?.Id + " on road having PosY=" + road.PosY);
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
