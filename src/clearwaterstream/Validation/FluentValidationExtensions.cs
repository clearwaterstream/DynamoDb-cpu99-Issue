using Autofac.Builder;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace clearwaterstream.Validation
{
    public static class FluentValidationExtensions
    {
        public static ValidationResult MergeWith(this ValidationResult validationResult, ValidationResult other)
        {
            if (other?.Errors == null || validationResult?.Errors == null)
                return validationResult;

            foreach (var item in other.Errors)
                validationResult.Errors.Add(item);

            return validationResult;
        }

        public static bool HasErrors(this ValidationResult validationResult)
        {
            var result = HasValidationFailuresBySeverity(validationResult, Severity.Error);

            return result;
        }

        public static bool HasWarnings(this ValidationResult validationResult)
        {
            var result = HasValidationFailuresBySeverity(validationResult, Severity.Warning);

            return result;
        }

        static bool HasValidationFailuresBySeverity(ValidationResult validationResult, Severity severity)
        {
            if (validationResult == null || validationResult.Errors == null)
                return false;

            var hasAny = validationResult.Errors.Any(x => x != null && x.Severity == severity);

            return hasAny;
        }

        public static IEnumerable<ValidationFailure> GetErrors(this ValidationResult validationResult)
        {
            return validationResult.Errors.Where(x => x.Severity == Severity.Error);
        }
    }
}

namespace clearwaterstream.IoC
{
    public static class FluentValidationExtensions
    {
        public static IValidator<T> GetValidatorFor<T>(this ServiceRegistrar registrar)
        {
            var result = registrar.GetInstance<IValidator<T>>();

            return result;
        }

        public static IValidator<T> GetValidatorFor<T>(this ServiceRegistrar registrar, string registrationKey)
        {
            registrar.TryGetInstance(registrationKey, out IValidator<T> validator);

            return validator;
        }
    }
}

namespace Autofac
{
    public static class ClearwaterstreamFluentValidationExtensions
    {
        public static IRegistrationBuilder<TLimit, SimpleActivatorData, TRegistrationStyle>
               AsValidator<TLimit, TRegistrationStyle>(
               this IRegistrationBuilder<TLimit, SimpleActivatorData, TRegistrationStyle> registration)
               where TLimit : IValidator
        {
            var limitType = (TypeInfo)registration.ActivatorData.Activator.LimitType;

            var lastRegistration = registration;

            foreach(var iface in limitType.ImplementedInterfaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IValidator<>))
                {
                    lastRegistration = registration.As(iface);
                }
            }

            return lastRegistration;
        }
    }
}
