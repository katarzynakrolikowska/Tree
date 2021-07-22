using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Abstarction;
using Tree.Models;
using Tree.Utilities.Extensions;
using Tree.ViewModels;

namespace Tree.Controllers
{
    [Authorize]
    public class NodeController : Controller
    {
        private readonly INodeService nodeService;

        public NodeController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        public IActionResult Index()
        {
            var count = this.nodeService.GetNodesCount();

            return View(count > 0);
        }

        public IActionResult GetAll()
        {
            IActionResult result = NotFound();

            var nodes = this.nodeService.GetAll().ToList();

            if (!nodes.IsNullOrEmpty())
            {
                var mainNode = nodes.FirstOrDefault(n => !n.ParentNodeId.HasValue);
                var vm = new NodeViewModel(mainNode);

                vm.SetChilds(nodes);
                result = Json(new { data = vm });
            }

            return result;
        }

        public IActionResult Create(int id)
        {
            var vm = new NodeCreateViewModel();

            vm.NodeList = GetNodeSelectList(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, NodeCreateViewModel vm)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                var node = new Node();
                node.Name = vm.Name;
                node.ParentNodeId = id == 0 ? null : id;

                this.nodeService.Add(node);

                result = RedirectToAction(nameof(Index));
            }
            else
            {
                vm.NodeList = GetNodeSelectList(id);
                
                result = View(vm);
            }

            return result;
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = this.nodeService.Delete(id);

            return Json(new { success = isDeleted });
        }

        private IEnumerable<SelectListItem> GetNodeSelectList(int id)
        {
            var nodeList = new List<SelectListItem>();

            var node = this.nodeService.GetItem(id);

            var item = node == null ? new SelectListItem() { Text = "brak", Value = "0" } : new SelectListItem() { Text = node.Name, Value = node.Id.ToString(), Selected = node.Id == id };
            nodeList.Add(item);

            return nodeList;
        }
    }
}
