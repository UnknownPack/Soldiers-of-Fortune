using System.Collections.Generic;
using Src.Backend;
using src.grid_management;
using src.pathfinding;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = src.grid_management.Grid;
using Task = System.Threading.Tasks.Task;

namespace src.grids
{
    public class GridManagementService : MonoBehaviour
    {
        private Grid grid;
        private MovementService movementService;
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

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

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Movement_WalkUnit(uint unitInstanceID, Node start, Node end)
        { 
            List<Node> path = PathFinder.GetPath(grid.nodes, start, end);
            GridMovement gridMovement = new GridMovement(unitInstanceID, path, grid);
            StartCoroutine(movementService.MoveUnit(gridMovement));
        }
    }
}
