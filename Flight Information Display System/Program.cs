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
void LoadAirlines(string filename) //Reads airlines data from a CSV file
    {
    Console.WriteLine("Loading Airlines..."); //Display message to indicate that the airlines are being loaded
    try// - Uses try-catch for robust error handling
        {
        string[] lines = File.ReadAllLines(filename); //Read all lines from the file
        for (int i = 1; i < lines.Length; i++) // iterate // - Skips header row by starting at index 1
            {
            string[] parts = lines[i].Split(',');
            if (parts.Length >= 2)// - Validates CSV has at least 2 columns before processing
                {
                Airline airline = new Airline(parts[0], parts[1]);
                Airlines[airline.Code] = airline; //Updates the Airlines dictionary with the airline code as the key and the Airline object as the value
                }
        }
        Console.WriteLine($"{Airlines.Count} Airlines Loaded!"); //Display the number of airlines loaded
        }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading airlines: {ex.Message}");
    }
}

//Basic Feature 1
void LoadBoardingGates(string filename)
    {
    Console.WriteLine("Loading Boarding Gates...");//Display message to indicate that the boarding gates are being loaded
    try
        {
        string[] lines = File.ReadAllLines(filename);//Read all lines from the file
        for (int i = 1; i < lines.Length; i++)// iterate // - Skips header row by starting at index 1
            {
            string[] parts = lines[i].Split(',');//Split each line by comma to get individual parts
            if (parts.Length >= 4) // - Validates CSV has at least 4 columns before processing
                {
                // Assuming you might want to set Flight to null if not available
                BoardingGate gate = new BoardingGate(
                    parts[0],
                    ConvertToBoolean(parts[1]), //uses helper method to convert string to boolean
                    ConvertToBoolean(parts[2]),
                    ConvertToBoolean(parts[3]),
                    null // Or a valid Flight object if needed
                );//creates a new BoardingGate object matching the BoardingGate class constructor
                BoardingGates[gate.GateName] = gate;
                }
            }
        Console.WriteLine($"{BoardingGates.Count} Boarding Gates Loaded!");//Display the number of boarding gates loaded
        }
    catch (Exception ex)
        {
        Console.WriteLine($"Error loading boarding gates: {ex.Message}");
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


// Basic Feature 2
void LoadFlights(string filename)
    {
    Console.WriteLine("Loading Flights...");//Display message to indicate that the flights are being loaded
    try// - Uses try-catch for robust error handling
        {
        string[] lines = File.ReadAllLines(filename); //Read all lines from the file
        for (int i = 1; i < lines.Length; i++) // Skipping header row start at index 1
            {
            string[] parts = lines[i].Split(',');//Split each line by comma to get individual parts
            if (parts.Length >= 4) // - Validates CSV has at least 4 columns before processing
                {
                // Extracting flight details from the CSV
                string flightNumber = parts[0];
                string origin = parts[1];
                string destination = parts[2];
                string timeInput = parts[3];

                // Combine the current date and the time from the input to create a complete DateTime
                string dateInput = DateTime.Now.ToString("MM/dd/yyyy");
                //initialise expectedTime
                DateTime expectedTime;

                //formatting the date and time
                if (!DateTime.TryParseExact($"{dateInput} {timeInput}", "MM/dd/yyyy h:mm tt", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                    {
                    Console.WriteLine($"Error parsing time: {parts[3]}");
                    continue;
                    }

                string specialRequest = parts.Length >= 5 ? parts[4] : ""; // Check if special request is available in the csv file

                // intialise flight object
                Flight flight;

                if (specialRequest == "DDJB")
                    {
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, 300);
                    }
                else if (specialRequest == "CFFT")
                    {
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, 150);
                    }
                else if (specialRequest == "LWTT")
                    {
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, 500);
                    }
                else
                    {
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
                    }

                Flights[flight.FlightNumber] = flight; // Storing flight in the Flights dictionary with flightnumber as key
                }
            }
        Console.WriteLine($"{Flights.Count} Flights Loaded!"); //Display the number of flights loaded
        }
    catch (Exception ex)
        {
        Console.WriteLine($"Error loading flights: {ex.Message}");
        }
    }



//Basic Feature 3
void ListAllFlights()
{
    //consistent formatting for the list of flights
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}",
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    foreach (Flight flight in Flights.Values)//iterate through the Flights dictionary
        {
        string airlineName = "Unknown"; //default airline name
        if (Airlines.ContainsKey(flight.FlightNumber.Substring(0, 2))) //check if the airline code is in the Airlines dictionary
            {
            airlineName = Airlines[flight.FlightNumber.Substring(0, 2)].Name; //get the airline name from the Airlines dictionary
            }

        Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-30}", //format the output
            flight.FlightNumber,
            airlineName,
            flight.Origin,
            flight.Destination,
            flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
    }
}

//Basic Feature 4
void ListBoardingGates()
    {
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}",
        "Gate Name", "DDJB", "CFFT", "LWTT");

    foreach (KeyValuePair<string, BoardingGate> gateEntry in BoardingGates) //iterate through the BoardingGates dictionary
        {
        BoardingGate gate = gateEntry.Value; //get the BoardingGate object from the KeyValuePair
        Console.WriteLine("{0,-15}{1,-22}{2,-22}{3,-22}", 
            gate.GateName,
            //checks if the gate supports a special request and prints "True" or "False" accordingly
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
    Flight selectedFlight = Flights[flightNumber]; //get the flight number property of the Flight object from the Flights dictionary

    string specialRequest = "None";
    if (selectedFlight is DDJBFlight) specialRequest = "DDJB";
    else if (selectedFlight is CFFTFlight) specialRequest = "CFFT";
    else if (selectedFlight is LWTTFlight) specialRequest = "LWTT";

    while (true)
    {
        Console.WriteLine("Enter Boarding Gate Name: ");
        string gateName = Console.ReadLine().ToUpper();
        if (!BoardingGates.ContainsKey(gateName)) //check if the gate name is in the BoardingGates dictionary
            {
            Console.WriteLine("Invalid Boarding Gate. Please try again.");
            continue;
        }
        BoardingGate selectedGate = BoardingGates[gateName]; //get the gateName property of the BoardingGate object from the BoardingGates dictionary
        if (selectedGate.Flight != null) //check if the gate is already assigned to a flight
            {
            Console.WriteLine("The selected Boarding Gate is already assigned to another flight. Please choose a different gate.");
            continue;
        }
        selectedGate.Flight = selectedFlight; //assign the selected flight to the selected gate
        // Display the flight details and boarding gate assignment
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
            //  Update the status of the flight
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.WriteLine("Please select the new status of the flight: ");
            string statusChoice = Console.ReadLine();
            //modify the status property of the selected Flight object based on the user input
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

//Basic Feature 6
void CreateFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().ToUpper();

        // Manual validation: Check length, prefix, space, and digits
        if (flightNumber.Length != 6 ||
                !(flightNumber[0] >= 'A' && flightNumber[0] <= 'Z') ||
                !(flightNumber[1] >= 'A' && flightNumber[1] <= 'Z') ||
                flightNumber[2] != ' ' ||
                !(flightNumber[3] >= '0' && flightNumber[3] <= '9') ||
                !(flightNumber[4] >= '0' && flightNumber[4] <= '9') ||
                !(flightNumber[5] >= '0' && flightNumber[5] <= '9'))
            {
            Console.WriteLine("Invalid flight format. Please enter in the format 'SQ 999'.");
            continue;
            }

        // Check if flight already exists
        if (Flights.ContainsKey(flightNumber))
            {
            Console.WriteLine("Flight already exists. Please enter a different Flight Number.");
            continue;
            }

        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();
        if (string.IsNullOrEmpty(origin) || origin.Any(char.IsDigit))
            {
            Console.WriteLine("Invalid input. Please enter a valid origin without numbers.");
            continue;
            }

        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();
        if (string.IsNullOrEmpty(destination) || destination.Any(char.IsDigit))
            {
            Console.WriteLine("Invalid input. Please enter a valid destination without numbers.");
            continue;            }
        

        Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        string dateTimeInput = Console.ReadLine();

        DateTime expectedDateTime; //initialise expectedDateTime
        try //try to format the date and time input
            {
            expectedDateTime = DateTime.ParseExact(dateTimeInput, "dd/MM/yyyy HH:mm", null); //format the date and time input
            }
        catch (Exception)
        {
            Console.WriteLine("Invalid date or time format. Please use the format: dd/MM/yyyy HH:mm.");
            CreateFlight();
            return;
            
        }

        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string requestCode = Console.ReadLine().ToUpper();
        Flight newFlight; //initialise newFlight

        // Create the appropriate flight object based on the special request code
        if (requestCode == "CFFT")
        {
            newFlight = new CFFTFlight(flightNumber, origin, destination, expectedDateTime, 150);
        }
        else if (requestCode == "DDJB")
        {
            newFlight = new DDJBFlight(flightNumber, origin, destination, expectedDateTime, 300);
        }
        else if (requestCode == "LWTT")
        {
            newFlight = new LWTTFlight(flightNumber, origin, destination, expectedDateTime, 500);
        }
        else
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedDateTime);
        }

        Flights[flightNumber] = newFlight;

        // Write the flight details to a CSV file in the format: FlightNumber,Origin,Destination,ExpectedTime,SpecialRequestCode
        using (StreamWriter writer = new StreamWriter("flights.csv", true))
        {
            // Write the flight details including the time formatted as "hh:mm tt"
            writer.WriteLine($"{flightNumber},{origin},{destination},{expectedDateTime:hh:mm tt},{requestCode}");
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

    foreach (Airline airline in Airlines.Values) //iterate through the Airlines dictionary to get the Values of Airline objects
        {
        Console.WriteLine("{0,-15}{1,-25}", airline.Code, airline.Name);
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!Airlines.ContainsKey(airlineCode)) //check if the airline code is in the Airlines dictionary
        {
        Console.WriteLine("Invalid Airline Code. Please try again.");
        return;
    }

    Airline selectedAirline = Airlines[airlineCode]; //get the airline code property of the Airline object from the Airlines dictionary

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected");

    List<Flight> airlineFlights = new List<Flight>(); //initialise airlineFlights as a new List of Flight objects
    foreach (Flight flight in Flights.Values) //iterate through the Flights dictionary to get the Values of Flight objects
        {
        if (flight.FlightNumber.StartsWith(selectedAirline.Code)) //check if the flight number starts with the selected airline code
            {
            airlineFlights.Add(flight); //add the flight to the airlineFlights list
            Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}",
                flight.FlightNumber,
                selectedAirline.Name,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
        }
    }

    if (airlineFlights.Count == 0) //check if there are no flights in the airlineFlights list
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
    Console.WriteLine("{0,-15}{1,-25}{2,-25}{3,-25}{4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

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

    Flight selectedFlight = null; //initialise selectedFlight as empty
    foreach (var flight in airlineFlights) //iterate through the airlineFlights list
        {
        if (flight.FlightNumber == flightNumber) //check if the flight number matches the user input
            {
            selectedFlight = flight; //assign the flight to the selectedFlight variable
            break;
            }
        }

    if (selectedFlight == null) //check if the selectedFlight is does not exist
        {
        Console.WriteLine("Invalid Flight Number. Please try again.");
        return;
        }

    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.WriteLine("Choose an option: ");
    string option = Console.ReadLine();

    if (option == "1") // modify flight details
        {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.WriteLine("Choose an option: ");
        string modifyOption = Console.ReadLine();

        if (modifyOption == "1") // modify basic information
            {
            Console.Write("Enter new Origin: ");
            selectedFlight.Origin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            selectedFlight.Destination = Console.ReadLine();

            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string newDateTimeInput = Console.ReadLine();
            DateTime newExpectedTime;
            //formatting the date and time
            if (DateTime.TryParseExact(newDateTimeInput, "dd/M/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out newExpectedTime))
                {
                selectedFlight.ExpectedTime = newExpectedTime;
                if (selectedFlight.ExpectedTime > DateTime.Now) //check if the expected time is in the future
                    {
                    selectedFlight.Status = "Scheduled";
                    }
                else
                    {
                    selectedFlight.Status = "Delayed";
                    }
                Console.WriteLine("Flight updated!");
                }
            else
                {
                Console.WriteLine("Invalid date or time format. Please use dd/MM/yyyy HH:mm."); //display error message if the date and time format is invalid
                return;
                }
            }
        else if (modifyOption == "2")
            {
            Console.Write("Enter new Status: ");
            string newStatus = Console.ReadLine();
            if (string.IsNullOrEmpty(newStatus)) //check if the status input is empty
                {
                Console.WriteLine("Invalid status input. Please try again.");
                return;
                }
            selectedFlight.Status = newStatus; //update the status property of the selectedFlight object
            Console.WriteLine("Flight status updated!");
            }
        else if (modifyOption == "3")
            {
            Console.WriteLine("Enter new Special Request Code (DDJB, CFFT, LWTT, NORM): ");
            string newCode = Console.ReadLine().ToUpper();
            //update the selectedFlight based on the special request code to create a new flight object
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
            else if (newCode == "NORM")
                {
                selectedFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
                }
            else
                {
                Console.WriteLine("Invalid Special Request Code.");
                return;
                }
            Flights[selectedFlight.FlightNumber] = selectedFlight; //update the Flights dictionary with the new flight object
            Console.WriteLine("Special Request Code updated!");
            }
        else if (modifyOption == "4")
            {
            Console.WriteLine("Available Boarding Gates:");
            foreach (var gateEntry in BoardingGates) //iterate through the BoardingGates dictionary
                {
                BoardingGate gate = gateEntry.Value; //get the BoardingGate object from the KeyValuePair
                //check if the gate supports the special request code of the selected flight
                if ((gate.SupportsDDJB && selectedFlight is DDJBFlight) ||
                    (gate.SupportsCFFT && selectedFlight is CFFTFlight) ||
                    (gate.SupportsLWTT && selectedFlight is LWTTFlight))
                    {
                    Console.WriteLine($"{gate.GateName}"); //display the gate name property of BoardingGate object if it supports the special request code
                    }
                }
            Console.Write("Enter new Boarding Gate: ");
            string newGate = Console.ReadLine();
            if (BoardingGates.ContainsKey(newGate)) //check if the new gate is in the BoardingGates dictionary
                {
                BoardingGates[newGate].Flight = selectedFlight; //assign the selected flight to the new gate
                Console.WriteLine("Boarding Gate updated!");
                }
            else
                {
                Console.WriteLine("Invalid Boarding Gate. Please try again.");
                return;
                }
            }
        else
            {
            Console.WriteLine("Invalid modify option. Please try again.");
            return;
            }
        }
    else if (option == "2") // delete flight
        {
        Flights.Remove(selectedFlight.FlightNumber); //remove the selected flight from the Flights dictionary
        Console.WriteLine("Flight deleted!");
        }
    else
        {
        Console.WriteLine("Invalid option. Please try again.");
        return;
        }

    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    Console.WriteLine($"Status: {selectedFlight.Status}");

    // Display the special request code based on the flight type
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
    // Display the boarding gate assigned to the flight
    Console.WriteLine("Boarding Gate: Unassigned");
    }


// Basic Feature 9: Display Flight Schedule
void DisplayFlightSchedule()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15} {1,-23} {2,-22} {3,-20} {4,-34} {5,-15} {6,-15}",
                      "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Boarding Gate");

    List<Flight> sortedFlights = new List<Flight>(Flights.Values); // Copy the flights to a list for sorting
    sortedFlights.Sort((a, b) => a.ExpectedTime.CompareTo(b.ExpectedTime)); // Sort the flights by expected time

    foreach (Flight flight in sortedFlights) // Iterate through every flight object in the sorted list
        {
        string airlineName = "Unknown"; // Default airline name
        string airlineCode = flight.FlightNumber.Split(' ')[0]; // Extract airline code from flight number
        if (Airlines.ContainsKey(airlineCode)) //Check if the airline code is in the dictionary
        {
            airlineName = Airlines[airlineCode].Name; // Get the airline name from the dictionary
            }

        string boardingGate = "Unassigned"; // Default boarding gate
        foreach (var gate in BoardingGates) // Iterate through boarding gates
        {
            if (gate.Value.Flight == flight) // check if the flight is assigned to the gate access flight property from boardinggate object
                {
                boardingGate = gate.Key; // Get the gate name
                break;
                }
        }

        string specialRequest = "None"; // Default special request
        if (flight is DDJBFlight) specialRequest = "DDJB";
        else if (flight is CFFTFlight) specialRequest = "CFFT";
        else if (flight is LWTTFlight) specialRequest = "LWTT";

        Console.WriteLine("{0,-15} {1,-23} {2,-22} {3,-20} {4,-34} {5,-15} {6,-15}",
            flight.FlightNumber,
            airlineName,
            flight.Origin,
            flight.Destination,
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
    Queue<Flight> unassignedFlights = new Queue<Flight>(); // Queue to store unassigned flights
    // Counters for tracking assignments
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
            totalUnassignedGates++;// Count unassigned gates
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
        bool hasGate = false; // Flag to check if flight has a gate assigned
        foreach (var gate in BoardingGates.Values)
            {
            if (gate.Flight == flight)// Check if the gate is assigned to the flight
                {
                hasGate = true;
                break;
                }
            }
        if (!hasGate)// If the flight does not have a gate assigned, set flag to false and break the loop
            {
            allFlightsAssigned = false;
            break;
            }
        }

    if (!allFlightsAssigned) // If not all flights have been assigned boarding gates, display error message and return
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

        //add Special request fee to the base fee
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

        airlineDiscounts[airlineCode] += discount; // Add discount to the airline
        }

    // Apply the additional 3% discount for airlines with more than 5 flights
    foreach (var airline in airlineFees.Keys.ToList()) // Iterate through the keys of the airlineFees dictionary
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




//Advanced feature: Update Flight Status
void UpdateFlightStatus()
    {
    DateTime currentTime = DateTime.Now;

    foreach (var flight in Flights.Values)
        {
        if (flight.ExpectedTime < currentTime)
            {
            flight.Status = "Delayed";
            }
        else if ((flight.ExpectedTime - currentTime).TotalMinutes <= 30)
            {
            if (flight.Status != null && flight.Status.Contains("Departure"))
                {
                flight.Status = "Departing";
                }
            else
                {
                flight.Status = "Arriving";
                }
            }
        }

    Console.WriteLine("=============================================");
    Console.WriteLine("Updated Flight Status for Changi Airport Terminal 5");
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

        string specialRequest = "None";
        if (flight is DDJBFlight) specialRequest = "DDJB";
        else if (flight is CFFTFlight) specialRequest = "CFFT";
        else if (flight is LWTTFlight) specialRequest = "LWTT";

        Console.WriteLine("{0,-15} {1,-23} {2,-22} {3,-20} {4,-34} {5,-15} {6,-15}",
            flight.FlightNumber,
            airlineName,
            flight.Origin,
            flight.Destination,
            flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt"),
            flight.Status ?? "Scheduled",
            boardingGate);
        }
    }



void MainMenu()
    {
    Console.WriteLine("\n\n\n\n=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights"); //ikram
    Console.WriteLine("2. List Boarding Gates"); //javier
    Console.WriteLine("3. Assign a Boarding Gate to a Flight"); //javier
    Console.WriteLine("4. Create Flight"); //ikram
    Console.WriteLine("5. Display Airline Flights"); //javier
    Console.WriteLine("6. Modify Flight Details"); //ikram
    Console.WriteLine("7. Display Flight Schedule"); //javier
    Console.WriteLine("8. Bulk Assign Boarding Gates"); //javier 
    Console.WriteLine("9. Display total fee per airline for the day"); //ikram
    Console.WriteLine("10. Update flight status"); //javier //ikram
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
            UpdateFlightStatus();
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

Run(); 

    // Entry point
//    void Main() // why not straight to Run()?: Run() is a method that loads the data from the CSV files and displays the main menu. Hence, it is called in the Main method.
//    {
//        Run();
//    }
////what happens if straight away Run(): 
//    Main(); // Call the Main method to start the program


