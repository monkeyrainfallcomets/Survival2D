using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver
{
    [SerializeField] List<T> list;
    HashSet<T> hashSet;
    public void OnBeforeSerialize()
    {
        this.list = new List<T>(this.hashSet);
        this.hashSet = null;
    }

    public void OnAfterDeserialize()
    {
        foreach (T item in this.list)
        {
            this.hashSet.Add(item);
        }
    }
    public bool Add(T item)
    {
        return hashSet.Add(item);
    }

    public bool Contains(T item)
    {
        return hashSet.Contains(item);
    }

}
