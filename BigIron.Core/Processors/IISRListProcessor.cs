using Core.Models;

namespace Core.Processors
{
    public interface IISRListProcessor
    {
        IReadOnlyList<ISR> ISRList { get; }

        void AddItem(ISR obj);
        List<ISR> GetOrdered();
    }
}