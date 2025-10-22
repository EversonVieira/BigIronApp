using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ISR
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string? Name { get; init; }
        public string? Description { get; init; }
        public required GeoLocation Location { get; init; }
    }
}
