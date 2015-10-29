using System.Collections.Generic;

namespace MultiagentVS.Model
{
    public class CarPark
    {
        public CarPark(int maxPlace)
        {
            this.MaxPlace = maxPlace;
            this.Cars = new List<Car>();
        }

        public int MaxPlace { get; private set; }

       
        public List<Car> Cars { get; private set; }
    }
}