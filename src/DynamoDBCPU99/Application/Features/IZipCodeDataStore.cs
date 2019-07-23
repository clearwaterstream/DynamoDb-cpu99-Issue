using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Application.Features
{
    public interface ZipCodeDataStore
    {
        Task Seed();
        Task Lookup();
    }
}
