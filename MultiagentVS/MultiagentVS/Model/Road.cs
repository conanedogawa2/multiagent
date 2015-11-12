using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Shapes;
using ShapeRectangle = System.Windows.Shapes.Rectangle;
using StrucRectangle = System.Drawing.RectangleF;
using MediaBrush = System.Windows.Media.Brushes;


namespace MultiagentVS.Model
{

    public class Road : ObjectInWorld
    {
        
        public static readonly Window Window = ((App)Application.Current).MainWindow;
        public PointF Middle
            => new PointF((float)(this.PosX + Length / 2), (float)(PosX - Length / 2));

        public float Length { get; private set; }

        public TrafficLight Light { get; set; }

        private List<Car> _cars;
        public List<Car> Cars { get { return _cars; } set { _cars = value; } }

        public double SensAngle { get; private set; }

        public double SensDegAngle
        {
            get { return (SensAngle/Math.PI)*180; }
            private set { SensAngle = (value/180)*Math.PI; }
        }

        public static readonly int Height = 30;

        public float CarX { get; private set; }
        public float CarY { get; private set; }

        public Car LastCar => Cars.LastOrDefault();

        public short GotYouBitchRoad { get; private set; }

        //public Canvas mapCanvas { get; private set; }

        public Road(short gotYouBitchRoad, double sensAngle, float posY, float posX, float carX = 0, float carY = 0, int length = 100, float width = 0)
        {
            this.PosY = posY;
            PosX = posX;
            SensAngle = sensAngle;
            Width = width;
            GotYouBitchRoad = gotYouBitchRoad;
            Length = length;
            Cars = new List<Car>();

            CarX = carX.Equals(0) ? posX : carX;
            CarY = carY.Equals(0) ? posY : carY;

            this.Width = width.Equals(0) ? (float)Window.Width : width;
        }

        private void WinOnRemoveCarEvent(Car car)
        {
            Cars.Remove(car);
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

        public override string ToString()
        {
            return "[Road] n°" + GotYouBitchRoad + ", " + SensDegAngle + "°";
        }

        private void UpdateCars()
        {
            if (Cars == null || Cars.Count == 0)
                return;

            Car[] tmpCarz = Cars.ToArray();

            foreach (Car voiture in tmpCarz)
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

        public float Width { get; private set; }

        public StrucRectangle StructRect => new RectangleF(PosX, PosY, Width, Height);

        public ShapeRectangle MainRectangle { get; private set; }

        public override void Draw(Canvas parent)
        {
            MainRectangle = new ShapeRectangle
            {
                Height = Height,
                Width = Width,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Fill = MediaBrush.DarkGray
            };
            //ShapeRectangle testRect = new ShapeRectangle
            //{
            //    Height = Height,
            //    Width = Width,
            //    Margin = new Thickness(PosX, PosY, 0, 0),
            //    Fill = MediaBrush.Red
            //};

            MainWindow.RotateRectangle(MainRectangle, SensDegAngle, Middle);

            parent.Children.Add(MainRectangle);
            //parent.Children.Add(testRect);
        }
    }
}