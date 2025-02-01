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

Dictionary<string, Airline> Airlines = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> BoardingGates = new Dictionary<string, BoardingGate>();
Dictionary<string, Flight> Flights = new Dictionary<string, Flight>();

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
                Airlines[airline.Code] = airline;
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



// Load Flights from CSV
void LoadFlights(string filename)
{
    Console.WriteLine("Loading Flights...");
    try
    {
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines[1..])
        {
            string[] parts = line.Split(',');
            if (parts.Length >= 4)
            {
                DateTime expectedTime = DateTime.ParseExact(parts[3], "h:mm tt", null);
                string specialRequest = parts.Length >= 5 ? parts[4] : "";
                Flight flight = specialRequest switch
                {
                    "DDJB" => new DDJBFlight(parts[0], parts[1], parts[2], expectedTime, 300),
                    "CFFT" => new CFFTFlight(parts[0], parts[1], parts[2], expectedTime, 150),
                    "LWTT" => new LWTTFlight(parts[0], parts[1], parts[2], expectedTime, 500),
                    _ => new NORMFlight(parts[0], parts[1], parts[2], expectedTime)
                };
                Flights[flight.FlightNumber] = flight;
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
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected\nDeparture/Arrival Time");

    foreach (Flight flight in Flights.Values)
    {
        string airlineName = "Unknown";
        if (Airlines.ContainsKey(flight.FlightNumber.Substring(0, 2)))
        {
            airlineName = Airlines[flight.FlightNumber.Substring(0, 2)].Name;
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

//Basic Feature 5
void AssignBoardingGate()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.WriteLine("Enter Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();
    if (!Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Invalid Flight Number. Please try again.");
        return;
    }
    Flight selectedFlight = Flights[flightNumber];

    string specialRequest = "None";
    if (selectedFlight is DDJBFlight) specialRequest = "DDJB";
    else if (selectedFlight is CFFTFlight) specialRequest = "CFFT";
    else if (selectedFlight is LWTTFlight) specialRequest = "LWTT";

    while (true)
    {
        Console.WriteLine("Enter Boarding Gate Name: ");
        string gateName = Console.ReadLine().ToUpper();
        if (!BoardingGates.ContainsKey(gateName))
        {
            Console.WriteLine("Invalid Boarding Gate. Please try again.");
            continue;
        }
        BoardingGate selectedGate = BoardingGates[gateName];
        if (selectedGate.Flight != null)
        {
            Console.WriteLine("The selected Boarding Gate is already assigned to another flight. Please choose a different gate.");
            continue;
        }
        selectedGate.Flight = selectedFlight;

        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:d/M/yyyy h:mm:ss tt}");
        Console.WriteLine($"Special Request Code: {specialRequest}");
        Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
        Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

        Console.Write("Would you like to update the status of the flight? (Y/N): ");
        string updateStatus = Console.ReadLine().ToUpper();
        if (updateStatus == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight: ");
            string statusChoice = Console.ReadLine();
            if (statusChoice == "1") selectedFlight.Status = "Delayed";
            else if (statusChoice == "2") selectedFlight.Status = "Boarding";
            else selectedFlight.Status = "On Time";
        }
        else
        {
            selectedFlight.Status = "On Time";
        }

        Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
        break;
    }
}

void CreateFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().ToUpper();
        if (Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine("Flight already exists. Please enter a different Flight Number.");
            continue;
        }

        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();

        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();

        Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
        string timeInput = Console.ReadLine();
        DateTime expectedTime;

        try
        {
            // Normalize user input by trimming spaces and ensuring single digits are handled
            string[] dateParts = timeInput.Split(' ');
            string[] date = dateParts[0].Split('/');
            string[] time = dateParts[1].Split(':');

            int day = int.Parse(date[0]);
            int month = int.Parse(date[1]);
            int year = int.Parse(date[2]);
            int hour = int.Parse(time[0]);
            int minute = int.Parse(time[1]);

            expectedTime = new DateTime(year, month, day, hour, minute, 0);
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid date format. Please use the format: dd/MM/yyyy HH:mm");
            continue;
        }

        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string requestCode = Console.ReadLine().ToUpper();
        Flight newFlight;

        if (requestCode == "CFFT")
        {
            newFlight = new CFFTFlight(flightNumber, origin, destination, expectedTime, 150);
        }
        else if (requestCode == "DDJB")
        {
            newFlight = new DDJBFlight(flightNumber, origin, destination, expectedTime, 300);
        }
        else if (requestCode == "LWTT")
        {
            newFlight = new LWTTFlight(flightNumber, origin, destination, expectedTime, 500);
        }
        else
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime);
        }

        Flights[flightNumber] = newFlight;

        using (StreamWriter writer = new StreamWriter("flights.csv", true))
        {
            writer.WriteLine();
            writer.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{requestCode}");
        }

        Console.WriteLine($"Flight {flightNumber} has been added!");

        Console.Write("Would you like to add another flight? (Y/N): ");
        string response = Console.ReadLine().ToUpper();
        if (response != "Y")
        {
            break;
        }
    }
}




//Basic Feature 7
void DisplayAirlineFlights(Dictionary<string, BoardingGate> BoardingGates)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}", "Airline Code", "Airline Name");

    foreach (Airline airline in Airlines.Values)
    {
        Console.WriteLine("{0,-15}{1,-25}", airline.Code, airline.Name);
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }

    Airline selectedAirline = Airlines[airlineCode];

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected");

    List<Flight> airlineFlights = new List<Flight>();
    foreach (Flight flight in Flights.Values)
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

    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Details");
    Console.WriteLine("=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");

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
        Console.WriteLine("NORM Flight");
    }

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

    foreach (Airline airline in Airlines.Values)
    {
        Console.WriteLine("{0,-15}{1,-25}", airline.Code, airline.Name);
    }

    Console.WriteLine("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }

    Airline selectedAirline = Airlines[airlineCode];

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected");

    List<Flight> airlineFlights = new List<Flight>();
    foreach (Flight flight in Flights.Values)
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
            Flights[selectedFlight.FlightNumber] = selectedFlight;
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
        Flights.Remove(selectedFlight.FlightNumber);
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

//Basic Feature 9
void DisplayFlightSchedule()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-14} {1,-24} {2,-24} {3,-24} {4,-34} {5,-17} {6,-15}",
                      "Flight Number", "Airline Name", "Origin", "Destination", "Expected\nDpearture/Arrival Time", "Status", "Boarding Gate");

    List<Flight> sortedFlights = new List<Flight>(Flights.Values);
    sortedFlights.Sort((a, b) => a.ExpectedTime.CompareTo(b.ExpectedTime));

    foreach (Flight flight in sortedFlights)
    {
        string airlineName = "Unknown";
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        if (Airlines.ContainsKey(airlineCode))
        {
            airlineName = Airlines[airlineCode].Name;
        }

        string boardingGate = "Unassigned";
        foreach (var gate in BoardingGates)
        {
            if (gate.Value.Flight == flight)
            {
                boardingGate = gate.Key;
                break;
            }
        }

        Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}{5,-15}{6,-15}",
            flight.FlightNumber,
            airlineName,
            $"{flight.Origin}",
            $"{flight.Destination}",
            flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt"),
            flight.Status ?? "Scheduled",
            boardingGate);
    }
}

string GetAirportCode(string airportName)
{
    // This method should return the airport code based on the airport name.
    // For simplicity, let's assume we have a dictionary that maps airport names to their codes.
    Dictionary<string, string> airportCodes = new Dictionary<string, string>
    {
        { "Singapore", "SIN" },
        { "Dubai", "DXB" },
        { "Manila", "MNL" },
        { "Melbourne", "MEL" },
        { "London", "LHR" },
        { "Bangkok", "BKK" },
        { "Tokyo", "NRT" },
        { "Kuala Lumpur", "KUL" },
        { "Jakarta", "CGK" },
        { "Hong Kong", "HKD" },
        { "Sydney", "SYD" }
    };

    return airportCodes.ContainsKey(airportName) ? airportCodes[airportName] : "Unknown";
}

//Advanced Feature: Bulk Assign Boarding Gates
void BulkAssignBoardingGates()
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    int totalUnassignedFlights = 0;
    int totalUnassignedGates = 0;
    int successfullyAssigned = 0;

    foreach (Flight flight in Flights.Values)
    {
        bool hasGate = false;
        foreach (var gate in BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                hasGate = true;
                break;
            }
        }
        if (!hasGate)
        {
            unassignedFlights.Enqueue(flight);
            totalUnassignedFlights++;
        }
    }

    foreach (var gate in BoardingGates.Values)
    {
        if (gate.Flight == null)
        {
            totalUnassignedGates++;
        }
    }

    Console.WriteLine($"Total Unassigned Flights: {totalUnassignedFlights}");
    Console.WriteLine($"Total Unassigned Boarding Gates: {totalUnassignedGates}");

    while (unassignedFlights.Count > 0 && totalUnassignedGates > 0)
    {
        Flight currentFlight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;

        foreach (var gate in BoardingGates.Values)
        {
            if (gate.Flight == null)
            {
                if ((currentFlight is DDJBFlight && gate.SupportsDDJB) ||
                    (currentFlight is CFFTFlight && gate.SupportsCFFT) ||
                    (currentFlight is LWTTFlight && gate.SupportsLWTT) ||
                    (!(currentFlight is DDJBFlight || currentFlight is CFFTFlight || currentFlight is LWTTFlight)))
                {
                    assignedGate = gate;
                    break;
                }
            }
        }

        if (assignedGate != null)
        {
            assignedGate.Flight = currentFlight;
            successfullyAssigned++;
            totalUnassignedGates--;
            Console.WriteLine("Flight Assigned: {0,-15}{1,-25}{2,-25}{3,-25}{4,-30}{5,-15}{6,-15}",
                currentFlight.FlightNumber,
                Airlines[currentFlight.FlightNumber.Split(' ')[0]].Name,
                currentFlight.Origin,
                currentFlight.Destination,
                currentFlight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt"),
                currentFlight.Status ?? "Scheduled",
                assignedGate.GateName);
        }
    }

    double processedPercentage = (double)successfullyAssigned / (totalUnassignedFlights + successfullyAssigned) * 100;
    Console.WriteLine($"Total Flights Processed: {totalUnassignedFlights + successfullyAssigned}");
    Console.WriteLine($"Total Boarding Gates Processed: {totalUnassignedGates + successfullyAssigned}");
    Console.WriteLine($"Flights and Boarding Gates Automatically Assigned: {processedPercentage:F2}%");
}

//Advanced Feature: Calculate Total Fees per Airline
void CalculateTotalFeesPerAirline()
{
    bool allFlightsAssigned = true;
    foreach (var flight in Flights.Values)
    {
        bool hasGate = false;
        foreach (var gate in BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                hasGate = true;
                break;
            }
        }
        if (!hasGate)
        {
            allFlightsAssigned = false;
            break;
        }
    }

    if (!allFlightsAssigned)
    {
        Console.WriteLine("Not all flights have been assigned boarding gates. Please assign all flights before running this feature again.");
        return;
    }

    Dictionary<string, double> airlineFees = new Dictionary<string, double>();
    Dictionary<string, double> airlineDiscounts = new Dictionary<string, double>();

    foreach (var flight in Flights.Values)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        double flightFee = 300;

        if (flight.Destination == "Singapore (SIN)")
        {
            flightFee += 500;
        }
        else if (flight.Origin == "Singapore (SIN)")
        {
            flightFee += 800;
        }

        if (flight is DDJBFlight) flightFee += 300;
        else if (flight is CFFTFlight) flightFee += 150;
        else if (flight is LWTTFlight) flightFee += 500;

        if (!airlineFees.ContainsKey(airlineCode))
        {
            airlineFees[airlineCode] = 0;
            airlineDiscounts[airlineCode] = 0;
        }
        airlineFees[airlineCode] += flightFee;

        double discount = 0;

        if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
        {
            discount += 110;
        }

        if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
        {
            discount += 25;
        }

        if (!(flight is DDJBFlight || flight is CFFTFlight || flight is LWTTFlight))
        {
            discount += 50;
        }

        airlineDiscounts[airlineCode] += discount;
    }

    foreach (var airline in airlineFees.Keys)
    {
        int flightCount = 0;
        foreach (var flight in Flights.Values)
        {
            if (flight.FlightNumber.StartsWith(airline))
            {
                flightCount++;
            }
        }

        airlineDiscounts[airline] += (flightCount / 3) * 350;

        if (flightCount > 5)
        {
            airlineDiscounts[airline] += airlineFees[airline] * 0.03;
        }

        double finalTotal = airlineFees[airline] - airlineDiscounts[airline];
        double discountPercentage = (airlineDiscounts[airline] / airlineFees[airline]) * 100;

        Console.WriteLine("Airline: {0}", airline);
        Console.WriteLine("Subtotal Fees: ${0:F2}", airlineFees[airline]);
        Console.WriteLine("Subtotal Discounts: ${0:F2}", airlineDiscounts[airline]);
        Console.WriteLine("Final Total: ${0:F2}", finalTotal);
        Console.WriteLine("Discount Percentage: {0:F2}%\n", discountPercentage);
    }
}

void MainMenu()
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
    Console.WriteLine("8. Bulk Assign Boarding Gates");
    Console.WriteLine("9. Display total fee per airline for the day");
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
        else if (choice == "3")
        {
            AssignBoardingGate();
        }
        else if (choice == "4")
        {
            CreateFlight();
        }

        else if (choice == "5")
        {
            DisplayAirlineFlights(BoardingGates);
        }
        else if (choice == "6")
        {
            ModifyFlightDetails();
        }
        else if (choice == "7")
        {
            DisplayFlightSchedule();
        }
        else if (choice == "8")
        {
            BulkAssignBoardingGates();
        }
        else if (choice == "9")
        {
            CalculateTotalFeesPerAirline();
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

