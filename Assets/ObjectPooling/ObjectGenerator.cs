using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectPoolingBehavior objectPooling;

    private void Start()
    {
        StartCoroutine(GenerateCoroutine_ObjectPooling());
    }

    public IEnumerator GenerateCoroutine_ObjectPooling()
    {
        objectPooling.CreatePool();

        while (true)
        {
            GameObject obj = objectPooling.GetPooledObject();
            obj.SetActive(true);
            obj.transform.position = transform.position;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
