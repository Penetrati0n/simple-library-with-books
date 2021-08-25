using Database.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<AuthorEntity> Authors { get; set; }
        DbSet<BookEntity> Books { get; set; }
        DbSet<GenreEntity> Genres { get; set; }
        DbSet<PersonEntity> People { get; set; }
        DbSet<LibraryCardEntity> LibraryCards { get; set; }
        ChangeTracker ChangeTracker { get; }

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}
