using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiagentVS;
using MultiagentVS.Model;
using Brushes = System.Windows.Media.Brushes;
using ShapeRectangle = System.Windows.Shapes.Rectangle;


namespace UnitTestProject
{
    [TestClass]
    public class GlobalTest
    {
        private const double TestDistance = 201, SpeedMargeAcceptance = 0.05, DistanceMargeAcceptance = 0.5;
        private double _actualDistance, _actualDistance1;

        private void AssertDistances()
        {
            Debug.WriteLine("\t_actualDistance:" + _actualDistance
                //+ " _actualDistance1:" + _actualDistance1
                + " RefDistance:" + Car.RefDistance
                + " TestDistance:" + TestDistance
                + " Marge:" + DistanceMargeAcceptance);

            //Assert.IsTrue(_actualDistance >= Car.REF_DISTANCE - DistanceMargeAcceptance && _actualDistance <= Car.REF_DISTANCE + DistanceMargeAcceptance);
            //Assert.IsTrue(_actualDistance1 >= Car.REF_DISTANCE - DistanceMargeAcceptance && _actualDistance1 <= Car.REF_DISTANCE + DistanceMargeAcceptance);
            
            //Assert.AreEqual(TestDistance, this._actualDistance1);
            //Assert.AreEqual(this._actualDistance, this._actualDistance1);
        }

        [TestMethod]
        public void TestDistanceTo()
        {
            //short distMax = 800, speed = 2;

            ////Car c = new Car(null)
            ////{
            ////    PosX = 0,
            ////    PosY = 0,
            ////    Speed = speed
            ////};

            ////Car c1 = new Car(null, c)
            ////{
            ////    PosX = -1 * TestDistance,
            ////    PosY = 0,
            ////    Speed = speed
            ////};

            //Map.LeftToRight.Cars.Add(c);
            //Map.LeftToRight.Cars.Add(c1);

            //this._actualDistance = c.DistanceTo(c1);
            //this._actualDistance1 = c1.DistanceTo(c);
            //this.AssertDistances();

            //Debug.WriteLine("");

            //for (int i = 0; i < distMax; i++)
            //{
            //    //Debug.WriteLine(DebugString(c, i, _actualDistance));
            //    Debug.WriteLine(DebugString(c1, i, _actualDistance1));
            //    c.Advance();
            //    c1.Advance();

            //    Assert.IsTrue(c.Speed >= speed - SpeedMargeAcceptance && c.Speed <= speed + SpeedMargeAcceptance);
            //    Assert.IsTrue(c1.Speed >= speed - SpeedMargeAcceptance && c1.Speed <= speed + SpeedMargeAcceptance);

            //    Debug.WriteLine("\t" + (speed - SpeedMargeAcceptance) + " <= " + c1.Speed + " >= " + speed + SpeedMargeAcceptance);
                
            //    //Assert.AreEqual(speed, c.Speed);
            //    //Assert.AreEqual(c.Speed, c1.Speed);

            //    this._actualDistance = c.DistanceTo(c1);
            //    this._actualDistance1 = c1.DistanceTo(c);
            //    this.AssertDistances();
            //}
        }

        private string DebugString(Car c, int i, double distance)
        {
            return "[" + i + "] " + "x:" + c.PosX + " y:" + c.PosY + " speed:" + c.Speed + " distToFront:" + distance;
        }

        [TestMethod]
        public void TestAdaptSpeed()
        {
            //short x1 = 10, y1 = 10, speed = 200;

            //Car c = new Car(null)
            //{
            //    PosX = 0,
            //    PosY = 0,
            //    Speed = speed
            //};

            //Car c1 = new Car(null)
            //{
            //    PosX = x1,
            //    PosY = y1
            //};

            //double dist = c.SquareDistanceTo(c1);
            //Assert.AreEqual(dist, x1*x1 + y1*y1);

            //c.Advance();
            ////Assert.AreEqual(c.Speed, speed
            ////PointF target = new PointF(10, 10);
            ////var ditance = c.SquareDistanceTo()

        }

        [TestMethod]
        public void TestRotate()
        {
            float Height = 10, Length = 50, PosX = 10, PosY = 10;
            PointF Middle = new PointF(PosX + Length / 2, PosY + Height / 2);

            ShapeRectangle mainRect = new ShapeRectangle
            {
                Height = Height,
                Width = Length,
                Margin = new Thickness(PosX, PosY, 0, 0)
            },
                smallRect = new ShapeRectangle
                {
                    Height = 10,
                    Width = 10,
                    Margin = new Thickness(PosX + Length - 10, PosY, 0, 0)
                };

            MainWindow.RotateRectangle(ref mainRect, 90, Middle);
            MainWindow.RotateRectangle(ref smallRect, 90, Middle);
        }
    }
}
