using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
