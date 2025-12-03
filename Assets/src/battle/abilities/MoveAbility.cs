using System.Collections;
using Src.Backend;
using src.battle.entities;
using src.grid_management;
using src.grids;
using UnityEngine;

namespace src.battle.abilities
{
    [CreateAssetMenu(fileName = "BaseAbilitySO", menuName = "Scriptable Objects/AbilitySO/MoveAbilitySO")]
    public class MoveAbility : BaseAbility
    {
        public override void Execute(AbilityInfoPackage abilityInfoPackage)
        {
            GridManagementService gridManagementService = ServiceLocator.Get<GridManagementService>();
            
            gridManagementService.Movement_WalkUnit(abilityInfoPackage.abilityUser.UniqueID,
                abilityInfoPackage.NodeOrigin,
                abilityInfoPackage.NodeTarget);
        }
    }
}
