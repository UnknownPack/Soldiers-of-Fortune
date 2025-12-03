using System.Collections;
using src.battle.entities;
using UnityEngine;

namespace src.battle.abilities
{
    [CreateAssetMenu(fileName = "BaseAbilitySO", menuName = "Scriptable Objects/AbilitySO/BaseAbilitySO")]
    public abstract class BaseAbility : ScriptableObject, IAbility
    {
        public AbilityName abilityName;
        //public Sprite icon;
        public int actionCost;
        public float cooldown; 
        public AbilityType abilityType;

        public abstract void Execute(AbilityInfoPackage abilityInfoPackage);
    }
    
    public interface IAbility
    { 
        void Execute(AbilityInfoPackage abilityInfoPackage);
    }
}




