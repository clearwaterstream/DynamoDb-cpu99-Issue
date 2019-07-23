using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text;

namespace clearwaterstream.Util
{
    public static class JsonUtil
    {
        public static readonly string ContentType;

        static JsonUtil()
        {
            var hdr = MediaTypeHeaderValue.Parse("application/json");
            hdr.CharSet = "utf-8";

            ContentType = hdr.ToString();
        }

        public static readonly JsonSerializerSettings FullSerializerSettings_withFormatting = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.Indented
        };

        public static readonly JsonSerializerSettings LeanSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings LeanSerializerSettings_withFormatting = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings LeanSerializerSettings_withDefaults = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings LeanSerializerSettings_withDefaultsAndNulls = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static readonly JsonSerializerSettings LeanSerializerSettings_withTypeInfo = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All
        };

        public static readonly JsonSerializerSettings NiceSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static T Deserialize<T>(Stream stream)
        {
            return (T)Deserialize(stream, typeof(T), null);
        }

        public static T Deserialize<T>(Stream stream, JsonSerializerSettings settings)
        {
            return (T)Deserialize(stream, typeof(T), settings);
        }

        public static object Deserialize(Stream stream, Type objectType)
        {
            return Deserialize(stream, objectType, null);
        }

        // cpu intensive, do NOT needlessly async this!
        public static object Deserialize(Stream stream, Type objectType, JsonSerializerSettings settings)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var sr = new StreamReader(stream))
            {
                using (var tr = new JsonTextReader(sr))
                {
                    var ser = JsonSerializer.Create(settings);

                    return ser.Deserialize(tr, objectType);
                }
            }
        }

        public static void Serialize(Stream stream, object value)
        {
            Serialize(stream, value, LeanSerializerSettings);
        }

        public static void Serialize(Stream stream, object value, JsonSerializerSettings settings)
        {
            if (value == null)
                return; // if the value is null, nothing to do here

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var sw = new StreamWriter(stream))
            {
                using (var tw = new JsonTextWriter(sw))
                {
                    var ser = JsonSerializer.Create(settings);

                    ser.Serialize(tw, value);
                }
            }
        }

        /// <returns>gzipped and base64 encoded string</returns>
        public static string SerializeToGZippedString(object value, JsonSerializerSettings settings)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (settings == null)
                settings = LeanSerializerSettings;

            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    using (var sw = new StreamWriter(gzip))
                    {
                        using (var tw = new JsonTextWriter(sw))
                        {
                            var ser = JsonSerializer.Create(settings);

                            ser.Serialize(tw, value);
                        }
                    }
                }

                ms.Position = 0;

                var result = WebEncoders.Base64UrlEncode(ms.GetBuffer(), 0, (int)ms.Length);

                return result;
            }
        }

        public static object DeserializeFromGZippedString(string gzippedString, Type objectType)
        {
            var buffer = WebEncoders.Base64UrlDecode(gzippedString);

            using (var ms = new MemoryStream(buffer))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    var result = Deserialize(gzip, objectType);

                    return result;
                }
            }
        }

        public static T DeserializeFromGZippedString<T>(string gzippedString)
        {
            return (T)DeserializeFromGZippedString(gzippedString, typeof(T));
        }
    }
}
