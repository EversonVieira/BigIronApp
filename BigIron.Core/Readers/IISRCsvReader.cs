using Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace Core.Adapter
{
    public interface IISRCsvReader
    {
        List<ISRDTO> ReadFile(IFormFile file);
    }
}