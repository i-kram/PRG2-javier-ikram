using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInformationDisplaySystem
    {
    public class NORMFlight : Flight
        {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
            : base(flightNumber, origin, destination, expectedTime)
            {
            }

        public override double CalculateFees()
            {
            double baseFee = 300;
            double arrivalFee = 500;
            double departureFee = 800;

            return baseFee + (Destination == "Singapore (SIN)" ? arrivalFee : departureFee);
            }
        }
    }