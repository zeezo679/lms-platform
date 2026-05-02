using FluentValidation;
using LMS.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // loop through all validators and collect the validation results
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // get all the validation failures
                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                // if there are any validation failures, throw a DomainValidationException with the error messages
                if (failures.Any())
                {
                    var errorMessages = string.Join(" | ", failures.Select(f => f.ErrorMessage));
                    throw new DomainValidationException($"Validation failed: {errorMessages}");
                }
            }

            // if the data is valid, let the request continue to the handler
            return await next();
        }
    }
}