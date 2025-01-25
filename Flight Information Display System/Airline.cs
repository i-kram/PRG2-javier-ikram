using System;
using System.Collections.Generic;

namespace FlightInformationDisplaySystem
{
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public bool AddFlight(Flight flight)
        {
            if (flight == null) return false;
            if (Flights.ContainsKey(flight.FlightNumber))
                return false;

            Flights[flight.FlightNumber] = flight;
            return true;
        }


        public bool RemoveFlight(Flight flight)
        {
            if (flight == null) return false;
            return Flights.Remove(flight.FlightNumber);
        }


        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }

        public override string ToString()
        {
            return $"Airline: {Name}, Code: {Code}, Flights: {Flights.Count}";
        }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
}
