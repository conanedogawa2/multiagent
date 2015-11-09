using System;
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
    //public delegate void DoUpdate();

    public class Road : ObjectInWorld
    {
        //public event DoUpdate doUpdateEvent;



        public static readonly Window Window = ((App)Application.Current).MainWindow;
        public PointF Middle
            => new PointF((float)(this.PosX + Length / 2), (float)(PosX - Length / 2));

        public float Length { get; private set; }

        public List<Car> Cars { get; set; }

        public double SensAngle { get; private set; }

        public double SensDegAngle
        {
            get { return (SensAngle/Math.PI)*180; }
            private set { SensAngle = (value/180)*Math.PI; }
        }

        public static readonly int Height = 30;

        public double CarX { get; private set; }
        public double CarY { get; private set; }

        public Car LastCar => Cars.LastOrDefault();

        public short GotYouBitchRoad { get; private set; }

        //public Canvas mapCanvas { get; private set; }

        public Road(short gotYouBitchRoad, double sensAngle, double posY, double posX, double carX = 0, double carY = 0, int length = 100)
        {
            this.PosY = posY;
            PosX = posX;
            SensAngle = sensAngle;
            GotYouBitchRoad = gotYouBitchRoad;
            Length = length;
            Cars = new List<Car>();

            CarX = carX.Equals(0) ? posX : carX;
            CarY = carY.Equals(0) ? posY : carY;

            //MainWindow win = (MainWindow) ((App) Application.Current).MainWindow;
            //mapCanvas = win.mapCanvas;
            
            //win.doDrawEvent += Draw;
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
            //int index = 0, max = Cars.Count;
            //Car c;
            //for (; index < max; index++)
            //{
            //    c = Cars.ElementAt(index);

            //    if (c.IsOutOfMap())
            //    {
            //        Cars.RemoveAt(index);
            //        Map.TotalCars -= 1;

            //        max--;

            //        if (max == index)
            //            return;

            //        index--;
            //    }
            //    else
            //        c.Update();
            //}
        }

        public override void Draw(Canvas parent)
        {
            ShapeRectangle mainRect = new ShapeRectangle
            {
                Height = Height,
                Width = Window.Width,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Fill = MediaBrush.DarkGray
            };

            MainWindow.RotateRectangle(ref mainRect, SensDegAngle, Middle);

            parent.Children.Add(mainRect);
        }
    }
}