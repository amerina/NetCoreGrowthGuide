using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStepSample.TypeConverter
{
    public class GeoPointConverter : System.ComponentModel.TypeConverter
    {

        //should return true if sourcetype is string
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType is string)
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        //should return true when destinationtype if GeopointItem
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType is string)
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        //Actual convertion from string to GeoPointItem
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return GeoPointItem.Parse(value as string);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Cannot convert '{0}' ({1}) because {2}", value, value.GetType(), ex.Message), ex);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        //Actual convertion from GeoPointItem to string
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context,System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            GeoPointItem gpoint = value as GeoPointItem;

            if (gpoint != null)
                if (this.CanConvertTo(context, destinationType))
                    return gpoint.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
