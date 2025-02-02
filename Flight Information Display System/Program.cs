//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
// Partner Number    : S10267028
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
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            if (parts.Length >= 4)
            {
                DateTime expectedTime;
                string timeInput = parts[3].Trim();
                string dateInput = DateTime.Now.ToString("MM/dd/yyyy");

                if (!DateTime.TryParseExact($"{dateInput} {timeInput}", "MM/dd/yyyy h:mm tt", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                {
                    Console.WriteLine($"Error parsing time: {parts[3].Trim()}");
                    continue;
                }

                string specialRequest = parts.Length >= 5 ? parts[4].Trim() : "";
                Flight flight;

                if (specialRequest == "DDJB")
                {
                    flight = new DDJBFlight(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), expectedTime, 300);
                }
                else if (specialRequest == "CFFT")
                {
                    flight = new CFFTFlight(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), expectedTime, 150);
                }
                else if (specialRequest == "LWTT")
                {
                    flight = new LWTTFlight(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), expectedTime, 500);
                }
                else
                {
                    flight = new NORMFlight(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), expectedTime);
                }

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
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

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

        Console.WriteLine("Would you like to update the status of the flight? (Y/N) ");
        string updateStatus = Console.ReadLine().ToUpper();
        if (updateStatus == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.WriteLine("Please select the new status of the flight: ");
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

        Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
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

        Console.WriteLine("Would you like to add another flight? (Y/N): ");
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

    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
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

    Console.WriteLine("Choose an existing Flight to modify or delete: ");
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
    Console.WriteLine("Choose an option: ");    
    string option = Console.ReadLine();

    if (option == "1")
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.WriteLine("Choose an option: ");
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
    Console.WriteLine("{0,-15} {1,-23} {2,-22} {3,-20} {4,-34} {5,-15} {6,-15}",
                      "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Boarding Gate");

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

        Console.WriteLine("{0,-15} {1,-23} {2,-22} {3,-20} {4,-34} {5,-15} {6,-15}",
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
    int preAssignedFlights = 0;

    // Count pre-assigned flights (already assigned to gates)
    foreach (var gate in BoardingGates.Values)
        {
        if (gate.Flight != null)
            {
            preAssignedFlights++;
            }
        }

    // Track initial unassigned flights and gates
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

    // Capture initial counts before processing
    int initialUnassignedFlights = totalUnassignedFlights;
    int initialUnassignedGates = totalUnassignedGates;
    int totalGates = BoardingGates.Count; // Total gates in the system

    Console.WriteLine($"Total Unassigned Flights: {initialUnassignedFlights}");
    Console.WriteLine($"Total Unassigned Boarding Gates: {initialUnassignedGates}");

    // Assign gates to flights
    while (unassignedFlights.Count > 0 && totalUnassignedGates > 0)
        {
        Flight currentFlight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;
        foreach (var gate in BoardingGates.Values)
            {
            if (gate.Flight == null)
                {
                bool isMatch = (currentFlight is DDJBFlight && gate.SupportsDDJB) ||
                               (currentFlight is CFFTFlight && gate.SupportsCFFT) ||
                               (currentFlight is LWTTFlight && gate.SupportsLWTT) ||
                               (!(currentFlight is DDJBFlight || currentFlight is CFFTFlight || currentFlight is LWTTFlight));
                if (isMatch)
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

            string specialRequestCode = "";
            if (currentFlight is DDJBFlight) specialRequestCode = "DDJB";
            else if (currentFlight is CFFTFlight) specialRequestCode = "CFFT";
            else if (currentFlight is LWTTFlight) specialRequestCode = "LWTT";

            Console.WriteLine("Gate name: {0,-4} Flight: {1,-15} {2,-22} {3,-22} {4,-20} {5}",
                assignedGate.GateName,
                currentFlight.FlightNumber,
                currentFlight.Origin,
                currentFlight.Destination,
                currentFlight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt").ToLower(),
                specialRequestCode);
            }
        }

    // Calculate percentages
    double flightPercentage = initialUnassignedFlights == 0 ? 0 :
        (double)successfullyAssigned / initialUnassignedFlights * 100;

    double gatePercentage = totalGates == 0 ? 0 :
        (double)successfullyAssigned / totalGates * 100;

    Console.WriteLine("\nProcessing Summary:");
    Console.WriteLine($"Total Flights processed and assigned: {successfullyAssigned}");
    Console.WriteLine($"Total Boarding Gates processed and assigned: {successfullyAssigned}");
    Console.WriteLine($"Percentage of automatic flight assignments: {flightPercentage:F2}%");
    Console.WriteLine($"Percentage of automatic gate assignments: {gatePercentage:F2}%");
    }

void CalculateTotalFeesPerAirline()
    {
    // Check that every flight has an assigned boarding gate
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

    // Store fees and discounts for each airline
    Dictionary<string, double> airlineFees = new Dictionary<string, double>();
    Dictionary<string, double> airlineDiscounts = new Dictionary<string, double>();

    foreach (var flight in Flights.Values)
        {
        string airlineCode = flight.FlightNumber.Substring(0, 2); // Airline code from flight number
        double flightFee = 300; // Base fee

        // Add fees for SIN origin/destination
        if (flight.Origin == "Singapore (SIN)")
            {
            flightFee += 800;
            }
        else if (flight.Destination == "Singapore (SIN)")
            {
            flightFee += 500;
            }

        // Special request fee
        if (flight is DDJBFlight ddjb)
            {
            flightFee += ddjb.RequestFee;
            }
        else if (flight is CFFTFlight cfft)
            {
            flightFee += cfft.RequestFee;
            }
        else if (flight is LWTTFlight lwtt)
            {
            flightFee += lwtt.RequestFee;
            }

        // Initialize airline fees if not already present
        if (!airlineFees.ContainsKey(airlineCode))
            {
            airlineFees[airlineCode] = 0;
            airlineDiscounts[airlineCode] = 0;
            }
        airlineFees[airlineCode] += flightFee;

        // Apply discounts
        double discount = 0;
        if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
            {
            discount += 110; // $110 discount for off-peak flights
            }

        if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
            {
            discount += 25; // $25 discount for specific origins
            }

        if (!(flight is DDJBFlight || flight is CFFTFlight || flight is LWTTFlight))
            {
            discount += 50; // $50 discount for no special request code
            }

        airlineDiscounts[airlineCode] += discount;
        }

    // Apply the additional 3% discount for airlines with more than 5 flights
    foreach (var airline in airlineFees.Keys.ToList())
        {
        int flightCount = 0;
        foreach (var flight in Flights.Values)
            {
            if (flight.FlightNumber.StartsWith(airline))
                {
                flightCount++;
                }
            }

        airlineDiscounts[airline] += (flightCount / 3) * 350; // $350 discount for every 3 flights

        if (flightCount > 5)
            {
            airlineDiscounts[airline] += airlineFees[airline] * 0.03; // 3% discount if more than 5 flights
            }
        }

    // Display results
    double totalFees = 0;
    double totalDiscounts = 0;
    double finalTotal = 0;

    Console.WriteLine("{0,-20} {1,-30} {2,-20} {3,-20}", "Airline", "Total Fees", "Discounts", "Final Total");
    foreach (var airline in airlineFees.Keys)
        {
        double finalFee = airlineFees[airline] - airlineDiscounts[airline];
        Console.WriteLine("{0,-20} {1,-30:F2} {2,-20:F2} {3,-20:F2}", airline, airlineFees[airline], airlineDiscounts[airline], finalFee);

        totalFees += airlineFees[airline];
        totalDiscounts += airlineDiscounts[airline];
        finalTotal += finalFee;
        }

    double discountPercentage = (totalDiscounts / totalFees) * 100;

    Console.WriteLine("\nTotal Fees: {0:F2}", totalFees);
    Console.WriteLine("Total Discounts: {0:F2}", totalDiscounts);
    Console.WriteLine("Final Total Fees: {0:F2}", finalTotal);
    Console.WriteLine("Discount Percentage: {0:F2}%", discountPercentage);
    }



//Advanced Feature: Flight Status Updates
void UpdateFlightStatuses()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Automated Flight Status Updates");
    Console.WriteLine("=============================================");

    DateTime currentTime = DateTime.Now;

    foreach (Flight flight in Flights.Values)
    {
        TimeSpan timeUntilDeparture = flight.ExpectedTime - currentTime;
        bool hasGate = false;

        foreach (var gate in BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                hasGate = true;
                break;
            }
        }

        if (timeUntilDeparture.TotalMinutes <= 30 && !hasGate)
        {
            flight.Status = "Boarding";
            Console.WriteLine($"Flight {flight.FlightNumber} is now Boarding.");
        }
        else if (timeUntilDeparture.TotalMinutes <= 0 && hasGate)
        {
            flight.Status = "Departed";
            Console.WriteLine($"Flight {flight.FlightNumber} has Departed.");
        }
        else if (flight.Status == "Delayed" && timeUntilDeparture.TotalMinutes < -60)
        {
            Console.WriteLine($"Flight {flight.FlightNumber} is Delayed by more than 60 minutes.");
        }
    }

    Console.Write("Would you like to manually cancel a flight? (Y/N): ");
    string response = Console.ReadLine().ToUpper();
    if (response == "Y")
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().ToUpper();
        if (Flights.ContainsKey(flightNumber))
        {
            Flights[flightNumber].Status = "Cancelled";
            Console.WriteLine($"Flight {flightNumber} has been Cancelled.");
        }
        else
        {
            Console.WriteLine("Invalid Flight Number.");
        }
    }
}

//Advanced Feature: Predictive Delay Notification
void PredictiveDelayNotification()
    {
    Console.WriteLine("=============================================");
    Console.WriteLine("Predictive Delay Notification System");
    Console.WriteLine("=============================================");

    DateTime currentTime = DateTime.Now;

    foreach (Flight flight in Flights.Values)
        {
        TimeSpan timeUntilDeparture = flight.ExpectedTime - currentTime;

        if (timeUntilDeparture.TotalMinutes < 60 && timeUntilDeparture.TotalMinutes > 30)
            {
            Console.WriteLine($"Warning: Flight {flight.FlightNumber} might be delayed due to operational constraints.");
            }
        else if (timeUntilDeparture.TotalMinutes <= 30 && timeUntilDeparture.TotalMinutes > 10)
            {
            Console.WriteLine($"Urgent: Flight {flight.FlightNumber} is at high risk of delay. Immediate attention required.");
            }
        else if (timeUntilDeparture.TotalMinutes <= 10)
            {
            Console.WriteLine($"Critical: Flight {flight.FlightNumber} is likely delayed. Update the passengers immediately.");
            }
        }
    }

void MainMenu()
    {
    Console.WriteLine("\n\n\n\n=============================================");
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
    Console.WriteLine("10. Update Flight Statuses");
    Console.WriteLine("11. Predictive Delay Notification");
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
        else if (choice == "10")
        {
            UpdateFlightStatuses();
        }
        else if (choice == "11")
        {
            PredictiveDelayNotification();
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

