using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        private readonly DbContext _dbContext;

        public StaffRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Staff?> GetStaffWithDetailsAsync(string id)
        {
            return await _dbContext.Set<Staff>()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Staff>> GetStaffByPositionAsync(string position)
        {
            return await _dbContext.Set<Staff>()
                .Include(s => s.User)
                .Where(s => s.Position == position)
                .ToListAsync();
        }
    }
} 