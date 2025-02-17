using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post?> GetPostWithDetailsAsync(int id);
        Task<IEnumerable<Post>> GetPostsByBlogAsync(int blogId);
        Task<IEnumerable<Comment>> GetPostCommentsAsync(int postId);
    }
} 