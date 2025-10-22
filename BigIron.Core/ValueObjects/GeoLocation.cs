using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public class GeoLocation
    {
        public double Latitude {  get; init; }
        public double Longitude { get; init; }

        public GeoLocation(double? latitude, double? longitude)
        {
            if (!latitude.HasValue) throw new ArgumentNullException(nameof(latitude));
            if (!longitude.HasValue) throw new ArgumentNullException($"{nameof(longitude)}");

            Latitude = latitude.Value;
            Longitude = longitude.Value;
        }

        
        // Since this is an example code, I'll be just using a distance between 2 points calculations ignoring other variables.
        public double Distance(GeoLocation target)
        {
            var xab = this.Latitude - target.Latitude;
            var yab = this.Longitude - target.Longitude;

            xab = Math.Pow(xab, 2);
            yab = Math.Pow(yab, 2);

            return Math.Sqrt(xab + yab);
        }
    }
}
