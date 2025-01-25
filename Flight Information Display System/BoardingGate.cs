using System;

namespace FlightInformationDisplaySystem
{
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public double CalculateFees()
        {
            if (Flight == null) return 0;

            double baseFee = 300;
            double specialRequestFee = 0;

            if (Flight is CFFTFlight cfftFlight)
            {
                specialRequestFee = cfftFlight.RequestFee;
            }
            else if (Flight is LWTTFlight lwttFlight)
            {
                specialRequestFee = lwttFlight.RequestFee;
            }
            else if (Flight is DDJBFlight ddjbFlight)
            {
                specialRequestFee = ddjbFlight.RequestFee;
            }

            return baseFee + specialRequestFee;
        }

        public override string ToString()
        {
            return $"Gate: {GateName}, Flight: {(Flight != null ? Flight.FlightNumber : "None")}";
        }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }
    }
}