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

namespace EWorkplaceCoreService.Lib.Services.Divisions
{
    public class DivisionService : IDivisionService
    {
        private readonly CoreDbContext _dbContext;
        private readonly DbSet<Division> _divisionDbSet;
        private readonly IIdentityService _identityService;
        private const string USER_AGENT = "Core Service";

        public DivisionService(CoreDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _divisionDbSet = dbContext.Set<Division>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public Task<int> Create(Division model)
        {
            EntityExtension.FlagForCreate(model, _identityService.Username, USER_AGENT);
            _divisionDbSet.Add(model);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var model = await GetSingleById(id);

            if (model == null)
                throw new Exception("Invalid Id");

            EntityExtension.FlagForDelete(model, _identityService.Username, USER_AGENT);
            _divisionDbSet.Update(model);
            return await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Division> GetQuery()
        {
            return _divisionDbSet.AsNoTracking();
        }

        public Task<Division> GetSingleById(int id)
        {
            return _divisionDbSet.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public Task<int> Update(int id, Division model)
        {
            EntityExtension.FlagForUpdate(model, _identityService.Username, USER_AGENT);
            _divisionDbSet.Update(model);
            return _dbContext.SaveChangesAsync();
        }
    }
}
