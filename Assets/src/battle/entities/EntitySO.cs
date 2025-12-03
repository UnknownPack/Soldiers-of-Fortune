
using System.Collections.Generic;
using src.backend;
using src.battle.abilities;
using UnityEngine;

namespace src.battle.entities
{
    [CreateAssetMenu(fileName = "EntitySO", menuName = "Scriptable Objects/EntitySO")]
    public class EntitySO : BaseSO
    {
        [SerializeField]private EntityStats baseStats;
        [SerializeField]private EntityStatMultipliers statMultipliers;
        public List<AbilityName> Abilities;   
        public uint level;
    
        public EntityStats GetInitializedStats()
        {
            return new EntityStats
            {
                Health = baseStats.Health * statMultipliers.HealthMultiplier,
                Attack = baseStats.Attack * statMultipliers.AttackMultiplier,
                Defense = baseStats.Defense * statMultipliers.DefenseMultiplier,
                Speed = baseStats.Speed * statMultipliers.SpeedMultiplier,
                ActionPoints = baseStats.ActionPoints,
                MovementPoints = baseStats.MovementPoints
            };
        }
    }
}
