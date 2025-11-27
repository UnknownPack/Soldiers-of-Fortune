using System;
using UnityEngine;

namespace src.battle.entities
{
    public abstract class EntityInstance: MonoBehaviour
    {
        protected uint Unique_ID;
        [SerializeField] protected EntitySO StatsSource_SO;
        protected EntityStats IntializedStats;

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
        }
    }

    [System.Serializable]
    public struct EntityStats
    {
        public uint Health;
        public uint Attack;
        public uint Defense;
        public uint Speed;
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