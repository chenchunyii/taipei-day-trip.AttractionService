namespace taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse
{
    public class AttractionsResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Transport { get; set; } = null!;
        public string? MRT { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IList<string>? Images { get; set; }
    }
}