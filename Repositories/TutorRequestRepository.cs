using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class TutorRequestRepository : Repository<TutorRequest>, ITutorRequestRepository
    {
        private readonly DbContext _dbContext;

        public TutorRequestRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<TutorRequest?> GetTutorRequestWithDetailsAsync(int id)
        {
            return await _dbContext.Set<TutorRequest>()
                .Include(tr => tr.Student)
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<IEnumerable<TutorRequest>> GetRequestsByStudentAsync(string studentId)
        {
            return await _dbContext.Set<TutorRequest>()
                .Where(tr => tr.StudentId == studentId)
                .OrderByDescending(tr => tr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TutorRequest>> GetRequestsByStatusAsync(string status)
        {
            return await _dbContext.Set<TutorRequest>()
                .Include(tr => tr.Student)
                .Where(tr => tr.Status == status)
                .OrderByDescending(tr => tr.CreatedAt)
                .ToListAsync();
        }
    }
} 