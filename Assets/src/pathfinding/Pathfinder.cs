using System.Collections.Generic;
using System.Linq;
using src.grid_management;
using UnityEngine;

namespace src.pathfinding
{ 
    public static class PathFinder
    { 
        public static readonly Vector2Int[] CardinalDirections = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        }; 
        
        public static readonly Vector2Int[] IntercardinalDirections =
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1),
        };

        public static readonly Vector2Int[] AllDirections = CardinalDirections.Concat(IntercardinalDirections).ToArray();

        private static int GenericGCost = 1;

        public static List<Node> GetPath(Dictionary<Vector2Int, Node> nodeDictionary, Node startNode, Node endNode)
        {
            if (nodeDictionary.Count <= 0)
            {
                Debug.LogError("Cell dictionary is empty.");
                return null;
            }

            if (!DoesCellExist(startNode.gridPosition, nodeDictionary) ||
                !DoesCellExist(endNode.gridPosition, nodeDictionary))
            {
                return null;
            }
            
            ResetAllNodes(nodeDictionary);
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();


            openSet.Add(startNode);
            startNode.gCost = 0;
            startNode.hCost = Get_HCost(startNode, endNode);

            while (openSet.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openSet);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (!NotSameNode(currentNode, endNode))
                    return ReconstructPath(startNode, endNode);

                foreach (Node neighbour in GetNeighbours(currentNode, nodeDictionary, CardinalDirections.ToList()))
                {
                    bool canNavigate = neighbour.isWalkable, containsInClosedSet = closedSet.Contains(neighbour);
                    if (!canNavigate || containsInClosedSet)
                        continue;

                    float estimatedCost = currentNode.gCost + Get_GCost(currentNode, neighbour);
                    if (!openSet.Contains(neighbour) || estimatedCost < neighbour.gCost)
                    {
                        neighbour.parentNode = currentNode;
                        neighbour.gCost = estimatedCost;
                        neighbour.hCost = Get_HCost(neighbour, endNode);

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            Debug.LogWarning("No path found.");
            return null;
        }

        public static List<Node> GetLineOfSight(Dictionary<Vector2Int, Node> nodeDictionary, Node startNode,
            Node endNode)
        {
            if (nodeDictionary.Count <= 0)
            {
                Debug.LogError("Cell dictionary is empty.");
                return null;
            }

            if (!DoesCellExist(startNode.gridPosition, nodeDictionary) ||
                !DoesCellExist(endNode.gridPosition, nodeDictionary))
            {
                return null;
            }

            ResetAllNodes(nodeDictionary);
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>(); 

            openSet.Add(startNode);
            startNode.gCost = 0;
            startNode.hCost = Get_HCost(startNode, endNode);

            while (openSet.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openSet);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (!NotSameNode(currentNode, endNode))
                    return ReconstructPath(startNode, endNode);

                foreach (Node neighbour in GetNeighbours(currentNode, nodeDictionary, AllDirections.ToList()))
                {
                    if (closedSet.Contains(neighbour))
                        continue;

                    float estimatedCost = currentNode.gCost + GenericGCost;
                    if (!openSet.Contains(neighbour) || estimatedCost < neighbour.gCost)
                    {
                        neighbour.parentNode = currentNode;
                        neighbour.gCost= estimatedCost;
                        neighbour.hCost = Get_HCost(neighbour, endNode);

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            Debug.LogWarning("No path found.");
            return null;
        }

        public static bool HasLineOfSight(Dictionary<Vector2Int, Node> cellDictionary, Node startNode,
            Node endNode)
        {
            List<Node> lineOfSightCells = GetLineOfSight(cellDictionary, startNode, endNode);
            foreach (var gridCell in lineOfSightCells)
            {
                if (gridCell.IsObstructed)
                    return false;
            }
            return true;
        }

        // Returns all non-obstructed cells in the line of sight path
        // can be used to get all cells that have cover or an enetity within it, could be useful for collateral stuff
        public static List<Node> GetAllObstructedCellsInPath(Dictionary<Vector2Int, Node> cellDictionary, Node startCell,
            Node endCell)

        {
            List<Node> lineOfSightCells = GetLineOfSight(cellDictionary, startCell, endCell);
            List<Node> result = new List<Node>();
            foreach (var gridCell in lineOfSightCells)
            {
                if (!gridCell.IsObstructed)
                    result.Add(gridCell);
            }
            return result;
        }

        private static Node GetLowestFCostNode(List<Node> openSet)
        {
            List<Node> sortedList = openSet.OrderBy(node => node.fCost).ToList();
            return sortedList[0];
        }

        private static List<Node> GetNeighbours(Node targetNode, Dictionary<Vector2Int, Node> cellDictionary, List<Vector2Int> neighboursDirections)
        {
            List<Node> neighbours = new List<Node>();
            foreach (var dir in neighboursDirections)
            {
                Vector2Int checkPos = targetNode.gridPosition + dir;
                if (cellDictionary.TryGetValue(checkPos, out Node neighbour))
                {
                    if (neighbour.isWalkable)
                    {
                        Debug.Log($"{neighbour.gridPosition} is neighbour of {targetNode.gridPosition} and walkable state : {neighbour.isWalkable}");
                        neighbours.Add(neighbour);
                    }
                }
            }
            return neighbours;
        }

        private static List<Node> ReconstructPath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (NotSameNode(currentNode, startNode))
            {
                path.Add(currentNode);
                currentNode = currentNode.parentNode;
            }
            path.Add(startNode);
            path.Reverse();
            return path;
        }

        private static bool NotSameNode(Node nodeOne, Node nodeTwo)
        {
            return nodeOne.gridPosition - nodeTwo.gridPosition != Vector2Int.zero;
        }

        private static float Get_GCost(Node fromNode, Node toNode)
        {
            float travelCost = Vector2Int.Distance(fromNode.gridPosition, toNode.gridPosition);

            //CellState state = toNode.cell.activeCellState;
            //return travelCost + state.dangerCost;
            return travelCost;
        }


        private static float Get_HCost(Node currentNode, Node goalNode)
        {
            return Mathf.Abs(currentNode.gridPosition.x - goalNode.gridPosition.x) +
                   Mathf.Abs(currentNode.gridPosition.y - goalNode.gridPosition.y);
        }

        private static bool DoesCellExist(Vector2Int position, Dictionary<Vector2Int, Node> cellDictionary)
        {
            return cellDictionary.ContainsKey(position);
        }
        
        private static void ResetAllNodes(Dictionary<Vector2Int, Node> nodeDictionary)
        {
            foreach (var node in nodeDictionary.Values)
            {
                node.parentNode = null;
                node.gCost = float.MaxValue;
                node.hCost = 0;
            }
        }
    }
}
