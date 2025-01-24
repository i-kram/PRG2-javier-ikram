using System;

public class DDJB
    {
    public double RequestFee { get; set; }

    public double CalculateFee()
        {
        return RequestFee;
        }

    public override string ToString()
        {
        return $"DDJB Request Fee: {RequestFee}";
        }
    }