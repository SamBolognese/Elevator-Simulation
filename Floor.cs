using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class Floor : IEnumerable
    {
        public List<Passenger> PassengersOnFloor { get; private set; } // Lista av passagerare på våningen
        /// <summary>
        /// Konstruerar ett Floor
        /// </summary>
        /// <param name="passengersOnFloor"> Lista av passagerare på våningen </param>
        public Floor(List<Passenger> passengersOnFloor)
        {
            PassengersOnFloor = passengersOnFloor;
        }
        /// <summary>
        /// Denna metod är en form av felhantering som kontrollerar att antalet passagerare på en våning ej överskrider 50.
        /// </summary>
        /// <returns> Denna metod returnerar en bool beroende på om gränsen överskrids.</returns>
        public bool FloorIsFull() // Ska användas senare?
        {
            if (PassengersOnFloor.Count < 50)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Denna metod används för att ta bort den passagerare man ger som input.
        /// </summary>
        /// <param name="passenger"> Tar emot en passanger. </param>
        public void Remove(Passenger passenger)
        {
            PassengersOnFloor.Remove(passenger);
        }
        /// <summary>
        ///  Denna metod används för att loopa igenom en kollektion.
        /// </summary>
        /// <returns> Metoden returnerar en enumrator som används i loopar. </returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)PassengersOnFloor).GetEnumerator();
        }
    }
}
