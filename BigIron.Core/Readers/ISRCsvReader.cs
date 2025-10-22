using Core.DTOs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapter
{
    public class ISRCsvReader : IISRCsvReader
    {
        public List<ISRDTO> ReadFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                throw new Exception("File is empty or missing.");
            }

            if (file.ContentType != "text/csv")
            {
                throw new Exception("Invalid file type");
            }

            using (var stream = file.OpenReadStream())
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
}
