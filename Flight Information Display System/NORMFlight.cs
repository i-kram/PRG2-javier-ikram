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
            return 300.0 + 800.0 
        }

        public override string ToString()
        {
        return base.ToString() + " (Normal Flight)";
        }
    }
}
