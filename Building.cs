using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class Building
    {
        /// <summary>
        /// Skapar en lista av Floors
        /// </summary>
        public List<Floor> Floors { get; private set; }
        public int AmountOfFloors { get; private set; }
        /// <summary>
        /// Konstruerar Building
        /// </summary>
        public Building(int _AmountOfFloors) 
        {
            Floors = ReadTxt.Instantiate(_AmountOfFloors); //Instansierar en lista av floors. Läser in vårt textdokument och omvandlar det till listor av passagerare.
        }
        /// <summary>
        /// En metod som kollar om processen är klar.
        /// </summary>
        /// <param name="floors"> Tar emot listan av floors. </param>
        /// <returns> Returnerar en bool som är true om det inte finns några passagerare kvar i listan floors. </returns>
        public bool IsDone(List<Floor> floors) 
        {
            foreach(Floor x in floors) 
            {
                if (x.PassengersOnFloor.Count != 0 )
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Denna metod används för att visualisera hissen och floors.
        /// </summary>
        /// <param name="floors"> Metoden tar emot listan floors. </param>
        /// <param name="elevator"> Metoden tar emot Elevatorn. </param>
        public void Visualize(List<Floor> floors, Elevator elevator)
        {
            for(int i = floors.Count - 1; i >= 0; i--)
            {
                Console.Write("Floor " + i);
                if (elevator.ElevatorPosition == i )
                {
                    Console.Write(" {" + elevator.PassengersInElevator.Count + "} ");
                }
                else
                {
                    Console.Write("     ");
                }
                if(floors[i].PassengersOnFloor.Count == 0)
                {
                    Console.Write("Empty");
                }
                foreach (Passenger passenger in floors[i])
                {
                    Console.Write(passenger.Destination + " ");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Denna metod används för att nå floors utanför building klassen.
        /// </summary>
        /// <param name="_Floors">Tar floor som parameter. </param>
        public void SetFloors(List<Floor> _Floors)
        {
            Floors = _Floors;
        }
    }
}
