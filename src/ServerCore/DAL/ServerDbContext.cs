using Microsoft.EntityFrameworkCore;
using ServerCore.Models;

namespace ServerCore.DAL
{
    public interface IServerDbContex
    {
        DbSet<Node> Nodes { get; }

        int SaveChanges();
    }

    public class ServerDbContext : DbContext, IServerDbContex
    {
        public ServerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
    }
}