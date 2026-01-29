using UnityEngine;
using System.Collections;

namespace JoonyleGameDevKit
{
    public class ItemGenerator : MonoBehaviour
    {
        [Header("Item Generator")]
        [Space]

        [SerializeField] protected Transform _spawnPoint;
        public Transform SpawnPoint => _spawnPoint;

        [Space]

        [SerializeField] protected float intervalTime = 1f;
        [SerializeField] protected int maxCount = 10;
        [SerializeField] protected int nowCount;

        public int NowCount
        {
            get => nowCount;
            internal set => nowCount = value;
        }

        protected ObjectPooling objectPooling;
        public ObjectPooling ObjectPooling => objectPooling;

        private WaitForSeconds _waitInterval;

        protected virtual void Awake()
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = transform;
            }

            objectPooling = GetComponent<ObjectPooling>();

            _waitInterval = new WaitForSeconds(intervalTime);
        }

        private void Start()
        {
            StartCoroutine(GenerateCoroutine());
        }

        protected virtual IEnumerator GenerateCoroutine()
        {
            while (true)
            {
                if (nowCount >= maxCount)
                {
                    yield return _waitInterval;
                    continue;
                }

                var item = ObjectPooling.GetObject<ItemBase>();

                if (item != null)
                {
                    item.OnGenerated(this);

                    item.transform.position = _spawnPoint.position;
                    item.transform.rotation = _spawnPoint.rotation;

                    yield return _waitInterval;
                }
                else
                {
                    Debug.LogWarning($"item is invalid\n{StackTraceUtility.ExtractStackTrace()}");
                    yield break;
                }
            }
        }
    }

}
