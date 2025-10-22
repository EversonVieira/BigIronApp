using Core.Models;
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
        private List<DistanceMapItem> distanceMapItems = [];
        public IReadOnlyList<ISR> ISRList => GetOrdered();

        public void AddItem(ISR obj)
        {
            foreach (var item in _map)
            {
                distanceMapItems.Add(new DistanceMapItem
                {
                    AId = obj.Id,
                    BId = item.Value.Id,
                    Distance = obj.Location.Distance(item.Value.Location)
                });
            }
            _map[obj.Id] = obj;
        }

        public List<ISR> GetOrdered()
        {
            var result = new List<ISR>();
            var orderList = distanceMapItems.OrderBy(x => x.Distance).ThenBy(x => x.AId);

            foreach (var item in orderList)
            {
                result.Add(_map[item.AId]);
            }

            return result;
        }



    }
}
