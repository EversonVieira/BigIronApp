using Core.DTOs;
using Core.Models;
using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappings
{
    public static class ISRMap
    {
        public static ISRDTO ToDto(this ISR value)
        {
            return new ISRDTO
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                Latitude = value.Location?.Latitude,
                Longitude = value.Location?.Longitude,
            };
        }

        public static ISR ToModel(this ISRDTO value)
        {
            return new ISR
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                Location = new GeoLocation(value.Latitude, value.Longitude)
            };
        }
    }
}
