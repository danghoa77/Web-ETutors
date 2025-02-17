using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        private readonly DbContext _dbContext;

        public DocumentRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Document?> GetDocumentWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Document>()
                .Include(d => d.UploadedByUser)
                .Include(d => d.Room)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByUserAsync(string userId)
        {
            return await _dbContext.Set<Document>()
                .Include(d => d.Room)
                .Where(d => d.UploadedBy == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByRoomAsync(int roomId)
        {
            return await _dbContext.Set<Document>()
                .Include(d => d.UploadedByUser)
                .Where(d => d.RoomId == roomId)
                .ToListAsync();
        }
    }
} 