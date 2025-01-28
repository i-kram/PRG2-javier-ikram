//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================
using System;

namespace FlightInformationDisplaySystem
    {
    public class CFFTFlight : Flight
        {
        public double RequestFee { get; set; } = 150;

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee)
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
        public override string ToString()
            {
            return base.ToString() + $" (CFFT Flight, Request Fee: {RequestFee:C})";
            }
        }
    }
