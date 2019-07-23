using Amazon.DynamoDBv2;
using clearwaterstream.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.AWS.Db
{
    public static class DynamoDBClientFactory
    {
        public static AmazonDynamoDBClient CreateClient()
        {
            return CreateClient(AppEnvironment.IsLambda());
        }

        public static AmazonDynamoDBClient CreateClient(bool isLambda)
        {
            AmazonDynamoDBClient client;

            if (isLambda && !AppEnvironment.IsDevelopment())
            {
                client = new AmazonDynamoDBClient();
            }
            else
            {
                client = new AmazonDynamoDBClient(CredentialsHelper.Credentials, RegionConfig.CurrentRegion);
            }

            return client;
        }
    }
}
