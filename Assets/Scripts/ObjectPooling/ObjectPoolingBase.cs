using JoonyleGameDevKit;
using UnityEngine;

public abstract class ObjectPoolingBase : MonoBehaviour
{
    [Header("━━━━━━━━ Object Pooling Base ━━━━━━━━")]
    [Space]

    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private string _objectName = "Object";
    [SerializeField] private int _poolCapacity = 100;

    private GameObject _lastObject;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        // 오브젝트 생성 후, 풀에 추가
        for (int i = 0; i < _poolCapacity; ++i)
        {
            var newObj = Instantiate(_objectPrefab, Vector3.zero, Quaternion.identity);

            newObj.gameObject.name = _objectName + " " + (i + 1).ToString();
            newObj.gameObject.SetActive(false);

            AddObject(newObj);

            _lastObject = newObj;
        }
    }
    private void ExtendPool()
    {
        // 기존 풀 용량의 절반만큼 확장
        var halfOfCapacity = _poolCapacity / 2;
        _poolCapacity += halfOfCapacity;

        for (int i = 1; i <= halfOfCapacity; i++)
        {
            // 마지막 오브젝트의 이름에서 숫자를 추출해 네임 태그 생성
            var lastNumber = _lastObject.name.ExtractNumber();
            var nameTag = (lastNumber + 1).ToString();

            CreateNewObject(nameTag);
        }
    }
    private void CreateNewObject(string nameTag)
    {
        var newObj = Instantiate(_objectPrefab, Vector3.zero, Quaternion.identity);

        newObj.gameObject.name = _objectName + " " + nameTag + " (Extended)";
        newObj.gameObject.SetActive(false);

        AddObject(newObj);

        _lastObject = newObj;
    }

    public T GetObject<T>() where T : Component
    {
        // 꺼낼 오브젝트가 없는 경우 풀을 확장한다
        if (IsEmptyPool())
            ExtendPool();

        var newObj = RemoveLast();
        newObj.SetActive(true);
        return newObj.GetComponent<T>();
    }
    public void ReturnObject(Component component)
    {
        var oldObj = component.gameObject;
        AddObject(oldObj);
        oldObj.SetActive(false);
    }

    public abstract bool IsEmptyPool();

    protected abstract void AddObject(GameObject obj);
    protected abstract GameObject RemoveLast();

#if UNITY_EDITOR
    public abstract void DebugObjectPool();
#endif
}
