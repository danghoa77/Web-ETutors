using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<Room?> GetRoomWithDetailsAsync(int id);
        Task<IEnumerable<Room>> GetRoomsByTutorAsync(string tutorId);
        Task<IEnumerable<Document>> GetRoomDocumentsAsync(int roomId);
        Task<IEnumerable<MeetingRecord>> GetRoomMeetingRecordsAsync(int roomId);
    }
} 