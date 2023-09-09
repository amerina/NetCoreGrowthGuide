using System;

namespace RedisWebAPI
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public override string ToString()
        {
            return "Date:" + Date.ToString("MM/dd/yyyy") + ",TemperatureC:" + TemperatureC + ",Summary:" + Summary;
        }
    }
}
