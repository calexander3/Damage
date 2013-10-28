using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    internal class WeatherOptions
    {
        public int? ZipCode{get; set;}
        public int? Latitude { get; set; }
        public int? Longitude { get; set; }
        public bool USUnits { get; set; }
        public bool UKUnits { get; set; }
    }
}
