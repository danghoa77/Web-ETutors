using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface ITutorRepository : IRepository<Tutor>
    {
        Task<Tutor?> GetTutorWithDetailsAsync(string id);
        Task<IEnumerable<Tutor>> GetTutorsByExpertiseAsync(string expertise);
        Task<IEnumerable<Room>> GetTutorRoomsAsync(string tutorId);
    }
} 