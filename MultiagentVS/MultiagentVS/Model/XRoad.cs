using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MultiagentVS.Model
{
    public class XRoad
    {
        public static TrafficLight[] trafLights { get; private set; }
        

        public XRoad()
        {
            trafLights = Map.Roads.Where(r => r.Light != null).Select(r => r.Light).ToArray();

            this.StartTrafficLights();
        }

        public static Rect Rect = new Rect(new Point(470, 100), new Point(530, 160));

        private void StartTrafficLights()
        {
            // TODO: event, maybe
            foreach (TrafficLight light in trafLights)
                light.Start();
        }

        public void Draw(Canvas parent)
        {
            foreach (TrafficLight light in trafLights)
                light.Draw(parent);

            //parent.Children.Add(new Rectangle
            //{
            //    Height = Rect.Height,
            //    Width = Rect.Width,
            //    Margin = new Thickness(Rect.X, Rect.Y, 0, 0),
            //    Stroke = Brushes.Chocolate,
            //    Fill = Brushes.Chocolate
            //});
        }
    }
}
