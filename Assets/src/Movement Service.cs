using System.Collections;
using src.backend;
using Src.Backend;
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
            Node currentCell = gridMovement.startNode;
            foreach (var node in gridMovement.Path)
            {
                // if (!gridMovement.BaseUnit.CanMove())
                //     break;

                yield return Move(currentCell, node, gridMovement);
                currentCell = node;
            }
        }

        //NOTE: WILL NEED MORE SOPHISTICATED HANDLING LATER (E.G. MOVEMNT INTERUPTIONS...)
        private IEnumerator Move(Node currentNode, Node targetNode, GridMovement gridMovement)
        {
            GameObject unitObject = gridMovement.unitToMove;
            if (unitObject == null)
            {
                Debug.LogWarning("No unit to move");
                yield break;
            }
            
            float duration = MovementSpeed, elapsedTIme = 0;
            Vector3 startPosition = currentNode.realPosition;
            Vector3 endPosition = targetNode.realPosition;

            while (elapsedTIme < duration)
            {
                unitObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTIme / duration);
                elapsedTIme += Time.deltaTime;
                yield return null;
            }
            unitObject.transform.position = endPosition;
        }
    }
}
