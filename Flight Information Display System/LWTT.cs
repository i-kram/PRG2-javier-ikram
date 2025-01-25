using System;

namespace FlightInformationDisplaySystem
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; } = 500;

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee)
            : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            double baseFee = 300;
            double arrivalFee = 500;
            double departureFee = 800;

            return baseFee + RequestFee + (Destination == "Singapore (SIN)" ? arrivalFee : departureFee);
        }
    }
}