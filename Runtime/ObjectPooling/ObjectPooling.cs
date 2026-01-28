using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    [Header("Object Pooling")]
    [Space]

    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private string _objectName = "Object";
    [SerializeField] private int _poolCapacity = 100;

    private Stack<GameObject> _objectPool = new();
    private Transform _poolContainer;
    private int _lastIndex;

    public int Count => _objectPool.Count;
    public int Capacity => _poolCapacity;

    public bool IsEmpty() => _objectPool.Count == 0;

    private void Awake()
    {
        CreatePoolContainer();
        CreatePool();
    }

    private void CreatePoolContainer()
    {
        var containerObj = new GameObject($"[Pool] {_objectName}");
        _poolContainer = containerObj.transform;
        _poolContainer.SetParent(transform);
    }
    private void CreatePool()
    {
        for (int i = 0; i < _poolCapacity; i++)
        {
            CreateNewObject();
        }
    }
    private void ExtendPool()
    {
        int extensionCount = _poolCapacity / 2;
        _poolCapacity += extensionCount;

        for (int i = 0; i < extensionCount; i++)
        {
            CreateNewObject(isExtended: true);
        }
    }
    private void CreateNewObject(bool isExtended = false)
    {
        var newObj = Instantiate(_objectPrefab, Vector3.zero, Quaternion.identity, _poolContainer);

        _lastIndex++;
        string suffix = isExtended ? " (Extended)" : string.Empty;
        newObj.name = $"{_objectName} {_lastIndex}{suffix}";
        newObj.SetActive(false);

        _objectPool.Push(newObj);
    }
    public T GetObject<T>() where T : Component
    {
        if (_objectPool.Count == 0)
        {
            ExtendPool();
        }

        var obj = _objectPool.Pop();
        obj.SetActive(true);

        if (obj.TryGetComponent<T>(out var component))
        {
            return component;
        }

        Debug.LogWarning($"[ObjectPooling] Component '{typeof(T).Name}' not found on '{obj.name}'");
        return null;
    }
    public void ReturnObject(Component component)
    {
        var obj = component.gameObject;
        obj.SetActive(false);
        obj.transform.SetParent(_poolContainer);
        _objectPool.Push(obj);
    }

    public void DebugObjectPool()
    {
#if UNITY_EDITOR
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"ObjectPool ({_objectPool.Count}/{_poolCapacity})");
        sb.AppendLine();

        foreach (var obj in _objectPool)
        {
            sb.AppendLine(obj.name);
        }

        Debug.Log(sb.ToString());
#endif
    }
}
