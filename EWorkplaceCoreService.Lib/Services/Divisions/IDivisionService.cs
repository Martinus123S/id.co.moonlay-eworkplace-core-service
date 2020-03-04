using EWorkplaceCoreService.Lib.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EWorkplaceCoreService.Lib.Services.Divisions
{
    public interface IDivisionService
    {
        IQueryable<Division> GetQuery();
        Task<Division> GetSingleById(int id);
        Task<int> Create(Division model);
        Task<int> Update(int id, Division model);
        Task<int> Delete(int id);
    }
}
