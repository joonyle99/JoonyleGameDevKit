using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingListed : ObjectPoolingBase
{
    public List<GameObject> ObjectPool { get; private set; } = new();

    protected override void AddObject(GameObject obj)
    {
        ObjectPool.Add(obj);
    }
    protected override GameObject RemoveLast()
    {
        var obj = ObjectPool[^1];
        ObjectPool.RemoveAt(ObjectPool.Count - 1);
        return obj;
    }

    public override bool IsEmptyPool()
    {
        return ObjectPool.Count == 0;
    }

#if UNITY_EDITOR
    public override void DebugObjectPool()
    {
        string str = $"ObjectPoolListed ({ObjectPool.Count})\n\n";
        foreach (var obj in ObjectPool)
        {
            str += obj.name + '\n';
        }
        Debug.Log(str);
        Debug.Log("=====================");
    }
#endif
}
