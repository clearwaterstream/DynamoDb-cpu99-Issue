using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using clearwaterstream.Configuration;
using clearwaterstream.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.AWS
{
    public static class CredentialsHelper
    {
        static readonly AWSCredentials _creds = null;

        static CredentialsHelper()
        {
            if (AppEnvironment.IsDevelopment())
            {
                _creds = GetCredsForLocalDevelopment();
            }
            else
            {
                _creds = new InstanceProfileAWSCredentials();
            }
        }

        public static AWSCredentials Credentials => _creds;

        static AWSCredentials GetCredsForLocalDevelopment()
        {
            var profileName = ServiceRegistrar.Configuration["aws_profile"];

            var osUsername = ServiceRegistrar.Configuration["my_os_username"];

            if (string.IsNullOrEmpty(osUsername))
            {
                // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows
                // from csproj dir, run dotnet user-secrets set "my_os_username" "username_value"
                throw new Exception("please set my_os_username in secrets.json");
            }

            // this is a workaround for IIS running under Windows

            var credsFileLoc = $@"C:\Users\{osUsername}\.aws\credentials";

            SharedCredentialsFile sharedFile = null;

            try
            {
                sharedFile = new SharedCredentialsFile(credsFileLoc);
            }
            catch (Exception ex)
            {
                throw new Exception($"make sure {credsFileLoc} exists and your IIS app pool or IIS Express has read access to the file", ex);

                // find out the app pool under which the web app is running, find the file, add read permission to it
                // for example, if the app is running under DefaultAppPool, add this permission: IIS AppPool\DefaultAppPool
            }

            if (!sharedFile.TryGetProfile(profileName, out CredentialProfile credsProfile))
                throw new Exception("aws profile is not set");

            var creds = credsProfile.GetAWSCredentials(null);

            return creds;
        }
    }
}
