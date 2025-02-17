using eTutoring.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly DbContext _dbContext;

        public ChatMessageRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesBetweenUsersAsync(string user1Id, string user2Id)
        {
            return await _dbContext.Set<ChatMessage>()
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                           (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetUserMessagesAsync(string userId)
        {
            return await _dbContext.Set<ChatMessage>()
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetRecentMessagesAsync(string userId, int count)
        {
            return await _dbContext.Set<ChatMessage>()
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentDate)
                .Take(count)
                .ToListAsync();
        }
    }
} 