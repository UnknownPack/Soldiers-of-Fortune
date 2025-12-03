using System.Collections;
using src.backend;
using Src.Backend;
using src.battle.entities;
using src.grid_management;
using src.grids;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace src
{
    public class MovementService : MonoBehaviour
    { 
        public const float MovementSpeed = 0.5f;
        private const uint FirstCellOfPath = 1;
       
        void Awake()
        {
            ServiceLocator.Register(this);
        }

        void OnDestroy()
        {
            ServiceLocator.Unregister<MovementService>();
        }

        public IEnumerator MoveUnit(GridMovement gridMovement)
        { 
            EntityRegistryService entityRegistryService = ServiceLocator.Get<EntityRegistryService>();
            EntityInstance movingEntity = entityRegistryService.GetEntity(gridMovement.unitToMoveID);
            
            if (movingEntity == null)
            {
                Debug.LogWarning("No unit to move");
                yield break;
            }
            
            Node currentCell = gridMovement.startNode;
            foreach (var node in gridMovement.Path)
            {
                currentCell.occupantID = 0;

                yield return Move(currentCell, node, movingEntity.gameObject);
                
                currentCell = node;
                currentCell.occupantID = gridMovement.unitToMoveID;
                movingEntity.SetGridPosition(currentCell.gridPosition);
                movingEntity.IntializedStats.ActionPoints -= 1;
            }
        }

        //NOTE: WILL NEED MORE SOPHISTICATED HANDLING LATER (E.G. MOVEMNT INTERUPTIONS...)
        private IEnumerator Move(Node currentNode, Node targetNode, GameObject movingObject)
        {
            EntityRegistryService entityRegistryService = ServiceLocator.Get<EntityRegistryService>(); 
            if (movingObject == null)
            {
                Debug.LogWarning("No unit to move");
                yield break;
            }
            
            float duration = MovementSpeed, elapsedTIme = 0;
            Vector3 startPosition = currentNode.realPosition;
            Vector3 endPosition = targetNode.realPosition;

            while (elapsedTIme < duration)
            {
                movingObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTIme / duration);
                elapsedTIme += Time.deltaTime;
                yield return null;
            }
            movingObject.transform.position = endPosition;
        }
    }
}
