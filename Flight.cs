using System;

public class Flight
    {
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }

    public double CalculateFees()
        {
        // Placeholder implementation
        return 0;
        }

    public override string ToString()
        {
        return $"{FlightNumber} from {Origin} to {Destination} at {ExpectedTime:dd/M/yyyy h:mm:ss tt}";
        }
    }