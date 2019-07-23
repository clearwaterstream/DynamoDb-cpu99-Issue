using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace clearwaterstream.Security
{
    public class CryptoRandomGenerator : IDisposable
    {
        public static readonly CryptoRandomGenerator Instance = new CryptoRandomGenerator();

        readonly RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        private CryptoRandomGenerator() { }

        public string GenerateKey()
        {
            // guids/uuids are 16 bytes. We'll go for 18. Living large.
            return GenerateKey(18);
        }

        public string GenerateKey(int keyLength)
        {
            var rndBytes = new byte[keyLength];

            rngCryptoServiceProvider.GetBytes(rndBytes);

            return WebEncoders.Base64UrlEncode(rndBytes);
        }

        public byte[] GenerateBytes(int numOfBytes)
        {
            var rndBytes = new byte[numOfBytes];

            rngCryptoServiceProvider.GetBytes(rndBytes);

            return rndBytes;
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
                rngCryptoServiceProvider?.Dispose();
            }
        }
    }
}
