using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Elevator
{
    // Grupp D4 - Samuel Bucht Stjernman, Ludvig Björnström, Nils Lundqvist, Nils Sörby, Ivar Lyttkens, Humam Majed Alasfar, Erik Oldner
    class Program
    {
        static void Main(string[] args)
        {
            int AmountOfFloors = 10; //Här kan man ändra variabel för att ändra antalet floors.
            Building Hotel = new Building(AmountOfFloors);
            Elevator elevator = new Elevator();
            int start = AmountOfFloors;
            int end = AmountOfFloors + AmountOfFloors;

            do  
            {
                Console.WriteLine($"Instructions: \n press Y to start the simulation with visualization \n Press T to run the simulation step-by-step \n Press K to go to end of simulation");

                var input = Console.ReadKey();
                switch (input.Key) // switchsats som bestämmer hur simulationen ska köras och visualiseras.
                {
                    case ConsoleKey.Y: //Om Y var den knappen som blev nedtryckt körs detta case. 
                        while (!Hotel.IsDone(Hotel.Floors) || !elevator.ElevatorIsEmpty(elevator.PassengersInElevator))
                        {
                            if (Console.KeyAvailable == true)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Press enter to resume the simulation");
                                Console.ReadLine();
                            }
                            Console.Clear();
                            Hotel.Visualize(Hotel.Floors, elevator);
                            elevator.VisualizePassengers();
                            Console.WriteLine("\n\nPress P to pause the simulation");
                            Thread.Sleep(50);  //Sleep används för att man ska hinna se vad som sker i simulationen. Ändra denna variabel ifall du vill göra simulationen snabbar eller långsammare.
                            elevator.PassengerHandling(Hotel); //Denna metod kallas två gånger för att man även ska plocka upp passagerare när hissen är påväg ner.
                            elevator.ChangeDirectionIfEmptyAhead(Hotel);
                            elevator.PassengerHandling(Hotel);
                            elevator.Move(elevator.DirectionOfElevator, AmountOfFloors);
                            Hotel.SetFloors(ReadTxt.RefillFloors(start, end, Hotel.Floors, AmountOfFloors));//Här kallas metoden RefillFloors och fyller på raderna med passagerare, med hjälp av start och end.
                            start += AmountOfFloors; //Start används för att ReadTxt ska veta vilken rad av inputet den ska börja på. Ökar med 10 efter varje lopp.
                            end += AmountOfFloors;   //End används för att ReadTxt ska veta vilken rad av inputen den ska sluta på. Ökar med 10 efter varje loop. Så alltså läser start och end tillsammans totalt 10 rader, dvs 1 time unit.
                            Timer(Hotel, elevator);
                        }
                        ResultElevator(Hotel, elevator);
                        ResultPassenger(elevator);
                        elevator.ResultToTxt();
                        Console.ReadKey();
                        break;

                    case ConsoleKey.T: //Om T var den knappen som blev nedtryckt körs detta case. Skillnaden i detta case från det föregående är att man kör simulationen steg för steg.
                        Console.Clear();
                        Console.WriteLine("Press any button to step");
                        while (!Hotel.IsDone(Hotel.Floors) || !elevator.ElevatorIsEmpty(elevator.PassengersInElevator))
                        {
                            Console.ReadKey(); //Denna läser input och används för att pausa. 
                            Console.Clear();
                            Hotel.Visualize(Hotel.Floors, elevator);
                            elevator.VisualizePassengers();
                            elevator.PassengerHandling(Hotel);
                            elevator.ChangeDirectionIfEmptyAhead(Hotel);
                            elevator.PassengerHandling(Hotel);
                            elevator.Move(elevator.DirectionOfElevator, AmountOfFloors);
                            Hotel.SetFloors(ReadTxt.RefillFloors(start, end, Hotel.Floors, AmountOfFloors));
                            start += AmountOfFloors;
                            end += AmountOfFloors;
                            Timer(Hotel, elevator);
                            Console.WriteLine($"\n\nPress any button to continue stepping");
                        }
                        ResultElevator(Hotel, elevator);
                        ResultPassenger(elevator);
                        elevator.ResultToTxt();
                        Console.ReadKey();
                        break;

                    case ConsoleKey.K: //Om K var den knappen som blev nedtryckt körs detta case. Ingen visualisering
                        while (!Hotel.IsDone(Hotel.Floors) || !elevator.ElevatorIsEmpty(elevator.PassengersInElevator))
                        {
                            elevator.PassengerHandling(Hotel);
                            elevator.ChangeDirectionIfEmptyAhead(Hotel);
                            elevator.PassengerHandling(Hotel);
                            elevator.Move(elevator.DirectionOfElevator, AmountOfFloors);
                            Hotel.SetFloors(ReadTxt.RefillFloors(start, end, Hotel.Floors, AmountOfFloors));
                            start += AmountOfFloors;
                            end += AmountOfFloors;
                            Timer(Hotel, elevator);
                        }
                        ResultElevator(Hotel, elevator);
                        ResultPassenger(elevator);
                        elevator.ResultToTxt();
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
            } while (!Hotel.IsDone(Hotel.Floors) || !elevator.ElevatorIsEmpty(elevator.PassengersInElevator));
            
        }
        /// <summary>
        ///  Denna metod används för att öka passagerarnas timeWaiting och TimeTotal med 10ms för varje "Time unit" som körs.
        /// </summary>
        /// <param name="Hotel"> Instansen av hotel tas in. </param>
        /// <param name="elevator"> Instansen av elevator tas in. </param>
        static void Timer(Building Hotel, Elevator elevator) 
        {
            foreach (Floor floor in Hotel.Floors)
            {
                foreach (Passenger passenger in floor.PassengersOnFloor)
                {
                    passenger.IncreaseTimeWaiting();
                    passenger.IncreaseTimeTotal();
                }
            }
            foreach (Passenger passenger in elevator.PassengersInElevator)
            {
                passenger.IncreaseTimeTotal();
            }
        }
        /// <summary>
        ///  Skriver ut resultatet för elevatorn, dvs hur många passagerare som åkte och hur lång tid simulationen tog.
        /// </summary>
        /// <param name="Hotel"> Instansen av hotel tas in. </param>
        /// <param name="elevator"> Instansen av elevator tas in. </param>
        static void ResultElevator(Building Hotel, Elevator elevator)
        {
            Console.Clear();
            Hotel.Visualize(Hotel.Floors, elevator);
            Console.WriteLine();
            Console.WriteLine($"Number of passengers: {elevator.TotalAmountOfPassengers}");
            Console.WriteLine();
            Console.WriteLine($"Total time taken: {elevator.TimeTraveled * 10} ms");
            Console.WriteLine();
        }
        /// <summary>
        /// Skriver ut resultatet för passagerarna, dvs hur lång tid average waiting time och average completion time var. Metoden skriver även ut least total time taken och highest total time taken. 
        /// </summary>
        /// <param name="elevator"> Instansen av elevator tas in. </param>
        static void ResultPassenger(Elevator elevator)
        {
            int AverageWaitingTime = 0;
            int AverageCompletionTime = 0;
            foreach(Passenger passenger in elevator.PassengersDoneTravelling)
            {
                AverageWaitingTime += passenger.TimeWaiting;
                AverageCompletionTime += passenger.TimeTotal;
            }
            AverageWaitingTime = AverageWaitingTime / elevator.PassengersDoneTravelling.Count();
            AverageCompletionTime = AverageCompletionTime / elevator.PassengersDoneTravelling.Count();
            Console.WriteLine("Passenger stats");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Average waiting time: {AverageWaitingTime} ms \nAverage completion time: {AverageCompletionTime} ms");
            int min = elevator.PassengersDoneTravelling[0].TimeTotal;
            int max = elevator.PassengersDoneTravelling[0].TimeTotal;
            foreach(Passenger passenger in elevator.PassengersDoneTravelling)
            {
                if(passenger.TimeTotal < min)
                {
                    min = passenger.TimeTotal;
                }
                if(passenger.TimeTotal > max)
                {
                    max = passenger.TimeTotal;
                }
            }
            Console.WriteLine($"Highest total time taken: {max} ms \nLeast total time taken: {min} ms");
        }
    }
}
