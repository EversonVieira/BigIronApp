using CsvHelper.Configuration.Attributes;

namespace Core.DTOs
{
    public class ISRDTO
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
    }
}
