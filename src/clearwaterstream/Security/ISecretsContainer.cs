using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace clearwaterstream.Security
{
    public interface ISecretsContainer
    {
        Task<string> WhisperAsync(string secretId);
    }
}
