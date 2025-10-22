using Core.DTOs;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapter
{
    public class ISRAdapter : IISRAdapter
    {
        public List<ISRDTO> ReadFile(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    Encoding = Encoding.UTF8,

                };

                using (var reader = new CsvReader(sr, config))
                {
                    return reader.GetRecords<ISRDTO>().ToList();
                }
            }

        }
    }
}
