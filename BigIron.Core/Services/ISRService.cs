using BigIron.Api.DTOs;
using Core.Adapter;
using Core.DTOs;
using Core.Enums;
using Core.Mappings;
using Core.Models;
using Core.Processors;
using Core.ValueObjects;
using Core.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ISRService(IISRCsvReader csvReader, IISRListProcessor processor) : IISRService
    {
        public Response<List<ISRWithDistance>> GetVisitationRoute(ISRProcessDataRequest request)
        {
            var msgs = new List<Message>();

            var source = csvReader.ReadFile(request.File);

            // Phase 1: Validate the input using ValueObjects within Model
            for (int i = 0; i < source.Count; i++)
            {
                try
                {
                    processor.AddItem(source[i].ToModel());
                }
                catch (Exception ex)
                {
                    msgs.Add(new Message()
                    {
                        Text = $"Item with index: {i} is invalid: {ex.Message}",
                        Type = MessageType.VALIDATION,
                    });
                }
            }

            return new Response<List<ISRWithDistance>>()
            {
                Result = processor.GetVisitOrderByMyLocation(new GeoLocation(request.Latitude, request.Longitude)),
                Messages = msgs,
            };
        }
    }
}
