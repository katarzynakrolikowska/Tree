using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Abstarction;
using Tree.Data;
using Tree.Models;
using Tree.Utilities.Extensions;

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

        public IEnumerable<Node> GetAllWithoutChilds(int id)
        {
            var childsIds = GetChilds(id).Select(n => n.Id).ToList();

            return this.dbContext.Nodes
                .Where(e => !childsIds.Contains(e.Id) && e.Id != id)
                .ToList();
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

        public void Update(Node node)
        {
            var entity = this.dbContext.Nodes.Find(node.Id);

            if (entity != null)
            {
                entity.Name = node.Name;
                entity.ParentNodeId = node.ParentNodeId;

                this.dbContext.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            var result = false;

            var entity = this.dbContext.Nodes.Find(id);

            if (entity != null)
            {
                var childs = GetChilds(id);

                this.dbContext.Nodes.RemoveRange(childs);
                this.dbContext.Nodes.Remove(entity);

                this.dbContext.SaveChanges();

                result = true;
            }

            return result;
        }

        //rozwiązanie zaczerpnięte ze stackoverflow
        private IEnumerable<Node> GetChilds(int id)
        {
            var lookup = this.dbContext.Nodes.ToLookup(n => n.ParentNodeId);

            return lookup[id].SelectRecursive(n => lookup[n.Id]).ToList();
        }
    }
}
