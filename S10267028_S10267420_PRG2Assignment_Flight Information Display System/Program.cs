using System;
using System.IO;

class Program
    {
    static void Main()
        {
        string filePath = "airlines.csv";
        if (File.Exists(filePath))
            {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
                {
                Console.WriteLine(line);
                }
            }
        else
            {
            Console.WriteLine("File not found.");
            }
        }
    }
