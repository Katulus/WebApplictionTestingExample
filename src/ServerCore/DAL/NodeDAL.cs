using System;
using System.Collections.Generic;
using System.Linq;
using ServerCore.Models;

namespace ServerCore.DAL
{
    public interface INodeDAL
    {
        IEnumerable<Node> GetNodes();
        void AddNode(Node node);
        void DeleteAll();
    }

    public class NodeDAL : INodeDAL
    {
        private readonly IServerDbContex _dbContext;

        public NodeDAL(IServerDbContex dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Node> GetNodes()
        {
            return _dbContext.Nodes;
        }

        public void AddNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _dbContext.Nodes.Add(node);
            _dbContext.SaveChanges();
            
        }

        public void DeleteAll()
        {
            foreach (Node node in _dbContext.Nodes.ToList())
            {
                _dbContext.Nodes.Remove(node);
            }
            _dbContext.SaveChanges();
        }
    }
}