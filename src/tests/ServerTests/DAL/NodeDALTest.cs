using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Server.DAL;
using Server.Models;

namespace ServerTests.DAL
{
    [TestFixture]
    public class NodeDALTest
    {
        private NodeDAL _dal;
        private ServerDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            // create in-memory DB just for this test
            _dbContext = new ServerDbContext(Effort.DbConnectionFactory.CreateTransient());
            _dal = new NodeDAL(_dbContext);
        }

        [Test]
        public void AddNode_SetsNodeId()
        {
            Node node = new Node { IpOrHostname = "1.2.3.4" };

            _dal.AddNode(node);

            Assert.That(node.Id, Is.GreaterThan(0), "Node ID was not set.");
        }

        [Test]
        public void AddNode_AddsNodeToDatabase()
        {
            Node node = new Node { IpOrHostname = "1.2.3.4" };

            _dal.AddNode(node);

            Assert.That(_dbContext.Nodes.Count(), Is.EqualTo(1), "Node was not added");
        }

        [Test]
        public void GetNodes_ReturnsNodesFromDatabase()
        {
            _dbContext.Nodes.Add(new Node {IpOrHostname = "1.1.1.1"});
            _dbContext.Nodes.Add(new Node {IpOrHostname = "2.2.2.2"});
            _dbContext.SaveChanges();

            IEnumerable<Node> nodes = _dal.GetNodes();

            Assert.That(nodes.Count(), Is.EqualTo(2), "Wrong number of nodes returned");
            Assert.That(nodes.Any(n => n.IpOrHostname == "1.1.1.1"), Is.True, "First node was not returned with proper data");
            Assert.That(nodes.Any(n => n.IpOrHostname == "2.2.2.2"), Is.True, "Second node was not returned with proper data");
        }

        [Test]
        public void DeleteAll_DeletesAllNodes()
        {
            _dbContext.Nodes.Add(new Node { IpOrHostname = "1.1.1.1" });
            _dbContext.Nodes.Add(new Node { IpOrHostname = "2.2.2.2" });
            _dbContext.SaveChanges();

            _dal.DeleteAll();


            Assert.That(_dbContext.Nodes.Count(), Is.EqualTo(0), "All nodes were not deleted.");
        }
    }
}
