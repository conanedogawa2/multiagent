using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace MultiagentVS.Model
{
    public class ObjectInWorld
    {
        public float PosX;

        public float PosY;

        protected readonly MainWindow _win = (MainWindow)Application.Current.MainWindow;

        public ObjectInWorld() { }

        public ObjectInWorld(float _x, float _y)
        {
            PosX = _x;
            PosY = _y;
        }

        public double SquareDistanceTo(double x, double y)
        {
            return (x - PosX) * (x - PosX) + (y - PosY) * (y - PosY);
        }

        public double DistanceTo(double x, double y)
        {
            return Math.Sqrt((x - PosX) * (x - PosX) + (y - PosY) * (y - PosY));
        }

        public double SquareDistanceTo(ObjectInWorld _object)
        {
            return SquareDistanceTo(_object.PosX, _object.PosX);
        }

        public double DistanceTo(ObjectInWorld _object)
        {
            return Math.Sqrt((_object.PosX - PosX) * (_object.PosX - PosX) + (_object.PosY - PosY) * (_object.PosY - PosY));
            //return Math.Sqrt( SquareDistanceTo(_object) );
        }

        public virtual void Draw(Canvas parent) { }
    }
}
