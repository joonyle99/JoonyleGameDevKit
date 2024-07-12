using UnityEngine;

public class SingletonManager : JoonyleGameDevKit.SingletonBehavior<SingletonManager>
{
    public void TestFunction()
    {
        Debug.Log("Test Function");
    }
}
