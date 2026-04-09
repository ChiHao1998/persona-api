using Api.Model.Entity;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Context.PersonaBackend
{
    public class PersonaContext(DbContextOptions<PersonaContext> options) : DbContext(options)
    {
        public DbSet<User> users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }
    }
    public class PersonaContextFactory : IDesignTimeDbContextFactory<PersonaContext>
    {
        public PersonaContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<PersonaContext> optionsBuilder = new();

            DbContextOptions<PersonaContext> options = new DbContextOptionsBuilder<PersonaContext>()
            .UseNpgsql("Host=localhost;Database=__design_time__;Username=ef;Password=ef")
            .Options;

            return new PersonaContext(options);
        }
    }
}