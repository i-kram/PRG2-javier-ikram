//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================





using System;
using System.Collections.Generic;
using System.IO;
using FlightInformationDisplaySystem;

List<Airline> Airlines = new List<Airline>();
Dictionary<string, BoardingGate> BoardingGates = new Dictionary<string, BoardingGate>();
List<Flight> Flights = new List<Flight>();

//Basic Feature 1
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

//Basic Feature 1
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
                // Assuming you might want to set Flight to null if not available
                BoardingGate gate = new BoardingGate(
                    parts[0],
                    ConvertToBoolean(parts[1]),
                    ConvertToBoolean(parts[2]),
                    ConvertToBoolean(parts[3]),
                    null // Or a valid Flight object if needed
                );
                BoardingGates[gate.GateName] = gate;
                }
            }
        Console.WriteLine($"{BoardingGates.Count} Boarding Gates Loaded!");
        }
    catch (Exception ex)
        {
        Console.WriteLine($"Error loading boarding gates: {ex.Message}");
        }
    }



//Basic Feature 2
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

                Flight flight = new Flight(
                    parts[0], // FlightNumber
                    parts[1], // Origin
                    parts[2], // Destination
                    expectedTime // ExpectedTime
                );

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


//Basic Feature 3
void ListAllFlights()
    {
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/\nArrival Time");

    foreach (Flight flight in Flights)
        {
        string airlineName = "Unknown";
        foreach (Airline airline in Airlines)
            {
            if (flight.FlightNumber.StartsWith(airline.Code))
                {
                airlineName = airline.Name;
                break;
                }
            }

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


//Basic Feature 4
void ListBoardingGates()
    {
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}",
        "Gate Name", "DDJB", "CFFT", "LWTT");

    foreach (KeyValuePair<string, BoardingGate> gateEntry in BoardingGates)
    {
        BoardingGate gate = gateEntry.Value;
        Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}",
            gate.GateName,
            gate.SupportsDDJB ? "True" : "False",
            gate.SupportsCFFT ? "True" : "False",
            gate.SupportsLWTT ? "True" : "False");
    }
}


//Basic Feature 7
void DisplayAirlineFlights()
    {
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}", "Airline Code", "Airline Name");

    foreach (Airline airline in Airlines)
        {
        Console.WriteLine("{0,-15}{1,-25}", airline.Code, airline.Name);
        }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine()?.ToUpper();

    Airline selectedAirline = null;
    foreach (Airline airline in Airlines)
        {
        if (airline.Code == airlineCode)
            {
            selectedAirline = airline;
            break;
            }
        }

    if (selectedAirline == null)
        {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
        }

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}",
        "Flight Number", "Airline Name", "Origin", "Destination");

    List<Flight> airlineFlights = new List<Flight>();
    foreach (Flight flight in Flights)
        {
        if (flight.FlightNumber.StartsWith(selectedAirline.Code))
            {
            airlineFlights.Add(flight);
            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}",
                flight.FlightNumber,
                selectedAirline.Name,
                flight.Origin,
                flight.Destination);
            }
        }

    if (airlineFlights.Count == 0)
        {
        Console.WriteLine("No flights available for the selected airline.");
        return;
        }

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();

    Flight selectedFlight = null;
    foreach (Flight flight in airlineFlights)
        {
        if (flight.FlightNumber == flightNumber)
            {
            selectedFlight = flight;
            break;
            }
        }

    if (selectedFlight == null)
        {
        Console.WriteLine("Invalid Flight Number. Please try again.");
        return;
        }

    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Details");
    Console.WriteLine("=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    //Console.WriteLine($"Special Request Code: {selectedFlight.SpecialRequestCode ?? "None"}");
    //Console.WriteLine($"Boarding Gate: {selectedFlight.BoardingGate ?? "Not Assigned"}");
    }


void MainMenu()
    {
    Console.WriteLine("\n\n\n=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("5. Display Airline Flights");
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
        else if (choice == "5")
            {
            DisplayAirlineFlights();
            }
        else if (choice == "0")
            {
            Console.WriteLine("Goodbye!");
            break;
            }
        else
            {
            Console.WriteLine("Invalid option. Please try again.");
            }
        }
    }

// Entry point
void Main()
    {
    Run();
    }

Main();