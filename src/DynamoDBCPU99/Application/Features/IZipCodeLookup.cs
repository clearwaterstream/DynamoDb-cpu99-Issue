using DynamoDBCPU99.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Application.Features
{
    public interface IZipCodeLookup
    {
        Task<ICollection<ZipCode>> Search();
    }
}
