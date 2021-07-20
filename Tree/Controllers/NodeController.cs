using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Abstarction;
using Tree.Models;
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
            var nodes = nodeService.GetAll().ToList();
            var mainNode = nodes.FirstOrDefault(n => !n.ParentNodeId.HasValue);
            var vm = new NodeViewModel(mainNode);

            vm.SetChilds(nodes);

            return Json(new { data = vm });
        }

        public IActionResult Create(int id)
        {
            var vm = new NodeCreateViewModel();
            var node = nodeService.GetItem(id);

            if (node == null)
            {
                vm.NodeList = new List<SelectListItem>() { new SelectListItem() { Text = "brak", Value = "0" } };
            }
            else 
            {
                vm.NodeList = new List<SelectListItem>() { new SelectListItem() { Text = node.Name, Value = node.Id.ToString(), Selected = node.Id == id } };
            }
            

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, NodeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var node = new Node();
                node.Name = vm.Name;
                node.ParentNodeId = id == 0 ? null : id;

                this.nodeService.Add(node);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(vm);
            }
        }
    }
}
