using Amazon;
using clearwaterstream.IoC;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.AWS
{
    public class RegionConfig
    {
        static readonly RegionEndpoint _currentRegion = null;

        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static RegionConfig()
        {
            RegionEndpoint ec2Region = null;

            try
            {
                ec2Region = Amazon.Util.EC2InstanceMetadata.Region;
            }
            catch
            {
                // do nothing
            }

            // will be null for non-ec2 code
            if (ec2Region != null)
            {
                _currentRegion = ec2Region;

                return;
            }

            var regionName = ServiceRegistrar.Configuration["AWS_REGION"];

            if (string.IsNullOrEmpty(regionName))
                regionName = "us-east-1";

            logger.Info($"using {regionName} region");

            _currentRegion = RegionEndpoint.GetBySystemName(regionName);
        }

        public static RegionEndpoint CurrentRegion => _currentRegion;
    }
}
