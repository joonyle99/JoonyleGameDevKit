using System.Collections.Generic;
using UnityEngine;

namespace JoonyleGameDevKit
{
    public class ObjectPoolingBehavior : MonoBehaviour
    {
        [Header("Object Pooling Behavior")]
        [Space]

        public ObjectPoolingData objectData;
        public Queue<GameObject> objectPool;

        public void CreatePool()
        {
            objectPool = new Queue<GameObject>();

            // 오브젝트를 생성 후, 풀에 추가
            for (int i = 0; i < objectData.poolCapacity; ++i)
            {
                var obj = Instantiate(objectData.poolingItem, transform.position, Quaternion.identity);
                obj.gameObject.name = "Object " + (i + 1).ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                return obj;
            }
            else
            {
                // local function
                GameObject CreateNewObject(string nameTag = " 1")
                {
                    var newObj = Instantiate(objectData.poolingItem, transform.position, Quaternion.identity);
                    newObj.gameObject.name = "Object " + "new" + nameTag;
                    newObj.SetActive(false);
                    objectPool.Enqueue(newObj);
                    return newObj;
                }

                var newObj = CreateNewObject();

                for (int i = 1; i < objectData.poolCapacity / 2; i++)
                {
                    var nameTag = " " + (i + 1).ToString();
                    CreateNewObject(nameTag);
                }

                return newObj;
            }
        }
    }
}