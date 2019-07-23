using Amazon.SimpleSystemsManagement;
using clearwaterstream.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.AWS.Security
{
    public static class SSMClientFactory
    {
        public static AmazonSimpleSystemsManagementClient CreateClient()
        {
            return CreateClient(AppEnvironment.IsLambda());
        }

        public static AmazonSimpleSystemsManagementClient CreateClient(bool isLambda)
        {
            AmazonSimpleSystemsManagementClient client = null;

            if (isLambda && !AppEnvironment.IsDevelopment())
            {
                client = new AmazonSimpleSystemsManagementClient();
            }
            else
            {
                client = new AmazonSimpleSystemsManagementClient(CredentialsHelper.Credentials, RegionConfig.CurrentRegion);
            }

            return client;
        }
    }
}
