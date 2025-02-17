using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface ITutorRequestRepository : IRepository<TutorRequest>
    {
        Task<TutorRequest?> GetTutorRequestWithDetailsAsync(int id);
        Task<IEnumerable<TutorRequest>> GetRequestsByStudentAsync(string studentId);
        Task<IEnumerable<TutorRequest>> GetRequestsByStatusAsync(string status);
    }
} 