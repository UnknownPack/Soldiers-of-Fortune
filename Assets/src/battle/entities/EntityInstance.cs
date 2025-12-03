using System;
using System.Collections.Generic;
using System.Linq;
using src.battle.abilities;
using UnityEngine;

namespace src.battle.entities
{
    public abstract class EntityInstance: MonoBehaviour
    {
        protected uint Unique_ID;
        [SerializeField] protected EntitySO StatsSource_SO;
        protected EntityStats IntializedStats;
        protected Dictionary<AbilityName, uint> Abilities;
        
        // current instance information
        public uint Health;
        public uint Attack;
        public uint Defense;
        public uint Speed;
        public uint ActionPoints;
        public uint MovementPoints;
        public Vector2Int gridPosition;

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            
        }

        protected virtual void InitalizeEntity()
        {
            Unique_ID = src.backend.IDGenerator.GetRuntimeAssetID();
            if (StatsSource_SO == null)
            {
                Debug.LogError("StatsSource_SO is not assigned!");
                return;
            }
            IntializedStats = StatsSource_SO.GetInitializedStats();
            Health = IntializedStats.Health;
            Attack = IntializedStats.Attack;
            Defense = IntializedStats.Defense;
            Speed = IntializedStats.Speed;
            ActionPoints = IntializedStats.ActionPoints;
            MovementPoints = IntializedStats.MovementPoints;
            foreach (AbilityName abilityName in StatsSource_SO.Abilities)
            {
                
            } 
        }

        #region Helper Methods
        
        public void SetGridPosition(Vector2Int newPosition) => gridPosition = newPosition;
        public List<AbilityName> GetAbilities => StatsSource_SO.Abilities; 
        
        public uint UniqueID => Unique_ID;
        public bool CanUseAbility(AbilityName abilityName)
        {
            return Abilities.ContainsKey(abilityName) && Abilities[abilityName] > 0;
        }

        #endregion
    }

    [System.Serializable]
    public struct EntityStats
    {
        public uint Health;
        public uint Attack;
        public uint Defense;
        public uint Speed;
        public uint ActionPoints;
        public uint MovementPoints;
    }
    
    [System.Serializable]
    public struct EntityStatMultipliers
    {
        public uint HealthMultiplier;
        public uint AttackMultiplier;
        public uint DefenseMultiplier;
        public uint SpeedMultiplier;
    }
}