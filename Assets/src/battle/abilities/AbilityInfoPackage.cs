using System.Collections.Generic;
using Src.Backend;
using src.battle.entities;
using src.grid_management;
using UnityEngine;

namespace src.battle.abilities
{
    public class AbilityInfoPackage
    {
        public Node NodeOrigin, NodeTarget;
        public EntityInstance abilityUser, abilityTargetEntity;
        
        public List<Node> AffectedAreaNodes;
        public List<EntityInstance> AffectedEntityInstances;
        
        public AbilityInfoPackage(AbilityType abilityType, Node origin, Node target)
        {
            EntityRegistryService entityRegistryService = ServiceLocator.Get<EntityRegistryService>();
            NodeOrigin = origin;
            NodeTarget = target;
            abilityUser = entityRegistryService.GetEntity(origin.occupantID);
            abilityTargetEntity = entityRegistryService.GetEntity(target.occupantID);
            
            //TODO: IMPLEMENT AOE STUFF
        }
    }
}
