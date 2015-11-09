using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultiagentVS.Model
{
    public class XRoad
    {
        public static TrafficLight[] trafLights { get; private set; }
        

        public XRoad()
        {
            trafLights = new TrafficLight[2]{
                new TrafficLight(Math.PI / -2)
                {
                    PosX = 535,
                    PosY = 165
                },
                new TrafficLight(Math.PI / 2)
                {
                    PosX = 455,
                    PosY = 85
                }
            };
            //((MainWindow)((App)Application.Current).MainWindow).doDrawEvent += Draw;

            this.StartTrafficLights();
        }

        private void StartTrafficLights()
        {
            // TODO: event
            foreach (TrafficLight light in trafLights)
                light.Start();
        }

        public void Draw(Canvas parent)
        {
            foreach (TrafficLight light in trafLights)
                light.Draw(parent);
        }
    }
}
