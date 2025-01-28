//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
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
                Airline airline = new Airline(parts[0], parts[1]);
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
    string[] lines = File.ReadAllLines(filename);
    for (int i = 1; i < lines.Length; i++)
        {
        string[] parts = lines[i].Split(',');
        if (parts.Length >= 4)
            {
            DateTime expectedTime = DateTime.ParseExact(
                parts[3] + " " + DateTime.Now.Year,
                "h:mm tt yyyy", // No changes here as the format works well with your data
                null
            );

            Flight flight;
            string flightType = parts.Length >= 5 ? parts[4].ToUpper() : "";

            if (flightType == "DDJB")
                {
                flight = new DDJBFlight(
                    parts[0],
                    parts[1],
                    parts[2],
                    expectedTime,
                    300
                );
                }
            else if (flightType == "CFFT")
                {
                flight = new CFFTFlight(
                    parts[0],
                    parts[1],
                    parts[2],
                    expectedTime,
                    150 // Use the appropriate request fee for CFFT
                );
                }
            else if (flightType == "LWTT")
                {
                flight = new LWTTFlight(
                    parts[0],
                    parts[1],
                    parts[2],
                    expectedTime,
                    500 // Use the appropriate request fee for LWTT
                );
                }
            else
                {
                flight = new NORMFlight(parts[0], parts[1], parts[2], expectedTime);
                }

            Flights.Add(flight);
            }
        }
    Console.WriteLine($"{Flights.Count} Flights Loaded!");
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
void DisplayAirlineFlights(Dictionary<string, BoardingGate> BoardingGates)
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
    string airlineCode = Console.ReadLine().ToUpper();

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
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected");

    List<Flight> airlineFlights = new List<Flight>();
    foreach (Flight flight in Flights)
        {
        if (flight.FlightNumber.StartsWith(selectedAirline.Code))
            {
            airlineFlights.Add(flight);
            BoardingGate gate = null;
            if (BoardingGates.ContainsKey(flight.FlightNumber))
                {
                gate = BoardingGates[flight.FlightNumber];
                }

            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}",
                flight.FlightNumber,
                selectedAirline.Name,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
            }
        }

    if (airlineFlights.Count == 0)
        {
        Console.WriteLine("No flights available for the selected airline.");
        return;
        }

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();

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

    // Displaying Special Request Code and Boarding Gate for selected flight
    if (selectedFlight is LWTTFlight lwttSelected)
        {
        Console.WriteLine("Special Request Code: LWTT");
        }
    else if (selectedFlight is DDJBFlight ddjbSelected)
        {
        Console.WriteLine("Special Request Code: DDJB");
        }
    else if (selectedFlight is CFFTFlight cfftSelected)
        {
        Console.WriteLine("Special Request Code: CFFT");
        }
    else
        {
        Console.WriteLine("NORM Flight");
        }

    // Displaying available boarding gates based on the special request codes
    Console.WriteLine("\nAvailable Boarding Gates:");
    foreach (var gateEntry in BoardingGates)
        {
        BoardingGate gate = gateEntry.Value;
        if ((gate.SupportsDDJB && selectedFlight is DDJBFlight) ||
            (gate.SupportsCFFT && selectedFlight is CFFTFlight) ||
            (gate.SupportsLWTT && selectedFlight is LWTTFlight))
            {
            Console.WriteLine($"{gate.GateName}");
            }
        }
    }

//Basic Feature 8

void ModifyFlightDetails()
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
    string airlineCode = Console.ReadLine().ToUpper();

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
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected");

    List<Flight> airlineFlights = new List<Flight>();
    foreach (Flight flight in Flights)
        {
        if (flight.FlightNumber.StartsWith(selectedAirline.Code))
            {
            airlineFlights.Add(flight);
            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}",
                flight.FlightNumber,
                selectedAirline.Name,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
            }
        }

    if (airlineFlights.Count == 0)
        {
        Console.WriteLine("No flights available for the selected airline.");
        return;
        }

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();

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

    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option: ");
    string option = Console.ReadLine();

    if (option == "1")
        {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.Write("Choose an option: ");
        string modifyOption = Console.ReadLine();

        if (modifyOption == "1")
            {
            Console.Write("Enter new Origin: ");
            selectedFlight.Origin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            selectedFlight.Destination = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            selectedFlight.ExpectedTime = DateTime.ParseExact(Console.ReadLine(), "dd/M/yyyy H:mm", null);
            Console.WriteLine("Flight updated!");
            }
        else if (modifyOption == "2")
            {
            Console.Write("Enter new Status: ");
            selectedFlight.Status = Console.ReadLine();
            Console.WriteLine("Flight status updated!");
            }
        else if (modifyOption == "3")
            {
            Console.WriteLine("Enter new Special Request Code (DDJB, CFFT, LWTT, NORM): ");
            string newCode = Console.ReadLine().ToUpper();
            if (newCode == "DDJB")
                {
                selectedFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, 300);
                }
            else if (newCode == "CFFT")
                {
                selectedFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, 150);
                }
            else if (newCode == "LWTT")
                {
                selectedFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, 500);
                }
            else
                {
                selectedFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
                }
            Console.WriteLine("Special Request Code updated!");
            }
        else if (modifyOption == "4")
            {
            Console.WriteLine("Available Boarding Gates:");
            foreach (var gateEntry in BoardingGates)
                {
                BoardingGate gate = gateEntry.Value;
                if ((gate.SupportsDDJB && selectedFlight is DDJBFlight) ||
                    (gate.SupportsCFFT && selectedFlight is CFFTFlight) ||
                    (gate.SupportsLWTT && selectedFlight is LWTTFlight))
                    {
                    Console.WriteLine($"{gate.GateName}");
                    }
                }
            Console.Write("Enter new Boarding Gate: ");
            string newGate = Console.ReadLine();
            if (BoardingGates.ContainsKey(newGate))
                {
                BoardingGates[newGate].Flight = selectedFlight;
                Console.WriteLine("Boarding Gate updated!");
                }
            else
                {
                Console.WriteLine("Invalid Boarding Gate. Please try again.");
                }
            }
        }
    else if (option == "2")
        {
        Flights.Remove(selectedFlight);
        Console.WriteLine("Flight deleted!");
        }
    else
        {
        Console.WriteLine("Invalid option. Please try again.");
        }

    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Details");
    Console.WriteLine("=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    Console.WriteLine($"Status: {selectedFlight.Status}");

    if (selectedFlight is LWTTFlight)
        {
        Console.WriteLine("Special Request Code: LWTT");
        }
    else if (selectedFlight is DDJBFlight)
        {
        Console.WriteLine("Special Request Code: DDJB");
        }
    else if (selectedFlight is CFFTFlight)
        {
        Console.WriteLine("Special Request Code: CFFT");
        }
    else
        {
        Console.WriteLine("Special Request Code: NORM");
        }

    Console.WriteLine("Boarding Gate: Unassigned");
    }




void MainMenu()
    {
    Console.WriteLine("\n\n\n=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("0. Exit");
    Console.WriteLine("\nPlease select your option: ");
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
            DisplayAirlineFlights(BoardingGates);
            }
        else if (choice == "6")
            {
            ModifyFlightDetails();
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
