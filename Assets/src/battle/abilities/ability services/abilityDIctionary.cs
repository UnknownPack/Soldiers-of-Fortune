using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.battle.abilities
{

    [System.Serializable]
    public enum AbilityType
    {
        Tile_SIngular,
        Tile_Area,
        Enitty_Singular,
        Entity_Area
    }
    
    [System.Serializable]
    public enum AbilityName
    {
        BasicRangedAttack,
        BasicMeleeAttack,
        
    }
}
