using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly DbContext _dbContext;

        public PostRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Post?> GetPostWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Post>()
                .Include(p => p.Blog)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetPostsByBlogAsync(int blogId)
        {
            return await _dbContext.Set<Post>()
                .Include(p => p.Comments)
                .Where(p => p.BlogId == blogId)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetPostCommentsAsync(int postId)
        {
            return await _dbContext.Set<Comment>()
                .Include(c => c.Author)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }
    }
} 