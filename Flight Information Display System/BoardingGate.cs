//==========================================================
// Student Number	: S10267420
// Student Name	: Muhammad Ikram
// Partner Name	: Javier Yeo
//==========================================================
using System;

namespace FlightInformationDisplaySystem
    {
    public class BoardingGate
        {
        public string GateName { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate(string gateName, bool supportsDDJB, bool supportsCFFT, bool supportsLWTT, Flight flight)
            {
            GateName = gateName;
            SupportsDDJB = supportsDDJB;
            SupportsCFFT = supportsCFFT;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
            }
        public double CalculateFees()
            {
            if (Flight == null) return 0;

            double baseFee = 300;
            double specialRequestFee = 0;

            if (Flight is CFFTFlight cfftFlight)
                {
                specialRequestFee = cfftFlight.RequestFee;
                }
            else if (Flight is LWTTFlight lwttFlight)
                {
                specialRequestFee = lwttFlight.RequestFee;
                }
            else if (Flight is DDJBFlight ddjbFlight)
                {
                specialRequestFee = ddjbFlight.RequestFee;
                }

            return baseFee + specialRequestFee;
            }


        public override string ToString()
            {
            return $"Gate: {GateName}, DDJB: {SupportsDDJB}, CFFT: {SupportsCFFT}, LWTT: {SupportsLWTT}";
            }
        }
    }