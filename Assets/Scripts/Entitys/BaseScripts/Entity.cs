using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Entity : MonoBehaviour
{
    [SerializeField] SpriteRenderer mainRenderer;
    [SerializeField] Stats baseStats;
    [SerializeField] TypeMatchups baseTypeMatchups;
    [SerializeField] MovementStates baseMovementStates;
    TypeMatchups typeMatchups;
    Stats stats;
    MovementStates movementStates;
    MovementState currentMovementState;
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
    protected void DealDamage(Entity entity, float damage, DamageTypes damageType)
    {
        entity.TakeDamage(damage * stats.attack, damageType);
    }

    protected void Move(Vector2 direction, float speedMultiplier)
    {
        currentMovementState.movement.Move(direction, stats.speed * speedMultiplier);
    }

    protected void UpdateEntity()
    {
        stats = ApplyStatModifiers(baseStats);
        movementStates = UpdateMovementStates();
        typeMatchups = UpdateTypeMatchups();
    }

    protected virtual Stats ApplyStatModifiers(Stats baseStats)
    {
        return baseStats;
    }

    protected virtual MovementStates UpdateMovementStates()
    {
        return baseMovementStates;
    }

    protected virtual TypeMatchups UpdateTypeMatchups()
    {
        return baseTypeMatchups;
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

    public float DamageMultiplier(DamageTypes damageType)
    {
        for (int i = 0; i < typeMatchups.weaknesses.Length; i++)
        {
            if (typeMatchups.weaknesses[i].type == damageType)
            {
                return typeMatchups.weaknesses[i].multiplier;
            }
        }

        for (int i = 0; i < typeMatchups.resistances.Length; i++)
        {
            if (typeMatchups.resistances[i].type == damageType)
            {
                return 1 / typeMatchups.resistances[i].multiplier;
            }
        }

        for (int i = 0; i < typeMatchups.immunities.Length; i++)
        {
            if (typeMatchups.immunities[i] == damageType)
            {
                return 0f;
            }
        }
        return 1f;
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

    public bool IsVeryEffective(DamageTypes damageType)
    {
        for (int i = 0; i < typeMatchups.resistances.Length; i++)
        {
            if (typeMatchups.immunities[i] == damageType)
            {
                return true;
            }
        }
        return false;
    }

    public bool Resists(DamageTypes damageType)
    {
        for (int i = 0; i < typeMatchups.resistances.Length; i++)
        {
            if (typeMatchups.immunities[i] == damageType)
            {
                return true;
            }
        }
        return false;
    }

    public bool TrySwapMovementState(MovementType movementType)
    {
        for (int i = 0; i < movementStates.states.Length; i++)
        {
            if (movementStates.states[i].type == movementType)
            {
                currentMovementState = movementStates.states[i];
                return true;
            }
        }
        return false;
    }

    public MovementType GetCurrentState()
    {
        return currentMovementState.type;
    }

    [System.Serializable]
    public struct Stats
    {
        public float maxHp;
        public float defense;
        public float attack;
        public float maxStamina;
        public float speed;
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

    public struct MovementState
    {
        public MovementType type;
        public Movement movement;
    }

    public struct MovementStates
    {
        public MovementState baseState;
        public MovementState[] states;
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

public enum MovementType
{
    Swim,
    Walk
}
