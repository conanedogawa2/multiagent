using System;
using System.Collections.Generic;
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
        protected bool Equals(Car other)
        {
            return PosX.Equals(other.PosX) && PosY.Equals(other.PosY) && this._angle.Equals(other._angle) &&
                   this._width == other._width && this._speed == other._speed && this._length == other._length &&
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
                hashCode = (hashCode*397) ^ this._width;
                hashCode = (hashCode*397) ^ this._speed;
                hashCode = (hashCode*397) ^ this._length;
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

        private Random r = new Random();

        public Car(SolidColorBrush c, Road rd = null)
        {
            _speed = 2;
            _length = 10;
            _width = 25;
            PosX = 10;
            PosY = 100;
            _angle = 0;
            _color = c ?? Brushes.OrangeRed;


            _distance = 5;

            _park = null;
            _road = rd;
        }

        private double _angle;

        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int _speed;


        private int _length;

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }


        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set { _color = value; }
        }


        private int _distance;


        private CarPark _park;


        private Road _road;

        public PointF Middle
            => new PointF((float) ((this.PosX + this._length)/2), (float) ((this.PosX - this._length)/2));

        public StrucRectangle RectF => new StrucRectangle((float) PosX, (float) PosY, Width, Length);

        //public Rectangle Rect
        //{
        //    get { return new Rectangle(PosX, PosY, _); }
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
        }

        public void Update()
        {
            Advance();
        }

        private IEnumerable<Car> CollidingCars()
        {
            StrucRectangle rec = RectF;

            return Map.Voitures.Where(c => rec.IntersectsWith(c.RectF)).Where(c => !c.Equals(this)).ToList();
        }

        public void Draw(ref Canvas parent)
        {
            parent.Children.Add(new ShapeRectangle
            {
                Height = Length,
                Width = Width,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Stroke = Color,
                Fill = Color
            });
        }
    }
}