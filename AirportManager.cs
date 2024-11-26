using System;
 
namespace AirportManager
{
    internal class FlightRecord
    {
        public string FlightNumber { get; set; } 
        public string DepartureLocation { get; set; }  
        public string Destination { get; set; } 
        public int DepartureTime { get; set; } 
        public int ArrivalTime { get; set; }   
        public bool IsActive { get; set; }     
 
        public FlightRecord(string flightNumber, string departureLocation, string destination, int departureTime, int arrivalTime, bool isActive)
        {
            FlightNumber = flightNumber;
            DepartureLocation = departureLocation;
            Destination = destination;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            IsActive = isActive;
        }
 
        public static FlightRecord FromCsv(string csvLine)
        {
            var values = csvLine.Split(',');
            return new FlightRecord(
                values[0],
                values[1],
                values[2],
                int.Parse(values[3]),
                int.Parse(values[4]),
                bool.Parse(values[5]));
        }
 
        public override string ToString()
        {
            return $"{FlightNumber},{DepartureLocation},{Destination},{DepartureTime},{ArrivalTime},{IsActive}";
        }
    }
}


