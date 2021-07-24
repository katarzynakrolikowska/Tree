using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Abstarction;
using Tree.Models;
using Tree.Utilities.Extensions;
using Tree.ViewModels;

namespace Tree.Facades
{
    public class NodeFacade
    {
        private readonly INodeService nodeService;
        private readonly IMapper mapper;

        public NodeFacade(INodeService nodeService, IMapper mapper)
        {
            this.nodeService = nodeService;
            this.mapper = mapper;
        }

        public int GetNodesCount()
        {
            return this.nodeService.GetNodesCount();
        }

        public NodeViewModel GetMainNodeWithChilds()
        {
            NodeViewModel vm = null;
            var nodes = this.nodeService.GetAll().ToList();

            if (!nodes.IsNullOrEmpty())
            {
                var mainNode = nodes.FirstOrDefault(n => !n.ParentNodeId.HasValue);
                vm = new NodeViewModel(mainNode);

                vm.SetChilds(nodes);
            }

            return vm;
        }

        public NodeCreateViewModel GetCreateNodeItem(int id)
        {
            var vm = new NodeCreateViewModel();

            vm.NodeList = GetNodeSelectList(id);

            return vm;
        }

        public NodeUpdateViewModel GetUpdateNodeItem(int id)
        {
            NodeUpdateViewModel vm = null;
            var node = this.nodeService.GetItem(id);

            if (node != null)
            {
                vm = this.mapper.Map<NodeUpdateViewModel>(node);

                vm.NodeList = GetEditNodeSelectList(id, node.ParentNodeId == null);
            }

            return vm;
        }

        public IEnumerable<SelectListItem> GetNodeSelectList(int id)
        {
            var nodeList = new List<SelectListItem>();

            var node = this.nodeService.GetItem(id);

            var item = node == null ? new SelectListItem("brak", "0") : new SelectListItem(node.Name, node.Id.ToString(), node.Id == id);
            nodeList.Add(item);

            return nodeList;
        }

        public IEnumerable<SelectListItem> GetEditNodeSelectList(int id, bool isMainNode = false)
        {
            var nodeList = new List<SelectListItem>();

            if (!isMainNode)
            {
                var nodes = this.nodeService.GetAllWithoutChilds(id);

                foreach (var node in nodes)
                {
                    var item = new SelectListItem(node.Name, node.Id.ToString(), node.Id == id);
                    nodeList.Add(item);
                }
            }
            else
            {
                nodeList.Add(new SelectListItem("brak", "0", true));
            }


            return nodeList;
        }

        public bool CreateNode(int id, NodeCreateViewModel nodeVm)
        {
            var node = this.mapper.Map<Node>(nodeVm);
            node.ParentNodeId = id == 0 ? null : id;

            return this.nodeService.Add(node);
        }

        public void UpdateNode(NodeUpdateViewModel nodeVm)
        {
            var node = this.mapper.Map<Node>(nodeVm);

            this.nodeService.Update(node);
        }

        public bool DeleteItem(int id)
        {
            return this.nodeService.Delete(id);
        }
    }
}
