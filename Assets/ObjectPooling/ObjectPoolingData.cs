using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolingData", menuName = "Scriptable Object/Object Pooling", order = 1)]
public class ObjectPoolingData : ScriptableObject
{
    [Header("Object Pooling Setting")]
    [Space]

    public GameObject poolingItem;
    public int poolCapacity;
}
