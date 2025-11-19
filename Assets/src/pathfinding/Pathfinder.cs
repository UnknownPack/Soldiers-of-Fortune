using UnityEngine;

public class Pathfinder
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

        public static List<GridCell> GetPath(Dictionary<Vector2Int, GridCell> cellDictionary, GridCell startCell, GridCell endCell)
        {
            if (cellDictionary.Count <= 0)
            {
                Debug.LogError("Cell dictionary is empty.");
                return null;
            }

            Dictionary<Vector2Int, PF_Node> temporaryNodeDictioanry = ProduceNodeDictionary(cellDictionary);

            if (!DoesCellExist(startCell.gridPosition, temporaryNodeDictioanry) ||
                !DoesCellExist(endCell.gridPosition, temporaryNodeDictioanry))
            {
                return null;
            }

            List<PF_Node> openSet = new List<PF_Node>();
            List<PF_Node> closedSet = new List<PF_Node>();

            PF_Node startNode = temporaryNodeDictioanry[startCell.gridPosition];
            PF_Node endNode = temporaryNodeDictioanry[endCell.gridPosition];

            openSet.Add(startNode);
            startNode.GCost = 0;
            startNode.HCost = Get_HCost(startNode, endNode);

            while (openSet.Count > 0)
            {
                PF_Node currentNode = GetLowestFCostNode(openSet);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (!NotSameCell(currentNode.cell, endNode.cell))
                    return ReconstructPath(startNode, endNode);

                foreach (PF_Node neighbour in GetNeighbours(currentNode.cell, temporaryNodeDictioanry, CardinalDirections.ToList()))
                {
                    bool canNavigate = neighbour.cell.canNavigate, containsInClosedSet = closedSet.Contains(neighbour);
                    if (!canNavigate || containsInClosedSet)
                        continue;

                    float estimatedCost = currentNode.GCost + Get_GCost(currentNode, neighbour);
                    if (!openSet.Contains(neighbour) || estimatedCost < neighbour.GCost)
                    {
                        neighbour.parent = currentNode;
                        neighbour.GCost = estimatedCost;
                        neighbour.HCost = Get_HCost(neighbour, endNode);
                        neighbour.FCost = neighbour.GCost + neighbour.HCost;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            Debug.LogWarning("No path found.");
            return null;
        }

        public static List<GridCell> GetLineOfSight(Dictionary<Vector2Int, GridCell> cellDictionary, GridCell startCell,
            GridCell endCell)
        {
            if (cellDictionary.Count <= 0)
            {
                Debug.LogError("Cell dictionary is empty.");
                return null;
            }

            Dictionary<Vector2Int, PF_Node> temporaryNodeDictioanry = ProduceNodeDictionary(cellDictionary);

            if (!DoesCellExist(startCell.gridPosition, temporaryNodeDictioanry) ||
                !DoesCellExist(endCell.gridPosition, temporaryNodeDictioanry))
            {
                return null;
            }

            List<PF_Node> openSet = new List<PF_Node>();
            List<PF_Node> closedSet = new List<PF_Node>();

            PF_Node startNode = temporaryNodeDictioanry[startCell.gridPosition];
            PF_Node endNode = temporaryNodeDictioanry[endCell.gridPosition];

            openSet.Add(startNode);
            startNode.GCost = 0;
            startNode.HCost = Get_HCost(startNode, endNode);

            while (openSet.Count > 0)
            {
                PF_Node currentNode = GetLowestFCostNode(openSet);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (!NotSameCell(currentNode.cell, endNode.cell))
                    return ReconstructPath(startNode, endNode);

                foreach (PF_Node neighbour in GetNeighbours(currentNode.cell, temporaryNodeDictioanry, AllDirections.ToList()))
                {
                    if (closedSet.Contains(neighbour))
                        continue;

                    float estimatedCost = currentNode.GCost + GenericGCost;
                    if (!openSet.Contains(neighbour) || estimatedCost < neighbour.GCost)
                    {
                        neighbour.parent = currentNode;
                        neighbour.GCost = estimatedCost;
                        neighbour.HCost = Get_HCost(neighbour, endNode);
                        neighbour.FCost = neighbour.GCost + neighbour.HCost;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            Debug.LogWarning("No path found.");
            return null;
        }

        public static bool HasLineOfSight(Dictionary<Vector2Int, GridCell> cellDictionary, GridCell startCell,
            GridCell endCell)
        {
            List<GridCell> lineOfSightCells = GetLineOfSight(cellDictionary, startCell, endCell);
            foreach (var gridCell in lineOfSightCells)
            {
                if (gridCell.isObstruted)
                    return false;
            }
            return true;
        }

        // Returns all non-obstructed cells in the line of sight path
        // can be used to get all cells that have cover or an enetity within it, could be useful for collateral stuff
        public static List<GridCell> GetAllObstructedCellsInPath(Dictionary<Vector2Int, GridCell> cellDictionary, GridCell startCell,
            GridCell endCell)

        {
            List<GridCell> lineOfSightCells = GetLineOfSight(cellDictionary, startCell, endCell);
            List<GridCell> result = new List<GridCell>();
            foreach (var gridCell in lineOfSightCells)
            {
                if (!gridCell.isObstruted)
                    result.Add(gridCell);
            }
            return result;
        }

        private static PF_Node GetLowestFCostNode(List<PF_Node> openSet)
        {
            List<PF_Node> sortedList = openSet.OrderBy(node => node.FCost).ToList();
            return sortedList[0];
        }



        private static List<PF_Node> GetNeighbours(GridCell targetCell, Dictionary<Vector2Int, PF_Node> cellDictionary, List<Vector2Int> neighboursDirections)
        {
            List<PF_Node> neighbours = new List<PF_Node>();
            foreach (var dir in neighboursDirections)
            {
                Vector2Int checkPos = targetCell.gridPosition + dir;
                if (cellDictionary.TryGetValue(checkPos, out PF_Node neighbour))
                    neighbours.Add(neighbour);
            }
            return neighbours;
        }

        private static List<GridCell> ReconstructPath(PF_Node startNode, PF_Node endNode)
        {
            List<GridCell> path = new List<GridCell>();
            PF_Node currentNode = endNode;

            while (NotSameCell(currentNode.cell, startNode.cell))
            {
                path.Add(currentNode.cell);
                currentNode = currentNode.parent;
            }
            path.Add(startNode.cell);
            path.Reverse();
            return path;
        }

        private static bool NotSameCell(GridCell startCell, GridCell goalCell)
        {
            return startCell.gridPosition - goalCell.gridPosition != Vector2Int.zero;
        }

        private static float Get_GCost(PF_Node fromNode, PF_Node toNode)
        {
            float travelCost = Vector2Int.Distance(fromNode.cell.gridPosition, toNode.cell.gridPosition);

            CellState state = toNode.cell.activeCellState;
            return travelCost + state.dangerCost;
        }


        private static float Get_HCost(PF_Node currentCell, PF_Node goalCell)
        {
            return Mathf.Abs(currentCell.cell.gridPosition.x - goalCell.cell.gridPosition.x) +
                   Mathf.Abs(currentCell.cell.gridPosition.y - goalCell.cell.gridPosition.y);
        }

        private static bool DoesCellExist(Vector2Int position, Dictionary<Vector2Int, PF_Node> cellDictionary)
        {
            return cellDictionary.ContainsKey(position);
        }

        private static Dictionary<Vector2Int, PF_Node> ProduceNodeDictionary(Dictionary<Vector2Int, GridCell> cellDictionary)
        {
            Dictionary<Vector2Int, PF_Node> nodeDictionary = new Dictionary<Vector2Int, PF_Node>();
            foreach (var cellEntry in cellDictionary)
            {
                nodeDictionary.Add(cellEntry.Key, new PF_Node(cellEntry.Value));
            }
            return nodeDictionary;
        }
    }
}
