using System;

public class BoardingGate
    {
    public string GateName { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    public double CalculateFees()
        {
        return 0;
        }

    public override string ToString()
        {
        return $"Gate: {GateName}, DDJB: {SupportsDDJB}, CFFT: {SupportsCFFT}, LWTT: {SupportsLWTT}";
        }
    }