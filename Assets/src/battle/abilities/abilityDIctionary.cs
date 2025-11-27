using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.battle.abilities
{

    [System.Serializable]
    public enum AbilityType
    {
        Singular,
        AreaOfEffect,
    }
    
    [System.Serializable]
    public enum AbilityName
    {
        BasicRangedAttack,
        BasicMeleeAttack,
        
    }
}
