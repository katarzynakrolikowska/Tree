using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Abstarction;
using Tree.Data;
using Tree.Models;

namespace Tree.Services
{
    public class NodeService : INodeService
    {
        private readonly ApplicationDbContext dbContext;

        public NodeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Node> GetAll()
        {
           return this.dbContext.Nodes.ToList();
        }

        public Node GetItem(int id)
        {
            return this.dbContext.Nodes.Find(id);
        }

        public int GetNodesCount()
        {
            return this.dbContext.Nodes.Count();
        }

        public void Add(Node node)
        {
            this.dbContext.Nodes.Add(node);

            this.dbContext.SaveChanges();
        }
    }
}
