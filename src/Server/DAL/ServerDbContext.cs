using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Server.Models;

namespace Server.DAL
{
    public interface IServerDbContex
    {
        IDbSet<Node> Nodes { get; }

        int SaveChanges();
    }

    public class ServerDbContext : DbContext, IServerDbContex
    {
        public ServerDbContext(DbConnection connection)
            : base(connection, true)
        {
        }

        public IDbSet<Node> Nodes { get; set; }
    }
}