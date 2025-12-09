//https://github.com/valerichikovani19-gif/lilprojects.git
using System.Linq.Expressions;
using System.Reflection;

namespace TemperatureMonitor
{
    public class TemperatureMonitor
    {
        private readonly List<TemperatureSensor> _sensors = new List<TemperatureSensor>();
        private readonly List<TemperatureReading> _allReadings = new List<TemperatureReading>();

        //store alerts
        private readonly List<string> _alerts = new List<string>();

        //protect shared collections
        private readonly object _lock = new object();

        //control monitoring loop
        private bool _isMonitoring = false;

        public void AddSensor(TemperatureSensor sensor)
        {
            //subscribe to the sensor's critical temperature
            sensor.OnCriticalTemperature += HandleCriticalTemperature;
            _sensors.Add(sensor);
        }

        private void HandleCriticalTemperature(TemperatureReading reading, string message)
        {
            string fullMessage = $"ALERT from {reading.SensorId} at {reading.Timestamp:HH:mm:ss}: {reading.Value} °C -> {message}";
            lock (_lock)
            {
                _alerts.Add(fullMessage);
            }
            Console.WriteLine(fullMessage);
        }

        public void RecordReading(TemperatureReading reading)
        {
            lock (_lock)
            {
                _allReadings.Add(reading);
            }
        }
        public void PrintSensorsState()
        {
            Console.WriteLine("=== Sensors State ===");
            foreach (var sensor in _sensors)
            {
                Console.WriteLine($"Sensor {sensor.Id} - > {sensor.CurrentTemperature} °C");
            }

        }
        public void PrintAlerts()
        {
            Console.WriteLine("=== Stored Alerts ===");
            if (_alerts.Count == 0)
            {
                Console.WriteLine("No alerts yet.");
            }
            else
            {
                foreach (var alert in _alerts)
                {
                    Console.WriteLine(alert);
                }
            }
        }
        public void StartMonitoring()
        {
            if (_isMonitoring) return;
            _isMonitoring = true;
            foreach (var sensor in _sensors)
            {
                var thread = new Thread(() => SensorLoop(sensor));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void SensorLoop(TemperatureSensor sensor)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            while (_isMonitoring)
            {
                double newTemp = random.NextDouble() * 110.0 - 10.0;

                var reading = sensor.UpdateTemperature(newTemp);
                RecordReading(reading);

                int delay = random.Next(500, 1501);
                Thread.Sleep(delay);
            }
        }

        public void StopMonitoring()
        {
            _isMonitoring = false;
        }
        public Dictionary<string, double> GetAvererageTemperaturePerSensor()
        {
            lock (_lock)
            {
                return _allReadings
                    .GroupBy(r => r.SensorId)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Average(r => r.Value)
                    );
            }
        }
        public List<TemperatureReading> GetCriticalReadings()
        {
            lock (_lock)
            {
                var result = _allReadings
                    .Where(r => r.Value > 70 || r.Value < 0)
                    .OrderByDescending(r => r.Value)
                    .ToList();
                return result;
            }
        }
        public List<TemperatureReading> GetTopHottestReadings(int count)
        {
            lock (_lock)
            {
                return _allReadings
            .OrderByDescending(r => r.Value)
            .Take(count)
            .ToList();
            }
        }
        public List<TemperatureReading> GetCriticalReadingsQuerySyntax()
        {
            lock (_lock)
            {
                var query =
                    from r in _allReadings
                    where r.Value > 70 || r.Value < 0
                    orderby r.Value descending
                    select r;
                return query.ToList();
            }
        }
        public async Task SaveAlertAsync(string path)
        {
            List<string> alertsSnapshot;
            lock (_lock)
            {
                alertsSnapshot = new List<string>(_alerts);
            }
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (var writer = new StreamWriter(path, false))
            {
                foreach (var alert in alertsSnapshot)
                {
                    await writer.WriteLineAsync(alert);
                }
            }
            Console.WriteLine($"Alerts saved to file: {path}");
        }
        public void PrintPublicMethodsInfo()
        {
            Type type = typeof(TemperatureMonitor);
            Console.WriteLine("=== public methods of TemperatureMonitor ===");
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                string methodName = method.Name;
                ParameterInfo[] parameters = method.GetParameters();
                string returnTypeName = method.ReturnType.Name;
                Console.Write($"{returnTypeName} {methodName} (");

                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    string paramTypeName = p.ParameterType.Name;
                    string paramName = p.Name;
                    Console.Write($"{paramTypeName} {paramName}");

                    if (i < parameters.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write(")");
            }
            Console.WriteLine("==============================================");
        }

        public void InvokeParameterlessMethodByName(string methodName)
        {
            Type type = typeof(TemperatureMonitor);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            MethodInfo method = type.GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
                null,
                Type.EmptyTypes,
                null
            );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (method == null)
            {
                Console.WriteLine($"No public parameterless method named '{methodName}' was found.");
                return;
            }
            object result = method.Invoke(this, null);

            if (method.ReturnType != typeof(void))
            {
                Console.WriteLine($"Method '{methodName}' returned: {result}");
            }
            else
            {
                Console.WriteLine($"Method '{methodName}' invoked successfully.");
            }
        }
        public List<TemperatureReading> GetReadingsAboveThresholdWithExpression(double threshold)
        {
            ParameterExpression param = Expression.Parameter(typeof(TemperatureReading), "r");
            MemberExpression valueProperty = Expression.Property(param, nameof(TemperatureReading.Value));
            ConstantExpression thresholdConst = Expression.Constant(threshold, typeof(double));
            BinaryExpression body = Expression.GreaterThan(valueProperty, thresholdConst);
            Expression<Func<TemperatureReading, bool>> lambda =
                Expression.Lambda<Func<TemperatureReading, bool>>(body, param);
            Func<TemperatureReading, bool> predicate = lambda.Compile();
            lock (_lock)
            {
                return _allReadings
                    .Where(predicate)
                    .OrderByDescending(r => r.Value)
                    .ToList();
            }
        }


    }

}
