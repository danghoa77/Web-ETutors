using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetStudentWithDetailsAsync(string id);
        Task<IEnumerable<Student>> GetStudentsByTutoringTypeAsync(string tutoringType);
        Task<bool> IsStudentInRoomAsync(string studentId, int roomId);
    }
} 