using Core.Models;
using Core.ValueObjects;

namespace Core.Processors
{
    public interface IISRListProcessor
    {

        void AddItem(ISR obj);
        List<ISRWithDistance> GetOrderByMyLocation(GeoLocation location);
    }
}