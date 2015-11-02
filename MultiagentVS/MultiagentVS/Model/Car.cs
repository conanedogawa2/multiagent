using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using StrucRectangle = System.Drawing.RectangleF;
using ShapeRectangle = System.Windows.Shapes.Rectangle;


namespace MultiagentVS.Model
{
    public class Car : ObjectInWorld
    {
        public static short CPT = -1;

        //private const double DistanceMargeAcceptance = 0.5;
        public const double RefDistance = 200;

        public readonly short Id;

        protected bool Equals(Car other)
        {
            return PosX.Equals(other.PosX) && PosY.Equals(other.PosY) && this._angle.Equals(other._angle) &&
                   this._length == other._length && this._speed == other._speed && this._height == other._height &&
                   Equals(this._color, other._color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Car) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this._angle.GetHashCode();
                hashCode = (hashCode*397) ^ this._length;
                hashCode = (hashCode*397) ^ (int)this._speed;
                hashCode = (hashCode*397) ^ this._height;
                hashCode = (hashCode*397) ^ (this._color != null ? this._color.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Car left, Car right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Car left, Car right)
        {
            return !Equals(left, right);
        }

        private Random _r = new Random();

        public Car(SolidColorBrush c, Road rd, float posX = 0, Car frontCar = null)
        {
            _speed = 2;
            this._height = 10;
            this._length = 25;
            PosX = posX;
            PosY = rd.PosY + Road.Height / (float)2;
            _angle = rd.SensAngle;
            _color = c ?? Brushes.OrangeRed;

            _park = null;

            _road = rd;

            this.FrontCar = frontCar;

            ++CPT;
            Id = CPT;

            if (rd != null)
                rd.Cars.Add(this);
        }

        private double _angle;

        private int _length;

        private Car FrontCar { get; set; }

        public int Length
        {
            get { return this._length; }
            set { this._length = value; }
        }


        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        private int _height;

        public int Height
        {
            get { return this._height; }
            set { this._height = value; }
        }


        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Set la vitesse en fonction d'une distance
        /// </summary>
        /// <param name="distance">Distance avec le vehicule de devant</param>
        private void AdaptSpeed(double distance)
        {
            distance += 100;

            _speed = 2;

            if (distance < 200)
                _speed = 2 * (float)distance / 200;
        }

        private CarPark _park;

        private Road _road;

        public PointF Middle
            => new PointF((float) ((this.PosX + this._height)/2), (float) ((this.PosX - this._height)/2));

        public StrucRectangle RectF => new StrucRectangle((float) PosX, (float) PosY, this.Length, this.Height);

        //public static short Id
        //{
        //    get
        //    {
        //        return id;
        //    }
        //}

        public void Advance(float step = (float) 0.8)
        {
            //int rand = r.Next(0,  20);

            //if (rand == 0)
            //    _angle = r.Next(0, 2 * (int)Math.PI);

            PosX += Math.Cos(_angle)*_speed*step;
            PosY += Math.Sin(_angle)*_speed*step;

            IEnumerable<Car> list = this.CollidingCars();

            Color = Brushes.Aqua;

            if (list.Any())
                Color = Brushes.Red;

            if(this.FrontCar != null)
                AdaptSpeed( DistanceTo(FrontCar) );
        }

        // why internal?
        //internal double DistanceToCar(Car c)
        //{

        //}

        //internal double DistanceToPoint(double wallXMin, double wallYMin, double wallXMax, double wallYMax)
        //{
        //    double min = double.MaxValue;
        //    min = Math.Min(min, this.PosX - wallXMin);
        //    min = Math.Min(min, this.PosY - wallYMin);
        //    min = Math.Min(min, wallYMax - this.PosY);
        //    min = Math.Min(min, wallXMax - this.PosX);
        //    return min;
        //}

        //internal double DistanceToPoint(double x, double y)
        //{
        //    double squaredDist = 0;

        //    squaredDist = (PosX - x)*(PosX - x) + (PosY - y)*(PosY - y);

        //    return squaredDist;
        //}

        public void Update()
        {
            Advance();
        }

        /// <summary>
        /// Retourne une liste des objets 'Car' en collision
        /// </summary>
        /// <returns> IEnumerable<Car/> </returns>
        private IEnumerable<Car> CollidingCars()
        {
            StrucRectangle rec = RectF;

            return _road.Cars.Where(c => rec.IntersectsWith(c.RectF)).Where(c => !c.Equals(this)).ToList();
        }

        public override void Draw(ref Canvas parent)
        {
            parent.Children.Add(new ShapeRectangle
            {
                Height = this.Height,
                Width = this.Length,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Stroke = Color,
                Fill = Color
            });
            parent.Children.Add(new ShapeRectangle
            {
                Height = 10,
                Width = 10,
                Margin = new Thickness(PosX + Length - 10, PosY, 0, 0),
                Stroke = Brushes.Red,
                Fill = Brushes.Red
            });
        }

        public bool IsOutOfMap()
        {
            return PosX < 0 || PosX > MainWindow.Width || PosY < 0 || PosY > MainWindow.Height;
        }
    }
}