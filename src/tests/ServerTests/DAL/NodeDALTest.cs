﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Server.DAL;
using Server.Models;
using Xunit;

namespace ServerTests.DAL
{
    public class NodeDALTest
    {
        private readonly NodeDAL _dal;
        private readonly ServerDbContext _dbContext;

        public NodeDALTest()
        {
            // create in-memory DB just for this test
            var dbOptions = new DbContextOptionsBuilder<ServerDbContext>().UseInMemoryDatabase("UnitTests").Options;
            _dbContext = new ServerDbContext(dbOptions);
            _dbContext.Database.EnsureDeleted();
            _dal = new NodeDAL(_dbContext);
        }

        [Fact]
        public void AddNode_SetsNodeId()
        {
            Node node = new Node { IpOrHostname = "1.2.3.4" };

            _dal.AddNode(node);

           node.Id.Should().BeGreaterThan(0, "Node ID was not set.");
        }

        [Fact]
        public void AddNode_AddsNodeToDatabase()
        {
            Node node = new Node { IpOrHostname = "1.2.3.4" };

            _dal.AddNode(node);

            _dbContext.Nodes.Should().HaveCount(1, "Node was not added");
        }

        [Fact]
        public void GetNodes_ReturnsNodesFromDatabase()
        {
            _dbContext.Nodes.Add(new Node {IpOrHostname = "1.1.1.1"});
            _dbContext.Nodes.Add(new Node {IpOrHostname = "2.2.2.2"});
            _dbContext.SaveChanges();

            IEnumerable<Node> nodes = _dal.GetNodes();

            nodes.Select(x => x.IpOrHostname).Should().BeEquivalentTo("1.1.1.1", "2.2.2.2");
        }

        [Fact]
        public void DeleteAll_DeletesAllNodes()
        {
            _dbContext.Nodes.Add(new Node { IpOrHostname = "1.1.1.1" });
            _dbContext.Nodes.Add(new Node { IpOrHostname = "2.2.2.2" });
            _dbContext.SaveChanges();

            _dal.DeleteAll();

           _dbContext.Nodes.Should().BeEmpty("All nodes were not deleted.");
        }
    }
}
