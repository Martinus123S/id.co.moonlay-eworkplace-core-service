using EWorkplaceCoreService.Lib.Models;
using System;

namespace EWorkplaceCoreService.Lib.Services.JobTitles
{
    public class JobTitleViewModel
    {
        public JobTitleViewModel()
        {

        }

        public JobTitleViewModel(JobTitle jobTitle, Division division)
        {
            Division = new JobTitleDivisionViewModel(division);
            Id = jobTitle.Id;
            Code = jobTitle.Code;
            Name = jobTitle.Name;
            LastModifiedUtc = jobTitle.LastModifiedUtc;
        }

        public JobTitleDivisionViewModel Division { get; }
        public int Id { get; }
        public string Code { get; }
        public string Name { get; }
        public DateTime LastModifiedUtc { get; }
    }

    public class JobTitleDivisionViewModel
    {
        public JobTitleDivisionViewModel()
        {

        }

        public JobTitleDivisionViewModel(Division division)
        {
            Id = division.Id;
            Code = division.Code;
            LastModifiedUtc = division.LastModifiedUtc;
            Name = division.Name;
        }

        public int Id { get; }
        public string Code { get; }
        public DateTime LastModifiedUtc { get; }
        public string Name { get; }
    }
}