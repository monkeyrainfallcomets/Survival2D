using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomGroup<Type>
{
    [SerializeField] RandomObject<Type>[] values;
    public bool TrySelect(float random, out Type output)
    {
        for (int i = 0; i < values.Length; i++)
        {
            Type outputValue;
            if (values[i].TryGetValue(random, out outputValue))
            {
                output = outputValue;
                return true;
            }

        }
        output = default(Type);
        return false;
    }
}

[System.Serializable]
public class RandomObject<Type>
{
    [SerializeField] float chance;
    [SerializeField] Type value;
    public bool TryGetValue(float random, out Type output)
    {
        if (chance >= random)
        {
            output = value;
            return true;
        }
        output = default(Type);
        return false;
    }
}