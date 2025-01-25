using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInformationDisplaySystem
{
    public class NORMFlight : Flight
    {
        public override double CalculateFees()
        {
            return 50.0; // Flat rate for normal flights
        }

        public override string ToString()
        {
        return base.ToString() + " (Normal Flight)";
        }
    }
}
