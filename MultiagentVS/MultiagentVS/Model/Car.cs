using System.Drawing;
using System.Linq;

namespace MultiagentVS.Model
{
    public class Car : ObjectInWorld
    {
        public Car(Road rd)
        {
            this._speed = 3;
            this._length = 25;
            this._width = 50;
            this.PosX = 10;
            this.PosY = 100;

            this._color = Color.OrangeRed;
            this._distance = 5;

            _park = null;
            _road = rd;
        }

        public void Pivoter(int angle)
        {
            
        }


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


        private Color _color;


        private int _distance;


        private CarPark _park;


        private Road _road;

        //public Rectangle Rect
        //{
        //    get { return new Rectangle(PosX, PosY, _); }
        //}

        public void Advance(int step = 1)
        {
            //if (_road != null && _road.Cars.Any())
            //{

            //}
        }

        public void Update()
        {
            PosX += _speed;
        }
    }
}