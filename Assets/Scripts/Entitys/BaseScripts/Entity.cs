using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Entity : MonoBehaviour
{
    [SerializeField] SpriteRenderer mainRenderer;
    [SerializeField] Stats baseStats;
    [SerializeField] TypeMatchups baseTypeMatchups;
    TypeMatchups typeMatchups;
    Stats stats;

    float stamina = 0;
    float currentHp = 0;

    void Start()
    {
        UpdateEntity();
        stamina = stats.maxStamina;
        currentHp = stats.maxHp;
    }

    public void TakeDamage(float damage, DamageTypes damageType)
    {
        currentHp -= damage / stats.defense;
    }
    public void DealDamage(Entity entity, float damage, DamageTypes damageType)
    {
        entity.TakeDamage(damage * stats.attack, damageType);
    }

    protected void UpdateEntity()
    {
        ApplyStatModifiers(baseStats);
    }

    protected virtual Stats ApplyStatModifiers(Stats baseStats)
    {
        return baseStats;
    }
    public Effectiveness GetEffectiveness(DamageTypes damageType)
    {
        for (int i = 0; i < typeMatchups.weaknesses.Length; i++)
        {
            if (typeMatchups.weaknesses[i].type == damageType)
            {
                return Effectiveness.VeryEffective;
            }
        }

        for (int i = 0; i < typeMatchups.resistances.Length; i++)
        {
            if (typeMatchups.resistances[i].type == damageType)
            {
                return Effectiveness.NotVeryEffective;
            }
        }

        for (int i = 0; i < typeMatchups.immunities.Length; i++)
        {
            if (typeMatchups.immunities[i] == damageType)
            {
                return Effectiveness.NoEffect;
            }
        }

        return Effectiveness.Effective;
    }
    public bool IsImmune(DamageTypes damageType)
    {
        for (int i = 0; i < typeMatchups.immunities.Length; i++)
        {
            if (typeMatchups.immunities[i] == damageType)
            {
                return true;
            }
        }
        return false;
    }


    [System.Serializable]
    public struct Stats
    {
        public float maxHp;
        public float defense;
        public float attack;
        public float maxStamina;
    }
    [System.Serializable]
    public struct TypeMultipliers
    {
        public DamageTypes type;
        public float multiplier;
    }
    [System.Serializable]
    public struct TypeMatchups
    {
        public TypeMultipliers[] weaknesses;
        public TypeMultipliers[] resistances;
        public DamageTypes[] immunities;
    }
}


public enum DamageTypes
{
    Fire,
    Water,
    Nature,
    Poison,
    Physical
}

public enum Effectiveness
{
    VeryEffective,
    Effective,
    NotVeryEffective,
    NoEffect

}