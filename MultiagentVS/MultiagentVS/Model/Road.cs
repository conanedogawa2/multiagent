using System.Collections.Generic;

namespace MultiagentVS.Model
{
    public class Road
    {
        public Road(int length = 100)
        {
            this.Length = length;
            this.Cars = new HashSet<Car>();
        }

        public int Length { get; private set; }

        public HashSet<Car> Cars { get; private set; }

        public void AddCar(Car c)
        {
            Cars.Add(c);
        }

        public void RemoveCar(Car c)
        {
            Cars.Remove(c);
        }
    }
}