//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================
using System;
using System.Collections.Generic;

namespace FlightInformationDisplaySystem
    {
    public class Airline
        {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Flight> Flights { get; set; } = new List<Flight>();

        public Airline(string name, string code)
            {
            Name = name;
            Code = code;
            }

        // Adds a flight to the airline
        public void AddFlight(Flight flight)
            {
            Flights.Add(flight);
            }

        // Removes a flight from the airline
        public bool RemoveFlight(Flight flight)
            {
            return Flights.Remove(flight);
            }

        // Calculates the total fees for all flights under the airline
        public double CalculateFees()
            {
            double totalFees = 0;
            foreach (var flight in Flights)
                {
                totalFees += flight.CalculateFees();
                }
            return totalFees;
            }

        public override string ToString()
            {
            return $"Airline {Name} ({Code}), Flights: {Flights.Count}";
            }
        }
    }