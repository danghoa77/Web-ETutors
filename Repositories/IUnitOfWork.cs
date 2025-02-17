namespace eTutoring.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ITutorRepository Tutors { get; }
        IRoomRepository Rooms { get; }
        IBlogRepository Blogs { get; }
        IDocumentRepository Documents { get; }
        IChatMessageRepository ChatMessages { get; }
        IMeetingRecordRepository MeetingRecords { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        IStaffRepository Staff { get; }
        Task<int> SaveChangesAsync();
    }
} 