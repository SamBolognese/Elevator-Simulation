
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class Elevator
    {
        public List<Passenger> PassengersInElevator { get; private set; } // En lista av passagerare i hissen.
        public List<Passenger> PassengersDoneTravelling { get; private set; } // En lista av passagerare som har anlänt till sin destination.
        public int ElevatorPosition { get; private set; } // En int som säger vilken våning hissen är på.
        public bool DirectionOfElevator { get; private set; } // En bool som säger om hissen är påväg nedåt eller uppåt.
        public int TimeTraveled { get; private set; } // En int som räknar hissens totala färdtid tills simulationens slut.
        public int TotalAmountOfPassengers { get; private set; } // En int som säger hur många passagerare som har åkt med hissen.
        /// <summary>
        /// Konstruerar hissen
        /// </summary>
        public Elevator()
        {
            PassengersInElevator = new List<Passenger>(); 
            PassengersDoneTravelling = new List<Passenger>();
            ElevatorPosition = 0;
            DirectionOfElevator = true;
            TotalAmountOfPassengers = 0;
        }
        /// <summary>
        /// Denna metod kör hissen till toppen (om det finns passagerare där) där den ändrar bolens värde och vänder. Sedan åker den till bottenvåningen och vänder. 
        /// </summary>
        /// <param name="DirectionOfElevator"> Tar emot ovannämnda boolen som input </param>
        /// <param name="Hotel"> Tar emot en instansiering av en Building vid namn Hotel. Som input</param>
        public void Move(bool DirectionOfElevator, int AmountOfFloors) // Flyttar hissen upp eller ner
        {
            TimeTraveled++;
            if (DirectionOfElevator)
            {
                if(ElevatorPosition != AmountOfFloors)
                {
                   ElevatorPosition++;
                }
            }
            else if (!DirectionOfElevator)
            {
                if(ElevatorPosition != 0)
                {
                    ElevatorPosition--;
                }
            }
        }
        /// <summary>
        /// Denna metod ändrar hissens färdriktning om ingen passagerare ska i den färdriktningen.
        /// </summary>
        /// <param name="Hotel"> Tar emot instansiering av Building vid namn Hotel som input. </param>
        public void ChangeDirectionIfEmptyAhead(Building Hotel) // En metod som byter rikting på hissen om ingen i hissen ska i hissens nuvarande rikning samt om våningarna i hissens nuvarande riktning är tomma.
            { 
            Passenger[] TempElevatorArray = new Passenger[PassengersInElevator.Count()]; // Skapar en kopia av hissen.
            PassengersInElevator.CopyTo(TempElevatorArray); //Kopierar över passagerarna till hiss-kopian.
            bool ElevatorDirBool = true; //Hållbytesvariabel
            bool FloorDirBool = true; //Hållbytesvariabel
            if (DirectionOfElevator) //Om hissen åker upp;
            {
                if(PassengersInElevator.Count > 0)
                {
                    foreach (Passenger passenger in  TempElevatorArray) //Loop som kollar om någon i hissen ska upp.
                    {
                        if (passenger.Destination > ElevatorPosition) //Om nån ska upp sätter den hållbytesvariabeln till falsk.
                        {
                            ElevatorDirBool = false;
                        }
                    }
                }
                for (int i = ElevatorPosition + 1; i < Hotel.Floors.Count; i++)//Loop som kollar om det står någon på våningarna över hissen.
                {
                    if (Hotel.Floors[i].PassengersOnFloor.Count != 0)//Om det står någon på någon av våningarna över hissen sätter den hållbytesvariabeln till falsk.
                    {
                        FloorDirBool = false;
                    }
                }
                if (ElevatorDirBool && FloorDirBool)//Om båda hållbytesvariablerna vänder hissen ner.
                {
                    DirectionOfElevator = false;
                }
            }
            else
            {
                if(PassengersInElevator.Count > 0)
                {
                    foreach (Passenger passenger in  TempElevatorArray) //Loop som kollar om någon i hissen ska ner.
                    {
                        if (passenger.Destination < ElevatorPosition) //Om någon ska ner sätter den hållbytesvariabeln till falsk.
                        {
                            ElevatorDirBool = false;
                        }
                    }
                }
                for (int i = ElevatorPosition - 1; i >= 0; i--) //Loop som kollar om det står någon på våningarna under hissen.
                {
                    if (Hotel.Floors[i].PassengersOnFloor.Count != 0) //Om det står någon på någon av våningarna under hissen sätter den hållbytesvariabeln till falsk.
                    {
                        FloorDirBool = false;
                    }
                }
                if (ElevatorDirBool && FloorDirBool) //Om båda hållbytesvariablerna vänder hissen upp.
                {
                    DirectionOfElevator = true;
                }
            }
         }
        /// <summary>
        ///  En metod som tar hand om all pålastning och avlastning av passagerare. Vi börjar med att lasta av passagerare. Om hissen fortfarande är full så körs Return rad 137, annars fylls hissen på med for-each loopen rad 141-153.
        /// </summary>
        /// <param name="Hotel"> Tar emot instansiering av Building vid namn Hotel som input. </param>
        public void PassengerHandling(Building Hotel) 
        {
            Passenger[] TempElevatorArray = new Passenger[PassengersInElevator.Count()]; //Eftersom det inte går att iterera igenom en kollektion, samtidigt som man ändrar den, görs en temporär array.
            PassengersInElevator.CopyTo(TempElevatorArray); //Listan kopieras till den temporära arrayen.
            if (PassengersInElevator.Count > 0)
            {
                foreach(Passenger passenger in TempElevatorArray)
                {
                    if (passenger.Destination == ElevatorPosition) //Denna if-sats kontrollerar att passagerarna är på rätt plats. 
                    {
                        PassengersDoneTravelling.Add(passenger); //Här läggs passagerarna till i listan av passagerare som har åkt klart. 
                        PassengersInElevator.Remove(passenger); //Här tas passageraren bort från hissen. 
                    }
                }
            }
            if(ElevatorIsFull(PassengersInElevator))
            {
                return;
            }
            Passenger[] TempFloorArray = new Passenger[Hotel.Floors[ElevatorPosition].PassengersOnFloor.Count()]; 
            Hotel.Floors[ElevatorPosition].PassengersOnFloor.CopyTo(TempFloorArray);
            foreach (Passenger passenger in TempFloorArray)
            {
                if(passenger.DirectionOfPassengers() == DirectionOfElevator)
                {
                    PassengersInElevator.Add(passenger);
                    TotalAmountOfPassengers++;
                    Hotel.Floors[ElevatorPosition].Remove(passenger);
                    if (ElevatorIsFull(PassengersInElevator))
                    {
                        return;
                    }
                }
            }
        }
        /// <summary>
        ///  Denna metod kollar om hissen är full.
        /// </summary>
        /// <param name="PassengersInElevator"> Tar emot listan av passagerare. </param>
        /// <returns> Metoden returnerar en bool, true om den är full eller false om den inte är det. </returns>
        public bool ElevatorIsFull(List<Passenger> PassengersInElevator)
        { 
            if (PassengersInElevator.Count == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Denna metod kollar om hissen är tom, användbart för att veta om simulationen är färdig.
        /// </summary>
        /// <param name="PassengersInElevator"> Tar emot listan av passagerare. </param>
        /// <returns> Metoden returnerar en bool, true om den är tom eller false om den inte är det. </returns>
        public bool ElevatorIsEmpty(List<Passenger> PassengersInElevator) 
        {
            if (PassengersInElevator.Count == 0)
            {  
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  Denna metod används för att skriva ut alla passagerare som är i hissen. 
        /// </summary>
        public void VisualizePassengers()
        {
            Console.WriteLine();
            Console.Write("Passengers in elevator: ");
            foreach (Passenger passenger in PassengersInElevator)
            {
                Console.Write(passenger.Destination + " ");
            }
        }
        /// <summary>
        /// Denna metod skriver ner statistiken för alla passagerare i en txt-fil.
        /// </summary>
        public void ResultToTxt()
        {
            List<string> lines = new List<string>();
            
            foreach (Passenger passenger in PassengersDoneTravelling)
            {
                lines.Add($"Start floor: {passenger.CurrentFloor} Destination: {passenger.Destination} Time waiting: {passenger.TimeWaiting} Time total: {passenger.TimeTotal}");
                lines.Add("-----------------");
            }
            string file = @"C:\Users\Samuel\source\repos\Result.txt"; // Här får man specificera vilken mapp och fil man vill få resultatet i
            File.WriteAllLines(file, lines);

        }
    }
}
