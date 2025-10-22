using Core.DTOs;
using Core.Models;
using Core.Wrappers;

namespace Core.Services
{
    public interface IISRService
    {
        Response<List<ISR>> ProcessData(List<ISRDTO> source);
    }
}