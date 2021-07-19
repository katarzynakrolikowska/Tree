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
    public class NodeController : Controller
    {
        private readonly INodeService nodeService;

        public NodeController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        public IActionResult Index()
        {
            var vm = new NodeViewModel();

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new NodeCreateViewModel();
            var nodes = nodeService.GetAll();

            if (nodes == null || !nodes.Any())
            {
                vm.NodeList = new List<SelectListItem>() { new SelectListItem() { Text = "brak", Value = "0" } };
            }
            else 
            {
                vm.NodeList = nodes.Select(m => new SelectListItem()
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                });
            }
            

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NodeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var node = new Node();
                node.Name = vm.Name;
                node.ParentNodeId = vm.ParentNodeId == 0 ? null : vm.ParentNodeId;

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
