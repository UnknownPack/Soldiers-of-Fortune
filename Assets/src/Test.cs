using System.Collections.Generic;
using System.Threading.Tasks;
using src.backend;
using Src.Backend;
using src.grid_management;
using src.grids;
using src.pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = src.grid_management.Grid;

namespace src
{
    public class Test : MonoBehaviour
    {
        public GameObject unit;
        public GameObject nodeMarkerPrefab;
        private Grid grid;
        private MovementService movementService;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async Task Start()
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError("Tilemap component not found!");
                return;
            }
        
            this.grid = new grid_management.Grid(tilemap);
            
            if (!ServiceLocator.HasService<MovementService>())
            {
                await ServiceLocator.WaitForServiceAsync<MovementService>();
            }
            movementService = ServiceLocator.Get<MovementService>();
        }
        
        [ContextMenu("test movement")]
        public void TestMovement()
        {
            (Node startNode, Node endNode) = grid.GetRandomNodesFromGrid();
            List<Node> path = PathFinder.GetPath(grid.nodes, startNode, endNode);
            GridMovement gridMovement = new GridMovement(unit, path, grid);
            StartCoroutine(movementService.MoveUnit(gridMovement));
        }
    }
}
