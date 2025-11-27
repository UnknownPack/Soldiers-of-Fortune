using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.battle.abilities
{
    public class AbilityService : MonoBehaviour
    {
        [SerializeField] private List<BaseAbility> abilities;
        private void Awake()
        {
            throw new NotImplementedException();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }

        private BaseAbility GetAbility(AbilityName abilityName)
        {
            foreach (BaseAbility ability in abilities)
            {
                if(ability.abilityName == abilityName)
                    return ability;
            }
            Debug.LogError("Ability not found: " + abilityName);
            return null;
        }
        
    }
}
