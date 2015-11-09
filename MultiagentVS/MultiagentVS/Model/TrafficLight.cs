using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shapes;


namespace MultiagentVS.Model
{
    public class TrafficLight : ObjectInWorld
    {
        public SolidColorBrush[] Colors { get; set; }

        public DispatcherTimer DispatcherTimer { get; set; }

        public int CurrentColor { get; set; }

        public static readonly float Height = 10, Width = 10;
        public double Angle { get; private set; }

        public TrafficLight(double angle)
        {
            this.Angle = angle;
            Colors = new SolidColorBrush[3]
            {
                Brushes.Red,
                Brushes.Green,
                Brushes.Orange
            };

            CurrentColor = 0;
            //((MainWindow)((App)Application.Current).MainWindow).doDrawEvent += Draw;
        }

        private const int SInterval = 5;

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CurrentColor = (CurrentColor + 1) % 3;
            //CurrentColor += 1;
        }

        public override void Draw(Canvas parent)
        {
            // TODO: ref to the ellipse
            Ellipse mainRect = new Ellipse
            {
                Height = Height,
                Width = Width,
                Margin = new Thickness(PosX, PosY, 0, 0),
                Stroke = Colors[CurrentColor],
                Fill = Colors[CurrentColor]
            };

            parent.Children.Add(mainRect);
        }

        public void Start()
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 0, SInterval, 0);
            DispatcherTimer.Start();
        }
    }
}
