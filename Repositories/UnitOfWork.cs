using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IStudentRepository? _studentRepository;
        private ITutorRepository? _tutorRepository;
        private IRoomRepository? _roomRepository;
        private IBlogRepository? _blogRepository;
        private IDocumentRepository? _documentRepository;
        private IChatMessageRepository? _chatMessageRepository;
        private IMeetingRecordRepository? _meetingRecordRepository;
        private IPostRepository? _postRepository;
        private ICommentRepository? _commentRepository;
        private IStaffRepository? _staffRepository;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IStudentRepository Students => 
            _studentRepository ??= new StudentRepository(_context);

        public ITutorRepository Tutors => 
            _tutorRepository ??= new TutorRepository(_context);

        public IRoomRepository Rooms =>
            _roomRepository ??= new RoomRepository(_context);

        public IBlogRepository Blogs =>
            _blogRepository ??= new BlogRepository(_context);

        public IDocumentRepository Documents =>
            _documentRepository ??= new DocumentRepository(_context);

        public IChatMessageRepository ChatMessages =>
            _chatMessageRepository ??= new ChatMessageRepository(_context);

        public IMeetingRecordRepository MeetingRecords =>
            _meetingRecordRepository ??= new MeetingRecordRepository(_context);

        public IPostRepository Posts =>
            _postRepository ??= new PostRepository(_context);

        public ICommentRepository Comments =>
            _commentRepository ??= new CommentRepository(_context);

        public IStaffRepository Staff =>
            _staffRepository ??= new StaffRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 