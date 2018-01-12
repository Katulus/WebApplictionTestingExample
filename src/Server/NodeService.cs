using System;
using System.Collections.Generic;
using Server.DAL;
using Server.Models;

namespace Server
{
    public interface INodeService
    {
        int AddNode(Node node);
        IEnumerable<Node> GetNodes();
        void DeleteAll();
    }

    public class NodeService : INodeService
    {
        private readonly INodeDAL _dal;
        private readonly INodePluginProvider _pluginProvider;

        public NodeService(INodeDAL dal, INodePluginProvider pluginProvider)
        {
            if (dal == null)
                throw new ArgumentNullException(nameof(dal));
            if (pluginProvider == null)
                throw new ArgumentNullException(nameof(pluginProvider));

            _dal = dal;
            _pluginProvider = pluginProvider;
        }

        public int AddNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!IsNodeValid(node))
                throw new InvalidNodeException();

            _dal.AddNode(node);
            foreach (IAddNodePlugin plugin in _pluginProvider.GetPlugins())
            {
                plugin.AfterNodeAdded(node);
            }

            return node.Id;
        }

        public IEnumerable<Node> GetNodes()
        {
            return _dal.GetNodes();
        }

        public void DeleteAll()
        {
            _dal.DeleteAll();
        }

        private bool IsNodeValid(Node node)
        {
            bool valid = Validate(node);
            foreach (IAddNodePlugin plugin in _pluginProvider.GetPlugins())
            {
                valid = valid && plugin.Validate(node);
            }
            return valid;
        }

        private bool Validate(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // Just test validation, all nodes with Id != 0 are considered invalid.
            return node.Id == 0;
        }
    }
}