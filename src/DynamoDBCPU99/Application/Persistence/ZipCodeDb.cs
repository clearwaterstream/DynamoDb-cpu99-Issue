using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using clearwaterstream.AWS.Db;
using clearwaterstream.Security;
using clearwaterstream.Util;
using DynamoDBCPU99.Application.Features;
using DynamoDBCPU99.Application.Model;
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
            var rand = new Random();

            var batch = new List<ZipCode>();

            var numRecords = 500;

            using (var dynamoDbContext = new DynamoDBContext(dbClientFactory.Value, new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 }))
            {
                for (int i = 0; i < numRecords; i++)
                {
                    var z = new ZipCode()
                    {
                        Id = IdGenerator.NewId(),
                        City = RandomUtil.GetRandomString(),
                        State = RandomUtil.GetRandomString(),
                        Code = rand.Next(99999).ToString("D5"),
                        Latitude = rand.NextDouble() * 90,
                        Longitude = rand.NextDouble() * 180
                    };

                    batch.Add(z);

                    if (i % 25 == 0 || i == numRecords - 1)
                        await FlushBatch(dynamoDbContext, batch);
                }
            }
        }

        async Task FlushBatch(DynamoDBContext dynamoDbContext, IEnumerable<ZipCode> batch)
        {
            var batchWrite = dynamoDbContext.CreateBatchWrite<ZipCode>();

            batchWrite.AddPutItems(batch);

            await batchWrite.ExecuteAsync();
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
