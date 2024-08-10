using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour, IGeneratable
{
    private ItemGenerator _fromGenerator;
    public ItemGenerator FromGenerator => _fromGenerator;

    public void OnGenerated(ItemGenerator itemGenerator)
    {
        _fromGenerator = itemGenerator;
        _fromGenerator.NowCount++;
    }
    public void OnEliminated()
    {
        if (transform.parent)
        {
            transform.SetParent(null);

#if UNITY_EDITOR
            // 하이어라키 상에 존재하는 루트 오브젝트 중에서 가장 마지막에 위치하도록 설정
            var currentScene = SceneManager.GetActiveScene();
            var rootCount = currentScene.rootCount;
            transform.SetSiblingIndex(rootCount - 1);
#endif
        }

        _fromGenerator.NowCount--;
        _fromGenerator.ObjectPoolingBase.ReturnObject(this);
    }
}
