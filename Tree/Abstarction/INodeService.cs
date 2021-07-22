﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Models;

namespace Tree.Abstarction
{
    public interface INodeService
    {
        IEnumerable<Node> GetAll();

        IEnumerable<Node> GetAllWithoutChilds(int id);

        Node GetItem(int id);

        int GetNodesCount();

        void Add(Node node);

        void Update(Node node);

        bool Delete(int id);
    }
}
