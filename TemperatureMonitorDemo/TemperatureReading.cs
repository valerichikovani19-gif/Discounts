//https://github.com/valerichikovani19-gif/lilprojects.git
namespace TemperatureMonitor
{
    public class TemperatureReading
    {
        public string SensorId { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }

        public TemperatureReading(string sensorId, double value, DateTime timeStamp)
        {
            SensorId = sensorId;
            Value = value;
            Timestamp = timeStamp;
        }
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] Sensor {SensorId}: {Value} °C";
        }

    }

}
