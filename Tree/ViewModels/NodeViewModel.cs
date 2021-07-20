using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Models;

namespace Tree.ViewModels
{
    public class NodeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentNodeId { get; set; }

        public List<NodeViewModel> ChildNodes { get; set; }

        public NodeViewModel(Node node)
        {
            this.Id = node.Id;
            this.Name = node.Name;
            this.ParentNodeId = node.ParentNodeId;
        }

        public void SetChilds(List<Node> nodes)
        {
            this.ChildNodes = nodes.Where(n => n.ParentNodeId == this.Id).Select(n => new NodeViewModel(n)).ToList();
            this.ChildNodes.ForEach(child => child.SetChilds(nodes));
        }
    }
}
