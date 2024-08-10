using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{
    [Header("━━━━━━━━ Object Generator ━━━━━━━━")]
    [Space]

    [SerializeField] protected Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;

    [Space]

    [SerializeField] protected float intervalTime = 1f;
    [SerializeField] protected int maxCount = 10;
    [SerializeField] protected int nowCount;                            // 활성화되어 사용되고 있는 오브젝트의 개수
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
            // 최대치에 도달하면 대기
            if (nowCount >= maxCount)
            {
                yield return new WaitForSeconds(intervalTime);
                continue;
            }

            var generatedObj = ObjectPoolingBase.GetObject<GeneratableObject>();

            if (generatedObj != null)
            {
                generatedObj.OnGenerated(this);

                // ** init **
                generatedObj.transform.position = _spawnPoint.position;
                generatedObj.transform.rotation = _spawnPoint.rotation;
                generatedObj.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // wait
                yield return new WaitForSeconds(intervalTime);
            }
            else
            {
                Debug.LogWarning($"generated object is invalid\n{StackTraceUtility.ExtractStackTrace()}");
                yield break;
            }

            yield return null;
        }
    }
}
