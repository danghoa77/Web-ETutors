using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly DbContext _dbContext;

        public CommentRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Comment?> GetCommentWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Comment>()
                .Include(c => c.Author)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByAuthorAsync(string authorId)
        {
            return await _dbContext.Set<Comment>()
                .Include(c => c.Post)
                .Where(c => c.AuthorId == authorId)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId)
        {
            return await _dbContext.Set<Comment>()
                .Include(c => c.Author)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }
    }
} 