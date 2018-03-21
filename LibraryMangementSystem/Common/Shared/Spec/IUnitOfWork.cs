namespace LMS.Shared.Spec
{
    public interface IUnitOfWork
    {
        IBookRespository BookRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        int SaveChanges();
    }
}