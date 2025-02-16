using Microsoft.EntityFrameworkCore;

namespace eTutoring.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IStudentRepository? _studentRepository;
        private ITutorRepository? _tutorRepository;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IStudentRepository Students => 
            _studentRepository ??= new StudentRepository(_context);

        public ITutorRepository Tutors => 
            _tutorRepository ??= new TutorRepository(_context);

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