using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratableObject : MonoBehaviour, IGeneratable
{
    private ObjectGenerator _fromGenerator;
    public ObjectGenerator FromGenerator => _fromGenerator;

    private Coroutine _eliminateCoroutine;

    private void OnEnable()
    {
        if (_eliminateCoroutine != null)
        {
            StopCoroutine(_eliminateCoroutine);
            _eliminateCoroutine = null;
        }

        _eliminateCoroutine = StartCoroutine(EliminateCoroutine());
    }

    private IEnumerator EliminateCoroutine()
    {
        yield return new WaitForSeconds(5f);

        OnEliminated();
    }

    public void OnGenerated(ObjectGenerator objectGenerator)
    {
        _fromGenerator = objectGenerator;
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

        _fromGenerator.ObjectPoolingBase.ReturnObject(this);
        _fromGenerator.NowCount--;
    }
}
