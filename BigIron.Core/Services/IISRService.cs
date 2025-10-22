using BigIron.Api.DTOs;
using Core.DTOs;
using Core.Models;
using Core.ValueObjects;
using Core.Wrappers;

namespace Core.Services
{
    public interface IISRService
    {
        Response<List<ISRWithDistance>> GetVisitationRoute(ISRProcessDataRequest request);
    }
}