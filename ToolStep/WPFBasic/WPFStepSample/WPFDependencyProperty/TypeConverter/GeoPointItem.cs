using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStepSample.TypeConverter
{
    [System.ComponentModel.TypeConverter(typeof(GeoPointConverter))]
    public class GeoPointItem
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoPointItem()
        {
        }

        public GeoPointItem(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
        public static GeoPointItem Parse(string data)
        {
            if (string.IsNullOrEmpty(data)) return new GeoPointItem();

            string[] items = data.Split(',');
            if (items.Count() != 2)
                throw new FormatException("GeoPoint should have both latitude and longitude");

            double lat, lon;
            try
            {
                lat = Convert.ToDouble(items[0]);
            }
            catch (Exception ex)
            {
                throw new FormatException("Latitude value cannot be converted", ex);
            }

            try
            {
                lon = Convert.ToDouble(items[1]);
            }
            catch (Exception ex)
            {
                throw new FormatException("Longitude value cannot be converted", ex);
            }

            return new GeoPointItem(lat, lon);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.Latitude, this.Longitude);
        }
    }
}
