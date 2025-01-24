using S10267028_S10267420_PRG2Assignment_Flight_Information_Display_System;
using System;
using System.Collections.Generic;

public class Airline
    {
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline()
        {
        Flights = new Dictionary<string, Flight>();
        }

    public bool AddFlight(Flight flight)
        {
        if (flight == null || string.IsNullOrEmpty(flight.FlightNumber))
            return false;

        Flights[flight.FlightNumber] = flight;
        return true;
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

    public bool RemoveFlight(Flight flight)
        {
        if (flight == null || string.IsNullOrEmpty(flight.FlightNumber))
            return false;

        return Flights.Remove(flight.FlightNumber);
        }

    public override string ToString()
        {
        return $"Airline: {Name}, Code: {Code}, Flights: {Flights.Count}";
        }
    }