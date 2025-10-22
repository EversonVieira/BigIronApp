using CsvHelper.Configuration.Attributes;

namespace Core.DTOs
{
    public class ISRDTO
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }

        [Format("0.000000")]
        public decimal? Latitude { get; init; }
        [Format("0.000000")]
        public decimal? Longitude { get; init; }
    }
}
