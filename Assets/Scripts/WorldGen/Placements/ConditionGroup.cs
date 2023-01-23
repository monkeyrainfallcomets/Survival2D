using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionGroup
{
    ConditionClump[] conditionGroups;
}

[System.Serializable]
public class Condition
{
    [SerializeField] RangeF range;
    [SerializeField] ConditionType condition;
    public bool ConditionsMet(float random, NoiseValue noiseValue)
    {
        switch (condition)
        {
            case ConditionType.Heat:
                return range.WithinRange(noiseValue.heatValue);
            case ConditionType.Height:
                return range.WithinRange(noiseValue.heightValue);
            case ConditionType.Moisture:
                return range.WithinRange(noiseValue.moistureValue);
            case ConditionType.Random:
                return range.WithinRange(random);
        }
        return false;
    }
}

public class ConditionalObject<T>
{
    Condition[] conditions;
    T value;
    public bool TryGetValue(out T value, float random, NoiseValue noiseValue)
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].ConditionsMet(random, noiseValue))
            {
                value = default(T);
                return false;
            }
        }
        value = this.value;
        return true;
    }
}

[System.Serializable]
public class ConditionClump
{
    Condition[] conditions;
}

public enum ConditionType
{
    Heat,
    Moisture,
    Height,
    Random
}