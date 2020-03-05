using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EWorkplaceCoreService.Lib.Models
{
    public class JobTitle : StandardEntity, IValidatableObject
    {
        [StringLength(100)]
        public string Code { get; set; }

        public int DivisionId { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "code" }));

            if (string.IsNullOrWhiteSpace(Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "name" }));

            if (DivisionId <= 0)
                validationResult.Add(new ValidationResult("Division is required", new List<string> { "division" }));

            if (validationResult.Count.Equals(0))
            {
                var dbContext = validationContext.GetService<CoreDbContext>();

                if (dbContext.JobTitles.Count(r => r.IsDeleted.Equals(false) && r.Id != Id && r.Code.Equals(Code)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Code already exists", new List<string> { "code" }));

                if (dbContext.JobTitles.Count(r => r.IsDeleted.Equals(false) && r.Id != Id && r.Name.Equals(Name)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Name already exists", new List<string> { "name" }));
            }

            return validationResult;
        }
    }
}
