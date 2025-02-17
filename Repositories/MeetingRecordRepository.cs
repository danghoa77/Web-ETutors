using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class MeetingRecordRepository : Repository<MeetingRecord>, IMeetingRecordRepository
    {
        private readonly DbContext _dbContext;

        public MeetingRecordRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<MeetingRecord?> GetMeetingRecordWithDetailsAsync(int id)
        {
            return await _dbContext.Set<MeetingRecord>()
                .Include(m => m.Room)
                    .ThenInclude(r => r.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MeetingRecord>> GetMeetingRecordsByRoomAsync(int roomId)
        {
            return await _dbContext.Set<MeetingRecord>()
                .Where(m => m.RoomId == roomId)
                .OrderByDescending(m => m.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<MeetingRecord>> GetMeetingRecordsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Set<MeetingRecord>()
                .Include(m => m.Room)
                .Where(m => m.StartTime >= startDate && m.EndTime <= endDate)
                .OrderByDescending(m => m.StartTime)
                .ToListAsync();
        }
    }
} 