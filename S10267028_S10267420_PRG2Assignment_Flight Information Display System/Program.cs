using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
    {
    static Dictionary<string, Airline> Airlines = new Dictionary<string, Airline>();
    static Dictionary<string, BoardingGate> BoardingGates = new Dictionary<string, BoardingGate>();
    static List<Flight> Flights = new List<Flight>();

    static void Main(string[] args)
        {
        Console.WriteLine("Loading Airlines...");
        LoadAirlines("airlines.csv");
        Console.WriteLine($"{Airlines.Count} Airlines Loaded!");

        Console.WriteLine("Loading Boarding Gates...");
        LoadBoardingGates("boardinggates.csv");
        Console.WriteLine($"{BoardingGates.Count} Boarding Gates Loaded!");

        Console.WriteLine("Loading Flights...");
        LoadFlights("flights.csv");
        Console.WriteLine($"{Flights.Count} Flights Loaded!");

        while (true)
            {
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
                {
                case "1":
                    ListAllFlights();
                    break;
                case "2":
                    ListBoardingGates();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
                }
            }
        }

    static void DisplayMenu()
        {
        Console.WriteLine("\n\n\n=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("0. Exit");
        Console.Write("\nPlease select your option: ");
        }

    static void LoadAirlines(string filename)
        {
        try
            {
            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines.Skip(1)) // Skip header
                {
                string[] parts = line.Split(',');
                if (parts.Length >= 2)
                    {
                    var airline = new Airline
                        {
                        Name = parts[0],
                        Code = parts[1]
                        };
                    Airlines[airline.Code] = airline;
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

    static void LoadBoardingGates(string filename)
        {
        try
            {
            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines.Skip(1)) // Skip header
                {
                string[] parts = line.Split(',');
                if (parts.Length >= 5)
                    {
                    var gate = new BoardingGate
                        {
                        GateName = parts[0],
                        SupportsCFFT = bool.Parse(parts[1]),
                        SupportsDDJB = bool.Parse(parts[2]),
                        SupportsLWTT = DateTime.Parse(parts[3])
                        };
                    BoardingGates[gate.GateName] = gate;
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

    static void LoadFlights(string filename)
        {
        try
            {
            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines.Skip(1)) // Skip header
                {
                string[] parts = line.Split(',');
                if (parts.Length >= 5)
                    {
                    var flight = new Flight
                        {
                        FlightNumber = parts[0],
                        Origin = parts[1],
                        Destination = parts[2],
                        ExpectedTime = DateTime.Parse(parts[3]),
                        Status = parts[4]
                        };

                    // Find and set the airline
                    string airlineCode = parts[0].Split()[0];
                    if (Airlines.TryGetValue(airlineCode, out Airline airline))
                        {
                        airline.AddFlight(flight);
                        }

                    Flights.Add(flight);
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading flights: {ex.Message}");
            }
        }

    static void ListAllFlights()
        {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
            "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (var flight in Flights.OrderBy(f => f.ExpectedTime))
            {
            string airlineName = Airlines.Values
                .FirstOrDefault(a => flight.FlightNumber.StartsWith(a.Code))?.Name ?? "Unknown";

            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
                flight.FlightNumber,
                airlineName,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
            }
        }

    static void ListBoardingGates()
        {
        // Implement boarding gate listing logic as needed
        }
    }