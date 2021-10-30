using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Repositories;

public class AlmanimeContextFactory : IDesignTimeDbContextFactory<AlmanimeContext>
{
    public AlmanimeContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AlmanimeContext>();
        optionsBuilder.UseSqlServer("Server=db;Database=Almanime;User Id=sa;Password=P@ssw0rd;");

        return new AlmanimeContext(optionsBuilder.Options);
    }
}
