using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class Passenger 
    {
        public int Destination { get; private set; }
        public int CurrentFloor { get; private set; }
        public int TimeWaiting { get; private set; }
        public int TimeTotal { get; private set; }
        /// <summary>
        /// Konstuktorn för passenger. Här sätts timeWaiting till 0 och time total till 0.
        /// </summary>
        /// <param name="destination"> Här tar man in en integer som representerar dit passageraren vill åka. </param>
        /// <param name="currentFloor"> Här tar man in en int som representerar vilken floor passageraren beffinner sig på </param>
        public Passenger(int destination, int currentFloor)
        {
            Destination = destination;
            TimeWaiting = 0;
            TimeTotal = 0;
            CurrentFloor = currentFloor;

        }
        /// <summary>
        ///  Jämför Destination med CurrentFloor för att ta reda på vilken riktning passageraren ska åka. 
        /// </summary>
        /// <returns> Metoden returnerar riktningen som representeras av en bool. </returns>
        public bool DirectionOfPassengers()
        {
            if (Destination > CurrentFloor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// En metod för att öka TimeWaiting med 10.
        /// </summary>
        public void IncreaseTimeWaiting()
        {
            TimeWaiting += 10;
        }
        /// <summary>
        /// En metod för att öka TimeTotal med 10.
        /// </summary>
        public void IncreaseTimeTotal()
        {
            TimeTotal += 10;
        }
    }
}
