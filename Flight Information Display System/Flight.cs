//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================
using System;

namespace FlightInformationDisplaySystem
    {
    public class Flight
        {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
            {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            }
        public virtual double CalculateFees()
            {
            return 0;
            }

        public override string ToString()
            {
            return $"Flight {FlightNumber} from {Origin} to {Destination}, Expected: {ExpectedTime}, Status: {Status}";
            }
        }
    }