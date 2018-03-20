using Microsoft.EntityFrameworkCore;
using ServerCore.Models;

namespace ServerCore.DAL
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