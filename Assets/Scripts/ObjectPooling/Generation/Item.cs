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
            // ���̾��Ű �� �����ϴ� ��Ʈ ������Ʈ �߿��� ���� �������� ��ġ�ϵ��� ����
            var currentScene = SceneManager.GetActiveScene();
            var rootCount = currentScene.rootCount;
            transform.SetSiblingIndex(rootCount - 1);
#endif
        }

        _fromGenerator.NowCount--;
        _fromGenerator.ObjectPoolingBase.ReturnObject(this);
    }
}
