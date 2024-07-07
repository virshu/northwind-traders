using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Northwind.Application.Common.Interfaces;
using Northwind.Application.Products.Queries.GetProductsFile;

namespace Northwind.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildProductsFile(IEnumerable<ProductRecordDto> records)
    {
        using MemoryStream memoryStream = new();
        using (StreamWriter streamWriter = new(memoryStream))
        {
            using CsvWriter csvWriter = new(streamWriter, System.Globalization.CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}