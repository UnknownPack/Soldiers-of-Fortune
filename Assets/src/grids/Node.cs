using UnityEngine;
namespace src.grid_management
{
    public class Node
    {
        public Node(Vector2Int gridPosition, Vector3 realPosition, bool isWalkable)
        {
            this.gridPosition = gridPosition;
            this.realPosition = realPosition;
            this.isWalkable = isWalkable;
        }
        
        public Vector2Int gridPosition;
        public Vector3 realPosition;
        
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        
        public GameObject occupant;
        public bool isWalkable;
        public Node parentNode;
        
        public bool IsOccupied => occupant != null; 
        //NOTE: Replace isobstructed with cover later on
        public bool IsObstructed => !isWalkable;
    }
}
