using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TypeMatchups
{
    [SerializeField] SerializableDictionary<DamageTypes, Entity.TypeMultipliers> types;
    public Effectiveness GetEffectiveness(DamageTypes damageType)
    {
        return types[damageType].effectiveness;
    }

    public float DamageMultiplier(DamageTypes damageType)
    {
        return types[damageType].multiplier;
    }

}
