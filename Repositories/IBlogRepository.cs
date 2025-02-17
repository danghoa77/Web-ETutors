using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<Blog?> GetBlogWithDetailsAsync(int id);
        Task<IEnumerable<Blog>> GetBlogsByAuthorAsync(string authorId);
        Task<IEnumerable<Post>> GetBlogPostsAsync(int blogId);
    }
} 