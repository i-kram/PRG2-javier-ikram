using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInformationDisplaySystem
{ 
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public override double CalculateFees()
        {
            return 100.0 + RequestFee; // Base fee + additional request fee
        }

        public override string ToString()
        {
            return base.ToString() + $" (CFFT Flight, Request Fee: {RequestFee:C})";
        }
    }
}
