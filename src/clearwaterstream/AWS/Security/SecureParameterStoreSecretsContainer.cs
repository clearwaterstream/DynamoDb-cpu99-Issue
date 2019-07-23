using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using clearwaterstream.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace clearwaterstream.AWS.Security
{
    public class SecureParameterStoreSecretsContainer : ISecretsContainer, IDisposable
    {
        readonly AmazonSimpleSystemsManagementClient _client = null;

        public SecureParameterStoreSecretsContainer()
        {
            _client = SSMClientFactory.CreateClient();
        }

        public SecureParameterStoreSecretsContainer(bool isLambda)
        {
            _client = SSMClientFactory.CreateClient(isLambda);
        }

        public async Task<string> WhisperAsync(string secretId)
        {
            string result = null;

            var req = new GetParameterRequest()
            {
                Name = secretId,
                WithDecryption = true
            };

            var response = await _client.GetParameterAsync(req);

            if (response.Parameter != null)
            {
                result = response.Parameter.Value;
            }
            else
            {
                throw new Exception($"{secretId} does not exist");
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
        }
    }
}
