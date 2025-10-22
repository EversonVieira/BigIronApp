using Core.Models;
using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Processors
{
    public class ISRListProcessor : IISRListProcessor
    {
        private Dictionary<Guid, ISR> _map = [];
        private List<DistanceMapItem> _distanceMapItems = [];
        private List<DistanceMapItem> _orderedMapItems = [];
        private List<Guid> _addedLocationIds = [];
        public void AddItem(ISR obj)
        {
            foreach (var item in _map)
            {

                _distanceMapItems.Add(new DistanceMapItem
                {
                    AId = obj.Id,
                    BId = item.Value.Id,
                    Distance = obj.Location.Distance(item.Value.Location)
                });

                // Adds reversed
                _distanceMapItems.Add(new DistanceMapItem
                {
                    AId = item.Value.Id,
                    BId = obj.Id,
                    Distance = item.Value.Location.Distance(obj.Location)
                });
            }
            _map[obj.Id] = obj;
        }

        public List<ISRWithDistance> GetOrderByMyLocation(GeoLocation location)
        {
            var result = new List<ISRWithDistance>();

            if (_distanceMapItems.Count == 0) return result;

            // Add host location and calculate the distance beforehand with every other option
            foreach (var item in _map)
            {
                _distanceMapItems.Add(new DistanceMapItem()
                {
                    AId = Guid.Empty,
                    BId = item.Key,
                    Distance = location.Distance(item.Value.Location)
                });
            }
            _map[Guid.Empty] = new ISR
            {
                Id = Guid.Empty,
                Name = "Home",
                Location = location
            };

            var addedLocationsIds = new List<Guid>();

            // First, we order by Distance, ensuring that the Distance will be the decider for the nearest position
            // With this approach, we're basicly defining a sequential structure that we always look the closest one (one by one). 
            // We're not considerating every possibility, there's a 100% of chance that this is not the better approach, but for this example it's fine enough.
            _orderedMapItems = _distanceMapItems.OrderBy(x => x.Distance).ToList();
            var firstItem = _orderedMapItems.First(x => x.AId == Guid.Empty);

            AddCascadingNearestItem(result, firstItem);

            return result;
        }

        void AddCascadingNearestItem(List<ISRWithDistance> list, DistanceMapItem item)
        {
            var currentItem = _map[item.AId];
            _addedLocationIds.Add(currentItem.Id);
            list.Add(new ISRWithDistance
            {
                Id = currentItem.Id,
                Name = currentItem.Name,
                Description = currentItem.Description,
                Location = currentItem.Location,
                Distance = item.Distance,
            });

            var nextItem = _orderedMapItems.FirstOrDefault(x => x.AId == item.BId && !_addedLocationIds.Contains(x.BId) && x.Distance != 0);
            if (nextItem is not null)
            {
                AddCascadingNearestItem(list, nextItem);
            }
        }

    }
}
