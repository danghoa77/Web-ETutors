using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class TutorRepository : Repository<Tutor>, ITutorRepository
    {
        private readonly DbContext _dbContext;

        public TutorRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Tutor?> GetTutorWithDetailsAsync(string id)
        {
            return await _dbContext.Set<Tutor>()
                .Include(t => t.User)
                .Include(t => t.Rooms)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tutor>> GetTutorsByExpertiseAsync(string expertise)
        {
            return await _dbContext.Set<Tutor>()
                .Include(t => t.User)
                .Where(t => t.Expertise == expertise)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetTutorRoomsAsync(string tutorId)
        {
            return await _dbContext.Set<Room>()
                .Where(r => r.TutorId == tutorId)
                .Include(r => r.Documents)
                .Include(r => r.MeetingRecords)
                .ToListAsync();
        }
    }
} 