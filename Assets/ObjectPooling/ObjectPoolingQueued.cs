using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingQueued : ObjectPoolingBase
{
    public Queue<GameObject> ObjectPool { get; private set; } = new();

    protected override void AddObject(GameObject obj)
    {
        ObjectPool.Enqueue(obj);
    }
    protected override GameObject RemoveLast()
    {
        return ObjectPool.Dequeue();
    }

    public override bool IsEmptyPool()
    {
        return ObjectPool.Count == 0;
    }

    public T PeekPool<T>()
    {
        return ObjectPool.Peek().GetComponent<T>();
    }
    public T DequeuePool<T>()
    {
        return ObjectPool.Dequeue().GetComponent<T>();
    }

#if UNITY_EDITOR
    public override void DebugObjectPool()
    {
        string str = $"ObjectPoolQueued ({ObjectPool.Count})\n\n";
        foreach (var obj in ObjectPool)
        {
            str += obj.name + '\n';
        }
        Debug.Log(str);
        Debug.Log("=====================");
    }
#endif
}
