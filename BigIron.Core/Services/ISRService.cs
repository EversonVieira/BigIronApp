using Core.DTOs;
using Core.Enums;
using Core.Mappings;
using Core.Models;
using Core.Processors;
using Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ISRService(ISRListProcessor processor) : IISRService
    {
        public Response<List<ISR>> ProcessData(List<ISRDTO> source)
        {
            var msgs = new List<Message>();

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

            return new Response<List<ISR>>()
            {
                Result = processor.GetOrdered(),
                Messages = msgs,
            };
        }
    }
}
