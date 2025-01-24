using System;
using System.Collections.Generic;
using System.IO;

class Program
    {
    List<Airline> Airlines = new List<Airline>();
    List<BoardingGate> BoardingGates = new List<BoardingGate>();
    List<Flight> Flights = new List<Flight>();

    void LoadAirlines(string filename)
        {
        Console.WriteLine("Loading Airlines...");
        try
            {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 1; i < lines.Length; i++)
                {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 2)
                    {
                    Airline airline = new Airline
                        {
                        Name = parts[0],
                        Code = parts[1]
                        };
                    Airlines.Add(airline);
                    }
                }
            Console.WriteLine($"{Airlines.Count} Airlines Loaded!");
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading airlines: {ex.Message}");
            }
        }

    void LoadBoardingGates(string filename)
        {
        Console.WriteLine("Loading Boarding Gates...");
        try
            {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 1; i < lines.Length; i++)
                {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 4)
                    {
                    BoardingGate gate = new BoardingGate
                        {
                        GateName = parts[0],
                        SupportsDDJB = ConvertToBoolean(parts[1]),
                        SupportsCFFT = ConvertToBoolean(parts[2]),
                        SupportsLWTT = ConvertToBoolean(parts[3])
                        };
                    BoardingGates.Add(gate);
                    }
                }
            Console.WriteLine($"{BoardingGates.Count} Boarding Gates Loaded!");
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading boarding gates: {ex.Message}");
            }
        }

    void LoadFlights(string filename)
        {
        Console.WriteLine("Loading Flights...");
        try
            {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 1; i < lines.Length; i++)
                {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 4)
                    {
                    DateTime expectedTime = DateTime.ParseExact(
                        parts[3] + " " + DateTime.Now.Year,
                        "h:mm tt yyyy",
                        null
                    );

                    Flight flight = new Flight
                        {
                        FlightNumber = parts[0],
                        Origin = parts[1],
                        Destination = parts[2],
                        ExpectedTime = expectedTime,
                        Status = parts.Length > 4 ? parts[4] : ""
                        };

                    Flights.Add(flight);
                    }
                }
            Console.WriteLine($"{Flights.Count} Flights Loaded!");
            }
        catch (Exception ex)
            {
            Console.WriteLine($"Error loading flights: {ex.Message}");
            }
        }


    void ListAllFlights()
        {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
            "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (Flight flight in Flights)
            {
            string airlineName = Airlines.Find(a => flight.FlightNumber.StartsWith(a.Code))?.Name ?? "Unknown";

            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
                flight.FlightNumber,
                airlineName,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
            }
        }

    bool ConvertToBoolean(string value)
        {
        if (value.ToLower() == "true")
            return true;
        if (value.ToLower() == "false")
            return false;
        throw new FormatException($"Invalid boolean value: {value}");
        }

    void ListBoardingGates()
        {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}",
            "Gate Name", "DDJB", "CFFT", "LWTT");

        foreach (BoardingGate gate in BoardingGates)
            {
            Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}",
                gate.GateName,
                gate.SupportsDDJB ? "True" : "False",
                gate.SupportsCFFT ? "True" : "False",
                gate.SupportsLWTT ? "True" : "False");
            }
        }

    void MainMenu()
        {
        Console.WriteLine("\n\n\n=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("0. Exit");
        Console.Write("\nPlease select your option: ");
        }

    void Run()
        {
        LoadAirlines("airlines.csv");
        LoadBoardingGates("boardinggates.csv");
        LoadFlights("flights.csv");

        while (true)
            {
            MainMenu();
            string choice = Console.ReadLine();

            if (choice == "1")
                {
                ListAllFlights();
                }
            else if (choice == "2")
                {
                ListBoardingGates();
                }
            else if (choice == "0")
                {
                break;
                }
            else
                {
                Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

    static void Main()
        {
        Program program = new Program();
        program.Run();
        }
    }