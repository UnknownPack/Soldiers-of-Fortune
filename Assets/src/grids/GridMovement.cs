using System.Collections.Generic;
using System.Linq;
using src.grid_management;
using UnityEngine;
using Grid = src.grid_management.Grid;

namespace src.grids
{
    public class GridMovement
    {
        public uint unitToMoveID;
        public Node startNode; 
        public Node endNode; 
        public Grid GridMap;
        public List<Node> Path;
        public uint Cost;

        public GridMovement(uint unitToMove, List<Node> path, Grid gridMap)
        {
            unitToMoveID = unitToMove;
            Path = path;
            Cost = (uint)path.Count;
            startNode = Path.First();
            endNode = Path.Last();
            GridMap = gridMap;
        }
    }
}
