using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IMeetingRecordRepository : IRepository<MeetingRecord>
    {
        Task<MeetingRecord?> GetMeetingRecordWithDetailsAsync(int id);
        Task<IEnumerable<MeetingRecord>> GetMeetingRecordsByRoomAsync(int roomId);
        Task<IEnumerable<MeetingRecord>> GetMeetingRecordsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 