using System.Collections.Generic;
using Src.Backend;
using UnityEngine;

namespace src.battle.entities
{
    public class EntityRegistryService : MonoBehaviour
    {
        private Dictionary<uint, EntityInstance> entities = new Dictionary<uint, EntityInstance>();

        void Awake()
        {
            ServiceLocator.Register(this);
        }

        public void RegisterEntity(uint id, EntityInstance entity)
        {
            if (!entities.TryGetValue(id, out EntityInstance entityInstance))
            {
                entities.Add(id, entity);
                return;
            }
            Debug.Log($"{id} is already registered in EntityRegistryService. Entity: {entityInstance} will not be added!");
        }
        
        public EntityInstance GetEntity(uint id)
        {
            if (id == 0)
            {
                Debug.LogError($"Gave terminator value for entity ID! Returning null.");    
                return null;
            }
            
            if (entities.TryGetValue(id, out EntityInstance entityInstance))
                return entityInstance;
            
            Debug.LogWarning($"Entity with ID: {id} not found in EntityRegistryService! Returning null.");
            return null;
        } 
        
    }
}
