namespace BigIron.Api.DTOs
{
    public class ISRProcessDataRequest
    {
        public required IFormFile File { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set;  }
    }
}
