using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Models;
using Tree.ViewModels;

namespace Tree.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NodeCreateViewModel, Node>();

            CreateMap<Node, NodeUpdateViewModel>();

            CreateMap<NodeUpdateViewModel, Node>()
                .ForMember(dest => dest.ParentNodeId, opt => opt.MapFrom(src => src.ParentNodeId == 0 ? null : src.ParentNodeId));
        }
    }
}
