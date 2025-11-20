using System.Collections.Generic;
using System.Linq;
using src.grid_management;
using UnityEngine;
using Grid = src.grid_management.Grid;

namespace src.grids
{
    public class GridMovement
    {
        public GameObject unitToMove;
        public Node startNode; 
        public Node endNode; 
        public Grid GridMap;
        public List<Node> Path;
        public uint Cost;

        public GridMovement(GameObject unit, List<Node> path, Grid gridMap)
        {
            unitToMove = unit;
            Path = path;
            Cost = (uint)path.Count;
            startNode = Path.First();
            endNode = Path.Last();
            GridMap = gridMap;
        }
    }
}
