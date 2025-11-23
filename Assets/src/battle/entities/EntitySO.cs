using src.backend;
using UnityEngine;

namespace src.battle.entities
{
    [CreateAssetMenu(fileName = "EntitySO", menuName = "Scriptable Objects/EntitySO")]
    public class EntitySO : BaseSO
    {
        [SerializeField]private EntityStats baseStats;
        [SerializeField]private EntityStatMultipliers statMultipliers;
        public uint level;
    
        public EntityStats GetInitializedStats()
        {
            return new EntityStats
            {
                Health = baseStats.Health * statMultipliers.HealthMultiplier,
                Attack = baseStats.Attack * statMultipliers.AttackMultiplier,
                Defense = baseStats.Defense * statMultipliers.DefenseMultiplier,
                Speed = baseStats.Speed * statMultipliers.SpeedMultiplier
            };
        }
    }
}
