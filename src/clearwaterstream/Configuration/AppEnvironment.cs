using System;
using System.Collections.Generic;
using System.Text;

namespace clearwaterstream.Configuration
{
    public static class AppEnvironment
    {
        static string _name = null;

        public static void SetName(string name)
        {
            _name = name;
        }

        public static bool IsDevelopment()
        {
            return _name.Eq(Development);
        }

        public static bool IsLambda()
        {
            // https://docs.aws.amazon.com/lambda/latest/dg/lambda-environment-variables.html

            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"));
        }

        public static bool IsQA()
        {
            return _name.Eq(QA);
        }

        public static bool IsStaging()
        {
            return _name.Eq(Staging);
        }

        public static bool IsProduction()
        {
            return _name.Eq(Production);
        }

        public static bool Is(string name)
        {
            return _name.Eq(name);
        }

        public static readonly string Development = new Inferred();
        public static readonly string QA = new Inferred();
        public static readonly string Staging = new Inferred();
        public static readonly string Production = new Inferred();


        public static string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    throw new Exception("application environment is not set");

                return _name;
            }
        }

        // get name w/o throwing execption
        public static string TryGetName()
        {
            var result = _name;

            if (string.IsNullOrEmpty(_name))
                result = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return result;
        }
    }
}
