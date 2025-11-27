using System.Collections;
using src.battle.entities;
using UnityEngine;

namespace src.battle.abilities
{
    [CreateAssetMenu(fileName = "BaseAbilitySO", menuName = "Scriptable Objects/BaseAbilitySO")]
    public class BaseAbility : ScriptableObject, IAbility
    {
        public AbilityName abilityName;
        //public Sprite icon;
        public int manaCost;
        public float cooldown;
        
        public bool CanUse(EntityInstance user)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator Execute(EntityInstance user, EntityInstance target)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public interface IAbility
    {
        bool CanUse(EntityInstance user);
        IEnumerator Execute(EntityInstance user, EntityInstance target);
    }
    
}


