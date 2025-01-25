using System;

namespace FlightInformationDisplaySystem
{
    public class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }

        public override double CalculateFees()
        {
            return 70.0 + RequestFee; // Base fee + additional request fee
        }

        public override string ToString()
        {
            return base.ToString() + $" (DDJB Flight, Request Fee: {RequestFee:C})";
        }
    }
}