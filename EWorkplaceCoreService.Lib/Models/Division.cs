using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EWorkplaceCoreService.Lib.Models
{
    public class Division : StandardEntity, IValidatableObject
    {
        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "Code" }));

            if (string.IsNullOrWhiteSpace(Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "Name" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                var dbContext = validationContext.GetService<CoreDbContext>();

                if (dbContext.Divisions.Count(r => r.IsDeleted.Equals(false) && r.Id != Id && r.Name.Equals(Name)) > 0) /* Name Unique */
                    validationResult.Add(new ValidationResult("Name already exists", new List<string> { "Name" }));
            }

            return validationResult;
        }
    }
}
