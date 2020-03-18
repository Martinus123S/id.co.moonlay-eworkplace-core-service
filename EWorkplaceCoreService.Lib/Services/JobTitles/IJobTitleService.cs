using EWorkplaceCoreService.Lib.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EWorkplaceCoreService.Lib.Services.JobTitles
{
    public interface IJobTitleService
    {
        IQueryable<JobTitle> GetQuery();
        Task<JobTitleViewModel> GetSingleById(int id);
        Task<int> Create(JobTitle model);
        Task<int> Update(int id, JobTitle model);
        Task<int> Delete(int id);
    }
}
