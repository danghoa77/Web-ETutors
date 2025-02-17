using eTutoring.Models;

namespace eTutoring.Repositories
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetMessagesBetweenUsersAsync(string user1Id, string user2Id);
        Task<IEnumerable<ChatMessage>> GetUserMessagesAsync(string userId);
        Task<IEnumerable<ChatMessage>> GetRecentMessagesAsync(string userId, int count);
    }
} 