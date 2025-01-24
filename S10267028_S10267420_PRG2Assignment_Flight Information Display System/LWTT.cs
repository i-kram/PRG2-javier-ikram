using System;

public class LWTT
    {
    public double RequestFee { get; set; }

    public double CalculateFee()
        {
        return RequestFee;
        }

    public override string ToString()
        {
        return $"LWTT Request Fee: {RequestFee}";
        }
    }