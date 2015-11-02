using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using ShapeRectangle = System.Windows.Shapes.Rectangle;
using MediaBrush = System.Windows.Media.Brushes;


namespace MultiagentVS.Model
{
    public class Road : ObjectInWorld
    {
        public int Length { get; private set; }

        public List<Car> Cars { get; set; }

        public double SensAngle { get; private set; }

        public static readonly int Height = 30;

        public Car LastCar => Cars.LastOrDefault();

        public Road(double posY, double sensAngle, int length = 100)
        {
            this.PosY = posY;
            PosX = 0;
            SensAngle = sensAngle;
            Length = length;
            Cars = new List<Car>();
        }


        //public void AddCar(Car c)
        //{
        //    if (Cars.Count >= Height)
        //        return;

        //    Cars.Add(c);
        //}

        //public void RemoveCar(Car c)
        //{
        //    Cars.Remove(c);
        //}
        public void Update()
        {
            UpdateCars();
        }

        private void UpdateCars()
        {
            foreach (Car voiture in Cars)
                voiture.Update();
        }

        public override void Draw(ref Canvas parent)
        {
            parent.Children.Add(
                new ShapeRectangle
                {
                    Height = Height,
                    Width = MainWindow.Width,
                    Margin = new Thickness(PosX, PosY, 0, 0),
                    Fill = MediaBrush.DarkGray
                }
            );
        }
    }
}