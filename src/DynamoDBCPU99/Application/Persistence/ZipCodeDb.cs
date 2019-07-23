using Amazon.DynamoDBv2;
using clearwaterstream.AWS.Db;
using DynamoDBCPU99.Application.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBCPU99.Application.Persistence
{
    public class ZipCodeDb : ZipCodeDataStore, IDisposable
    {
        static AmazonDynamoDBClient dbClient;

        static readonly Lazy<AmazonDynamoDBClient> dbClientFactory = new Lazy<AmazonDynamoDBClient>(() =>
        {
            dbClient = DynamoDBClientFactory.CreateClient();

            return dbClient;
        });

        public async Task Seed()
        {
            throw new NotImplementedException();
        }

        public async Task Lookup()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            dbClient?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
