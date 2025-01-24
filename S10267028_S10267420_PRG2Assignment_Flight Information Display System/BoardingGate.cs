using System;

public class BoardingGate
    {
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public DateTime SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    public double CalculateFees()
        {
        // Placeholder implementation
        // You might want to add specific fee calculation logic
        return 0;
        }

    public override string ToString()
        {
        string specialRequests = GetSpecialRequestCodes();
        string flightInfo = Flight != null ? Flight.FlightNumber : "No Flight";
        return $"Gate: {GateName}, Special Requests: {specialRequests}, Assigned Flight: {flightInfo}";
        }

    private string GetSpecialRequestCodes()
        {
        var requests = new List<string>();
        if (SupportsCFFT) requests.Add("CFFT");
        if (SupportsDDJB) requests.Add("DDJB");
        if (SupportsLWTT != default) requests.Add("LWTT");
        return string.Join(", ", requests);
        }
    }