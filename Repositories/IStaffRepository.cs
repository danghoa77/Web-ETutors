using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetStaffWithDetailsAsync(string id);
        Task<IEnumerable<Staff>> GetStaffByPositionAsync(string position);
    }
} 