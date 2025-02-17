using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly DbContext _dbContext;

        public BlogRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Blog?> GetBlogWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Blog>()
                .Include(b => b.Author)
                .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Blog>> GetBlogsByAuthorAsync(string authorId)
        {
            return await _dbContext.Set<Blog>()
                .Include(b => b.Posts)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetBlogPostsAsync(int blogId)
        {
            return await _dbContext.Set<Post>()
                .Include(p => p.Comments)
                .Where(p => p.BlogId == blogId)
                .ToListAsync();
        }
    }
} 