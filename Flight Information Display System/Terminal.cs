using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInformationDisplaySystem
    {
    public class Terminal
        {
        public string TerminalName { get; set; }
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        private Dictionary<string, double> gateFees = new Dictionary<string, double>();

        public bool AddAirline(Airline airline)
            {
            if (!airlines.ContainsKey(airline.Code))
                {
                airlines.Add(airline.Code, airline);
                return true;
                }
            return false;
            }

        public bool AddBoardingGate(BoardingGate gate)
            {
            if (!boardingGates.ContainsKey(gate.GateName))
                {
                boardingGates.Add(gate.GateName, gate);
                return true;
                }
            return false;
            }

        public Airline GetAirlineFromFlight(Flight flight)
            {
            foreach (var airline in airlines.Values)
                {
                if (flight.FlightNumber.StartsWith(airline.Code))
                    return airline;
                }
            return null;
            }

        public void PrintAirlineFees()
            {
            Console.WriteLine("Airline Fees:");
            foreach (var fee in gateFees)
                {
                Console.WriteLine($"Gate {fee.Key}: ${fee.Value:F2}");
                }
            }

        public override string ToString()
            {
            return $"Terminal: {TerminalName}, Airlines: {airlines.Count}, Boarding Gates: {boardingGates.Count}";
            }
        }
    }