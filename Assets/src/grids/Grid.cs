using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace src.grid_management
{
    public class Grid
    {
        public Dictionary<Vector2Int, Node> nodes;
        public Tilemap tilemap;
        public Vector2Int gridSize;
        public Vector3 offset;
        
        public Grid(Tilemap tilemap)
        { 
            if (tilemap == null)
            {
                Debug.LogError("Tilemap is null");
                return;
            }
            nodes = new Dictionary<Vector2Int, Node>();
            this.tilemap = tilemap;
            offset = tilemap.tileAnchor;
            
            BoundsInt boundOfTileMap = tilemap.cellBounds;
            gridSize = new Vector2Int(boundOfTileMap.size.x-1, boundOfTileMap.size.y-1);

            foreach (Vector3Int pos in boundOfTileMap.allPositionsWithin)
            {
                bool hasTile = tilemap.HasTile(pos); 
                Vector2Int gridPos = new Vector2Int(pos.x, pos.y);
                Vector3 worldPos = tilemap.CellToWorld(pos);
                nodes[gridPos] = new Node(gridPos, worldPos, hasTile);
                Debug.Log($"Node added at grid position: {gridPos}, " +
                          $"real position: {worldPos}, " +
                          $"walkable: {hasTile}");
            }
        }

        public (Node nodeOne, Node nodeTwo) GetRandomNodesFromGrid()
        {
            if (nodes.Count < 2)
            {
                Debug.LogError("Not enough nodes in grid!");
                return (null, null);
            }

            // ✅ Get all valid keys
            List<Vector2Int> allKeys = new List<Vector2Int>(nodes.Keys);
    
            // ✅ Pick two random indices
            int index1 = Random.Range(0, allKeys.Count);
            int index2 = Random.Range(0, allKeys.Count);
    
            // ✅ Ensure they're different
            while (index2 == index1)
            {
                index2 = Random.Range(0, allKeys.Count);
            }
    
            return (nodes[allKeys[index1]], nodes[allKeys[index2]]);
        }
    }
}
