using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document?> GetDocumentWithDetailsAsync(int id);
        Task<IEnumerable<Document>> GetDocumentsByUserAsync(string userId);
        Task<IEnumerable<Document>> GetDocumentsByRoomAsync(int roomId);
    }
} 