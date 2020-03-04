using Com.Moonlay.Models;
using EWorkplaceCoreService.Lib.Helpers.IdentityService;
using EWorkplaceCoreService.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorkplaceCoreService.Lib.Services.JobTitles
{
    public class JobTitleService : IJobTitleService
    {
        private readonly CoreDbContext _dbContext;
        private readonly DbSet<Division> _divisionDbSet;
        private readonly DbSet<JobTitle> _jobTitleDbSet;
        private readonly IIdentityService _identityService;
        private const string USER_AGENT = "Core Service";

        public JobTitleService(CoreDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _divisionDbSet = dbContext.Set<Division>();
            _jobTitleDbSet = dbContext.Set<JobTitle>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public Task<int> Create(JobTitle model)
        {
            EntityExtension.FlagForCreate(model, _identityService.Username, USER_AGENT);
            _jobTitleDbSet.Add(model);
            return _dbContext.SaveChangesAsync();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<JobTitleListViewModel> GetQuery()
        {
            throw new NotImplementedException();
        }

        public async Task<JobTitleViewModel> GetSingleById(int id)
        {
            var jobTitle = await _jobTitleDbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
            if (jobTitle == null)
                return null;

            var division = await _divisionDbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == jobTitle.DivisionId);
            if (division == null)
                return null;

            return new JobTitleViewModel(jobTitle, division);
        }

        public Task<int> Update(int id, JobTitle model)
        {
            throw new NotImplementedException();
        }
    }
}
