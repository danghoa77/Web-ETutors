using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly DbContext _dbContext;

        public StudentRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Student?> GetStudentWithDetailsAsync(string id)
        {
            return await _dbContext.Set<Student>()
                .Include(s => s.User)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetStudentsByTutoringTypeAsync(string tutoringType)
        {
            return await _dbContext.Set<Student>()
                .Include(s => s.User)
                .Where(s => s.TutoringType == tutoringType)
                .ToListAsync();
        }

        public async Task<bool> IsStudentInRoomAsync(string studentId, int roomId)
        {
            return await _dbContext.Set<Student>()
                .AnyAsync(s => s.Id == studentId && s.RoomId == roomId);
        }
    }
} 