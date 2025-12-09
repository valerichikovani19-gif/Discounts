//https://github.com/valerichikovani19-gif/lilprojects.git
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace TemperatureMonitor
{

    public delegate void TemperatureAlertHandler(TemperatureReading reading, string message);
    public class Program
    {
        static void Main(string[] args)
        {
            var monitor = new TemperatureMonitor();
            var sensor1 = new TemperatureSensor("S1", 22.5);
            var sensor2 = new TemperatureSensor("S2", 18.0);
            var sensor3 = new TemperatureSensor("S3", 19.0);

            monitor.AddSensor(sensor1);
            monitor.AddSensor(sensor2);
            monitor.AddSensor(sensor3);

            Console.WriteLine("Starting monitoring for 10 seconds");
            monitor.StartMonitoring();

            Thread.Sleep(15000);

            monitor.StopMonitoring();

            Console.WriteLine();
            monitor.PrintSensorsState();

            Console.WriteLine();
            monitor.PrintAlerts();
            Console.WriteLine();
            monitor.SaveAlertAsync("logs/async.txt").GetAwaiter().GetResult();

            var averages = monitor.GetAvererageTemperaturePerSensor();
            foreach (var pair in averages)
            {
                Console.WriteLine($"Sensor {pair.Key} -> avg {pair.Value:F2} °C"); ;
            }
            Console.WriteLine();

            Console.WriteLine("=== Top 3 hottest readings ===");
            var top3 = monitor.GetTopHottestReadings(3);
            foreach (var r in top3)
            {
                Console.WriteLine(r);
            }
            Console.WriteLine();
            Console.WriteLine("=== Critical Readings (LINQ) ===");
            var critical = monitor.GetCriticalReadings();
            foreach (var r in critical)
            {
                Console.WriteLine(r);
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("=== Expression tree filter: readings above custom threshold ===");
            Console.WriteLine("Enter threshold (e.g. 50):");

            string input = Console.ReadLine();
            if (double.TryParse(input, out double threshold))
            {
                var exprReadings = monitor.GetReadingsAboveThresholdWithExpression(threshold);
                foreach (var r in exprReadings)
                {
                    Console.WriteLine(r);
                }
            }
            else
            {
                Console.WriteLine("Invalid number, skipping expression filter demo.");
            }

            Console.WriteLine("Reflection Demo: ");
            monitor.PrintPublicMethodsInfo();
            Console.WriteLine("Enter a public parameterless method name of TemperatureMonitor to invoke (or just Enter to skip):");
            string methodName = Console.ReadLine();

            if (!string.IsNullOrEmpty(methodName))
            {
                monitor.InvokeParameterlessMethodByName(methodName);
            }

        }
    }

}
