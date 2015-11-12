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
using Point = System.Windows.Point;
using StrucRectangle = System.Drawing.RectangleF;
using ShapeRectangle = System.Windows.Shapes.Rectangle;


namespace MultiagentVS.Model
{
    public class Car : ObjectInWorld
    {
        public static short CPT = -1;
        public static readonly Window Window = ((App) Application.Current).MainWindow;

        //private const double DistanceMargeAcceptance = 0.5;
        public const double RefDistance = 300, MinDistance = 140;

        public readonly short Id;

        protected bool Equals(Car other)
        {
            return PosX.Equals(other.PosX) && PosY.Equals(other.PosY) && this._angle.Equals(other._angle) &&
                   this._length == other._length && this._speed.Equals(other._speed) && this._height == other._height &&
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
                hashCode = (hashCode*397) ^ _length;
                hashCode = (hashCode*397) ^ (int)_speed;
                hashCode = (hashCode*397) ^ _height;
                hashCode = (hashCode*397) ^ (_color?.GetHashCode() ?? 0);
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

        public Car(SolidColorBrush c, Road rd, Car frontCar = null)
        {
            _speed = 2;
            _height = 10;
            _length = 25;
            PosX = rd.CarX + Road.Height / (float)2;
            PosY = rd.CarY + Road.Height / (float)2;
            _color = c ?? Brushes.OrangeRed;

            _park = null;

            _road = rd;
            _angle = rd.SensAngle;

            this.FrontCar = frontCar;

            ++CPT;
            Id = CPT;

            MainRect = new ShapeRectangle
            {
                Height = this.Height,
                Width = this.Length,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Stroke = BorderColor,
                Fill = Color
            };

            rd.Cars.Add(this);

            //((MainWindow)((App)Application.Current).MainWindow).doDrawEvent += Draw;
        }

        private double _angle;
        public double Angle { get { return _angle; } set { _angle = value % (2 * Math.PI); } }

        public double DegAngle
        {
            get { return _angle*180/Math.PI; }
            set { _angle = value*Math.PI/180; }
        }

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

        private SolidColorBrush _borderColor;
        public SolidColorBrush BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
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
            {
                _speed = 2*(float) distance/200;

                if (distance <= MinDistance/* || (FrontCar != null && FrontCar.Speed.Equals(0))*/)
                    _speed = 0;
            }
        }

        private CarPark _park;
        private bool LightPassed = false;
        private double DistToLight = 1000;
        private Road _road;

        public PointF Middle
        {
            get { return new PointF((float) ((this.PosX + this._height)/2), (float) ((this.PosX - this._height)/2)); }
        }

        public StrucRectangle RectF => new StrucRectangle((float)PosX, (float)PosY, this.Length, this.Height);
        //public ShapeRectangle RectF => new ShapeRectangle()
        //public static short Id
        //{
        //    get
        //    {
        //        return id;
        //    }
        //}

        private bool ChangedRoad = false;

        public void Advance(float step = (float) 0.8)
        {
            //if (IsOutOfMap())
            //{

            //}
            Road closeRoad = null;
            
            if((_angle.Equals(Math.PI / 2) || _angle.Equals(Math.PI / -2)) && !ChangedRoad)
                closeRoad = NearXRoad();

            if (closeRoad != null)
            {
                Debug.WriteLine("close to road : " + closeRoad);
                ChangeRoad(closeRoad);
            }

            PosX += (float)Math.Cos(_angle)*_speed*step;
            PosY += (float)Math.Sin(_angle)*_speed*step;

            IEnumerable<Car> list = this.CollidingCars();

            BorderColor = Color;
            if (list.Any())
                BorderColor = Brushes.Red;


            double distanceToLight = HandleLight();


            double distanceToCar = 1000;

            if (FrontCar != null)
                distanceToCar = DistanceTo(FrontCar);

            if (distanceToLight <= DistToLight)
                DistToLight = distanceToLight;
            else
                LightPassed = true;


            AdaptSpeed(LightPassed || distanceToCar < distanceToLight ? distanceToCar : distanceToLight );
        }

        //private bool IsOverRoad(Road r)
        //{
        //    bool ret = false;
        //    Rect roadRectangle = new Rect(r.MainRectangle.RenderSize);
        //    Rect thisRectangle = new Rect(MainRect.RenderSize);

        //    if (roadRectangle.Contains(new Point(PosX, PosY)))
        //        ret = true;

        //    //Rect roadRectangle = new Rect()


        //    return ret;
        //}

        private void ChangeRoad(Road road)
        {
            road.Cars.Add(this);
            _road.Cars.Remove(this);

            _road = road;

            if (_angle.Equals(Math.PI/-2))
                PosY -= 15;
            else if (_angle.Equals(Math.PI / 2))
                PosY += 15;

            _angle = road.SensAngle;

            // changer la voiture de devant
            float dist = 1000;
            double tmpDist;
            Car[] carz = road.Cars.ToArray();
            Car closest = null;

            // get closest car
            foreach (Car car in carz)
            {
                if (car.Equals(this))
                    continue;

                tmpDist = DistanceTo(car);
                if (tmpDist < dist)
                {
                    dist = (float) tmpDist;
                    closest = car;
                }
            }

            Car prevCar = _road.Cars.FirstOrDefault(c => c.FrontCar == closest);
            if(prevCar != null)
                prevCar.FrontCar = this;
            FrontCar = closest;

            ChangedRoad = true;
        }

        private Road NearXRoad()
        {
            Road[] roadz = Map.Roads.Where(r => r != _road && !r.SensAngle.Equals(_angle)).ToArray();
            //StrucRectangle sr = new StrucRectangle(PosX, PosX, Length, Height);

            //Road ro = roadz.FirstOrDefault(road => road.StructRect.IntersectsWith(sr));
            Road ro = null;

            //foreach (Road road in roadz)
            //{
            //    //if (road.StructRect.IntersectsWith(this.RectF))
            //    if (IsOverRoad(road))
            //    {
            //        ro = road;
            //        break;
            //    }
            //}

            //if (ro != null)
            //    Debug.WriteLine("its okay ;)");

            PointF middle = new PointF((float)((this.PosX + this._height) / 2), (float)((this.PosX - this._height) / 2));


            Debug.WriteLine(middle.ToString());
            if (XRoad.Rect.Contains(new Point(PosX, PosY)))
            {
                Debug.WriteLine("ok bro");

                ro = roadz.ElementAt(0);
                if (PosY < XRoad.Rect.Top + XRoad.Rect.Height)
                    ro = roadz.ElementAt(1);
            }

            return ro;
        }

        private double HandleLight()
        {
            double distance;
            TrafficLight[] lights = XRoad.trafLights.Where(l => l.Angle.Equals(Angle)).ToArray();

            TrafficLight light = _road.Light;
            distance = DistanceTo(light);

            if (distance <= RefDistance
                && (light.CurrentColor == 0 || light.CurrentColor == 2))
            {
                return distance;
            }

            return 999;
        }

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

        public ShapeRectangle MainRect { get; private set; }


        public override void Draw(Canvas parent)
        {
            MainRect = new ShapeRectangle
            {
                Height = this.Height,
                Width = this.Length,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Stroke = BorderColor,
                Fill = Color
            };

            MainWindow.RotateRectangle(MainRect, DegAngle, Middle);

            parent.Children.Add(MainRect);
        }

        public bool IsOutOfMap()
        {
            return PosX < 0 || PosX > Window.Width || PosY < 0 || PosY > Window.Height;
        }
    }
}