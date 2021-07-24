using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Facades;
using Tree.Utilities.Extensions;
using Tree.ViewModels;

namespace Tree.Controllers
{
    [Authorize]
    public class NodeController : Controller
    {
        private readonly NodeFacade nodeFacade;

        public NodeController(NodeFacade nodeFacade)
        {
            this.nodeFacade = nodeFacade;
        }

        public IActionResult Index()
        {
            var count = this.nodeFacade.GetNodesCount();

            return View(count > 0);
        }

        public IActionResult GetMainNodeWithChilds()
        {
            IActionResult result = NotFound();

            var nodeVm = this.nodeFacade.GetMainNodeWithChilds();

            if (nodeVm != null)
            {
                result = Json(new { data = nodeVm });
            }

            return result;
        }

        public IActionResult Create(int id)
        {
            var vm = this.nodeFacade.GetCreateNodeItem(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, NodeCreateViewModel nodeVm)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                this.nodeFacade.CreateNode(id, nodeVm);

                result = RedirectToAction(nameof(Index));
            }
            else
            {
                nodeVm.NodeList = this.nodeFacade.GetNodeSelectList(id);
                
                result = View(nodeVm);
            }

            return result;
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = this.nodeFacade.DeleteItem(id);

            return Json(new { success = isDeleted });
        }

        public IActionResult Edit(int id)
        {
            var nodeVm = this.nodeFacade.GetUpdateNodeItem(id);

            return nodeVm != null ? View(nodeVm) : RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, NodeUpdateViewModel nodeVm)
        {
            IActionResult result = null;

            if (ModelState.IsValid)
            {
                this.nodeFacade.UpdateNode(nodeVm);

                result = RedirectToAction(nameof(Index));
            }
            else
            {
                nodeVm.NodeList = this.nodeFacade.GetEditNodeSelectList(id, nodeVm.ParentNodeId == 0);

                result = View(nodeVm);
            }

            return result;
        }
    }
}
