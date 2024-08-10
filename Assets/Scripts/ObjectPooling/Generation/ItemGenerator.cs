using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour
{
    [Header("���������������� Item Generator ����������������")]
    [Space]

    [SerializeField] protected Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;

    [Space]

    [SerializeField] protected float intervalTime = 1f;
    [SerializeField] protected int maxCount = 10;
    [SerializeField] protected int nowCount;                            // Ȱ��ȭ�Ǿ� ���ǰ� �ִ� ������Ʈ�� ����
    public int NowCount
    {
        get => nowCount;
        set => nowCount = value;
    }

    protected ObjectPoolingBase objectPoolingBase;
    public ObjectPoolingBase ObjectPoolingBase => objectPoolingBase;

    protected virtual void Awake()
    {
        if (_spawnPoint == null)
            _spawnPoint = transform;

        objectPoolingBase = GetComponent<ObjectPoolingBase>();
    }
    private void Start()
    {
        StartCoroutine(GenerateCoroutine());
    }

    protected virtual IEnumerator GenerateCoroutine()
    {
        while (true)
        {
            // �ִ�ġ�� �����ϸ� ���
            if (nowCount >= maxCount)
            {
                yield return new WaitForSeconds(intervalTime);
                continue;
            }

            var item = ObjectPoolingBase.GetObject<Item>();

            if (item != null)
            {
                item.OnGenerated(this);

                // ** init **
                item.transform.position = _spawnPoint.position;
                item.transform.rotation = _spawnPoint.rotation;

                // wait
                yield return new WaitForSeconds(intervalTime);
            }
            else
            {
                Debug.LogWarning($"item is invalid\n{StackTraceUtility.ExtractStackTrace()}");
                yield break;
            }

            yield return null;
        }
    }
}
