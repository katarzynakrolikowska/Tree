using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tree.ViewModels
{
    public class NodeCreateViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Parent node")]
        public int? ParentNodeId { get; set; }

        public IEnumerable<SelectListItem> NodeList { get; set; }

    }
}
