using System;
using System.Collections.Generic;
using Src.Backend;
using UnityEngine;

namespace src.battle.abilities
{
    public class AbilityService : MonoBehaviour
    {
        [SerializeField] private List<BaseAbility> abilities;
        private void Awake()
        {
            ServiceLocator.Register(this);
        } 

        public BaseAbility GetAbility(AbilityName abilityName)
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
