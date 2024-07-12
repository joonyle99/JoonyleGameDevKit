using UnityEngine;

public class SingletonManagerInvoker : MonoBehaviour
{
    void Start()
    {
        SingletonManager.Instance.TestFunction();
    }
}
