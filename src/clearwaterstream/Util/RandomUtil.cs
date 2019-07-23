using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace clearwaterstream.Util
{
    // https://www.dotnetperls.com/random-string
    public static class RandomUtil
    {
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();

            path = path.Replace(".", ""); // Remove period.

            return path;
        }
    }
}
