using JoonyleGameDevKit;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private ObjectPoolingBase _pooling;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pooling.DebugObjectPool();
        }
    }

    void Start()
    {

    }

    private void OnDrawGizmos()
    {

    }
}
