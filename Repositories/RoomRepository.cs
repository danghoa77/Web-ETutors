using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly DbContext _dbContext;

        public RoomRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Room?> GetRoomWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Room>()
                .Include(r => r.Tutor)
                .Include(r => r.Documents)
                .Include(r => r.MeetingRecords)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetRoomsByTutorAsync(string tutorId)
        {
            return await _dbContext.Set<Room>()
                .Include(r => r.Documents)
                .Include(r => r.MeetingRecords)
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetRoomDocumentsAsync(int roomId)
        {
            return await _dbContext.Set<Document>()
                .Include(d => d.UploadedByUser)
                .Where(d => d.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MeetingRecord>> GetRoomMeetingRecordsAsync(int roomId)
        {
            return await _dbContext.Set<MeetingRecord>()
                .Where(m => m.RoomId == roomId)
                .ToListAsync();
        }
    }
} 