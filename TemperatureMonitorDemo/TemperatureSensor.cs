//https://github.com/valerichikovani19-gif/lilprojects.git
namespace TemperatureMonitor
{
    public class TemperatureSensor
    {
        public string Id { get; }
        public double CurrentTemperature { get; private set; }

        public event TemperatureAlertHandler OnCriticalTemperature;
        public TemperatureSensor(string id, double initialTemperature)
        {
            Id = id;
            CurrentTemperature = initialTemperature;
        }
        public TemperatureReading UpdateTemperature(double newValue)
        {
            CurrentTemperature = newValue;
            var reading = new TemperatureReading(Id, newValue, DateTime.Now);
            if (newValue > 70)
            {
                RaiseCriticalAlert(reading, "Temperature is too HIGH!");

            }
            else if (newValue < 0)
            {
                RaiseCriticalAlert(reading, "Temperature is too LOW!");
            }
            return reading;
        }

        private void RaiseCriticalAlert(TemperatureReading reading, string message)
        {
            if (OnCriticalTemperature != null)
            {
                OnCriticalTemperature(reading, message);
            }
        }
    }

}
