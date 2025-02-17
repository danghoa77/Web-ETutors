using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> GetCommentWithDetailsAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByAuthorAsync(string authorId);
        Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId);
    }
} 