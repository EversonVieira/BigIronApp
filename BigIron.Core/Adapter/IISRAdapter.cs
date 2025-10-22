using Core.DTOs;

namespace Core.Adapter
{
    public interface IISRAdapter
    {
        List<ISRDTO> ReadFile(Stream stream);
    }
}