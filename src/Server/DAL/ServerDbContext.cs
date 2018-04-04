using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.DAL
{
    public interface IServerDbContext
    {
        DbSet<Node> Nodes { get; }

        int SaveChanges();
    }

    public class ServerDbContext : DbContext, IServerDbContext
    {
        public ServerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
    }
}