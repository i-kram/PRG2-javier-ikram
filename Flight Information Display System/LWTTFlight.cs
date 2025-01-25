using System;

namespace FlightInformationDisplaySystem
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }

        public override double CalculateFees()
        {
            return 80.0 + RequestFee; // Base fee + additional request fee
        }

        public override string ToString()
        {
            return base.ToString() + $" (LWTT Flight, Request Fee: {RequestFee:C})";
        }
    }
}