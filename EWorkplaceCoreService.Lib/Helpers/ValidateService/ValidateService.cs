using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EWorkplaceCoreService.Lib.Helpers.ValidateService
{
    public class ValidateService : IValidateService
    {
        private readonly IServiceProvider serviceProvider;

        public ValidateService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Validate(dynamic model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, serviceProvider, null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }
    }
}
