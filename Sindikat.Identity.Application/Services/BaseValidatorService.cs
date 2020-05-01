using FluentValidation.Results;
using Sindikat.Identity.Application.Models;
using Sindikat.Identity.Common.Exceptions;
using System.Collections.Generic;

namespace Sindikat.Identity.Application.Services
{
    public abstract class BaseValidatorService
    {
        public void CheckValidationResults(ValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                var validationErrors = GetValidationErrors(validationResult.Errors);
                throw new ValidationException(validationErrors);
            }
        }

        private static Dictionary<string, List<string>> GetValidationErrors(IList<ValidationFailure> errors)
        {
            var validationErrors = new ValidationErrors();

            foreach (var error in errors)
            {
                validationErrors.AddError(error.PropertyName, error.ErrorMessage);
            }

            return validationErrors.Errors;
        }

        protected static void ThrowValidationError(string propertyName, string errorMsg)
        {
            var validationErrors = new ValidationErrors();

            validationErrors.AddError(propertyName, errorMsg);

            throw new ValidationException(validationErrors.Errors);
        }


    }
}
