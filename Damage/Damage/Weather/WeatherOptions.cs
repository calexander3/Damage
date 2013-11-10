
namespace Weather
{
    internal class WeatherOptions
    {
        public string CityName{get; set;}
        public int? Latitude { get; set; }
        public int? Longitude { get; set; }
        public bool USUnits { get; set; }
        public bool UKUnits { get; set; }
    }
}
