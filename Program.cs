using System;
using System.Collections.Generic;
using System.IO;
 
namespace AirportManager
{
    class Program
    {
        private const string FilePath = "/Users/ksusha/Documents/studies/3 курс/архитектура ИС/Airport/Airport/flights.csv";
 
        static void Main(string[] args)
        {
            if (!File.Exists(FilePath))
            {
                using (var file = File.Create(FilePath)) { }
            }
 
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Показать все рейсы");
                Console.WriteLine("2. Показать рейс по номеру");
                Console.WriteLine("3. Добавить рейс");
                Console.WriteLine("4. Удалить рейс");
                Console.WriteLine("Esc. Выйти");
 
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        DisplayAllFlights();
                        break;
                    case ConsoleKey.D2:
                        DisplayFlightByNumber();
                        break;
                    case ConsoleKey.D3:
                        AddFlight();
                        break;
                    case ConsoleKey.D4:
                        DeleteFlight();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
 
        static void DisplayAllFlights()
        {
            var flights = ReadFlights();
            foreach (var flight in flights)
            {
                Console.WriteLine(flight);
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
 
        static void DisplayFlightByNumber()
        {
            var flights = ReadFlights();
            Console.Write("Введите номер рейса: ");
            int index;
            if (int.TryParse(Console.ReadLine(), out index) && index >= 1 && index <= flights.Count)
            {
                Console.WriteLine(flights[index - 1]);
            }
            else
            {
                Console.WriteLine("Неверный номер рейса.");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
 

        static void AddFlight()
        {
            Console.Clear(); 
            Console.Write("Введите номер рейса: ");
            string flightNumber = Console.ReadLine();
 
            Console.Write("Введите пункт вылета: ");
            string departureLocation = Console.ReadLine();
            if (ContainsNumbers(departureLocation))
            {
                ShowError("Неверный формат пункта вылета. Он не должен содержать числа.");
                return;
            }
 
            Console.Write("Введите пункт назначения: ");
            string destination = Console.ReadLine();
            if (ContainsNumbers(destination))
            {
                ShowError("Неверный формат пункта назначения. Он не должен содержать числа.");
                return;
            }
 
            Console.Write("Введите время отправления (HHMM): ");
            int departureTime;
            if (!int.TryParse(Console.ReadLine(), out departureTime) || !IsValidTime(departureTime))
            {
                ShowError("Неверный формат времени отправления. Убедитесь, что часы от 00 до 23, а минуты от 00 до 59.");
                return;
            }
 
            Console.Write("Введите время прибытия (HHMM): ");
            int arrivalTime;
            if (!int.TryParse(Console.ReadLine(), out arrivalTime) || !IsValidTime(arrivalTime))
            {
                ShowError("Неверный формат времени прибытия. Убедитесь, что часы от 00 до 23, а минуты от 00 до 59.");
                return;
            }
 
            Console.Write("Активен ли рейс (true/false): ");
            bool isActive;
            if (!bool.TryParse(Console.ReadLine(), out isActive))
            {
                ShowError("Неверный формат для статуса рейса.");
                return;
            }

            var flight = new FlightRecord(flightNumber, departureLocation, destination, departureTime, arrivalTime, isActive);
 
            string fileContent = File.ReadAllText(FilePath);
            if (!fileContent.EndsWith(Environment.NewLine))
            {
                File.AppendAllText(FilePath, Environment.NewLine); 
            }
 
            File.AppendAllText(FilePath, flight.ToString() + Environment.NewLine);
 
            Console.WriteLine("Рейс добавлен.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
 
        static void ShowError(string message)
        {
            Console.Clear(); 
            Console.WriteLine(message);
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(); 
        }
 
        static bool IsValidTime(int time)
        {
            int hours = time / 100;
            int minutes = time % 100;
            return hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59;
        }
 
        static bool ContainsNumbers(string text)
        {
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
 
 
        static void DeleteFlight()
        {
            var flights = ReadFlights();
            Console.Write("Введите номер рейса для удаления: ");
            int index;
            if (int.TryParse(Console.ReadLine(), out index) && index >= 1 && index <= flights.Count)
            {
                flights.RemoveAt(index - 1);
                File.WriteAllLines(FilePath, flights.ConvertAll(flight => flight.ToString()));
                Console.WriteLine("Рейс удален.");
            }
            else
            {
                Console.WriteLine("Неверный номер рейса.");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
 

        static List<FlightRecord> ReadFlights()
        {
            var flights = new List<FlightRecord>();
            foreach (var line in File.ReadAllLines(FilePath))
            {
                if (!string.IsNullOrWhiteSpace(line))                {
                    flights.Add(FlightRecord.FromCsv(line));
                }
            }
            return flights;
        }
    }
}
