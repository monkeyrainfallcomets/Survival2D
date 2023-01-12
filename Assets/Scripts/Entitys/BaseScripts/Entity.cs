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

    public TypeMatchups GetTypeMatchups()
    {
        return typeMatchups;
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
        public Effectiveness effectiveness;
        public float multiplier;
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
    Physical,
    None
}

public enum Effectiveness
{
    ExtremelyEffective = 4,
    VeryEffective = 3,
    Effective = 2,
    NotVeryEffective = 1,
    BarelyEffective = 0
}

public enum MovementType
{
    Swim,
    Walk
}
