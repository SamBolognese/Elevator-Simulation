using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class ReadTxt
    {
        static string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())); //En string som hittar mappen med input.
        
        static string file = "/../D4.txt"; //Specifik fil i mappen med input

        public static List<string> RowsOfText = File.ReadAllLines(path + file).ToList(); // Lägger in input i en lista av strings
        public int TimeUnit = 0;
        /// <summary>
        /// Denna metod skapar 10 våningar och fyller dom med passagerare vid t0.
        /// </summary>
        /// <returns> Denna metod returnerar en populerad lista av våningar till vår kontruktor av Building </returns>
        public static List<Floor> Instantiate(int _AmountOfFloors) 
        {
            List<string> RowsOfText = File.ReadAllLines(path + file).ToList();
            List<Passenger> ListOfPassengers;
            int CurrentFloor = 0;
            List<Floor> ListOfFloors = new List<Floor>();

            for (int i = 0; i < _AmountOfFloors; i++)
            {
                try
                {
                    List<string> tempList = RowsOfText[i].Split(',').ToList();
                    ListOfPassengers = new List<Passenger>();
                    foreach (var number in tempList)
                    {
                        int.TryParse(number, out int Number);
                        if (Number >= 0 && Number != CurrentFloor && Number < _AmountOfFloors)
                        {
                        Passenger newPassenger = new Passenger(Number, CurrentFloor);
                        ListOfPassengers.Add(newPassenger);
                        }
                    }
                    ListOfFloors.Add(new Floor(ListOfPassengers));
                    CurrentFloor++;
                }
                catch
                {
                    break;
                }
            }
            return ListOfFloors;
        }
        ///Denna metod används för att fylla på passagerarna på floorsen efter varje loop. Int start används för att visa vilken indexposition forloopen ska börja på. Int End används för att visa vilken indexposition forloopen ska sluta på.  Här tar man in listan av floors för att populera den med passagerare.  Denna metod returnerar den populerade listan med passagerare. 
        public static List<Floor> RefillFloors(int start, int end, List<Floor> BuildingFloors, int AmountOfFloors)
        {
            List<string> RowsOfText = File.ReadAllLines(path + file).ToList();
            List<Passenger> ListOfPassengers;
            int CurrentFloor = 0;

            if(end > RowsOfText.Count)
            {
                return BuildingFloors;
            }
            for (int i = start; i < end; i++)
            {
                List<string> tempList = RowsOfText[i].Split(',').ToList();
                ListOfPassengers = new List<Passenger>();
                foreach (var number in tempList)
                {
                    int.TryParse(number, out int Number);
                    if (Number >= 0 && Number != CurrentFloor && Number < AmountOfFloors)
                    {
                        Passenger newPassenger = new Passenger(Number, CurrentFloor);
                        ListOfPassengers.Add(newPassenger);
                    }
                }
                foreach (Passenger passenger in ListOfPassengers)
                {
                    if(BuildingFloors[CurrentFloor].PassengersOnFloor.Count < 50) // Om våningen är full så lägger man inte till några nya passagerare
                    {
                        BuildingFloors[CurrentFloor].PassengersOnFloor.Add(passenger);
                    }
                }
                CurrentFloor++;
            }
            return BuildingFloors;
        }
    }
}

