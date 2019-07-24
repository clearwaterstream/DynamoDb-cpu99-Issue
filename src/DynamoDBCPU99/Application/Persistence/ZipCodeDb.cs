using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
    public class ZipCodeDb : ZipCodeDataStore, IZipCodeLookup, IDisposable
    {
        static AmazonDynamoDBClient dbClient;

        readonly Random rand = new Random();

        static readonly Lazy<AmazonDynamoDBClient> dbClientFactory = new Lazy<AmazonDynamoDBClient>(() =>
        {
            dbClient = DynamoDBClientFactory.CreateClient();

            return dbClient;
        });

        public async Task Seed()
        {
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
                        Code = rand.Next(10).ToString("D5"), // we'll need them to repeat
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

        public async Task<ICollection<ZipCode>> Search()
        {
            using (var dynamoDbContext = new DynamoDBContext(dbClientFactory.Value, new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 }))
            {
                var zipCodeTable = dynamoDbContext.GetTargetTable<ZipCode>();

                var randZip = rand.Next(10).ToString("D5");

                var keyExpr = new Expression();
                keyExpr.ExpressionStatement = "Code = :v_code";
                keyExpr.ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    [":v_code"] = new Primitive(randZip)
                };

                var pageSize = 15;

                var query = zipCodeTable.Query(new QueryOperationConfig()
                {
                    IndexName = "Code-index",
                    KeyExpression = keyExpr,
                    PaginationToken = "{}",
                    Limit = pageSize
                });

                var bucket = new List<ZipCode>(pageSize); // 15 is our bucket or page size. After we fill it, we are good...

                var i = 0;
                do
                {
                    i++;

                    var docs = await query.GetNextSetAsync();

                    IEnumerable<ZipCode> zipCodes = dynamoDbContext.FromDocuments<ZipCode>(docs);

                    bucket.AddRange(zipCodes);

                    if (query.PaginationToken == "{}")
                    {
                        break; // BREAK!!! there are no more records
                    }

                } while (!query.IsDone || bucket.Count >= pageSize);

                return bucket;
            }
        }

        public void Dispose()
        {
            dbClient?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
