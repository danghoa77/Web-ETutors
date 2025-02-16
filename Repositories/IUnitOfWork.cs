namespace eTutoring.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ITutorRepository Tutors { get; }
        Task<int> SaveChangesAsync();
    }
} 