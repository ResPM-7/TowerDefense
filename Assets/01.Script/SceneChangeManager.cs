using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex=1;

    public void LoadScene()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void LoadSceneOptimized()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }    

    private IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(gameSceneIndex);

        while (op != null)
        {
            yield return null;
        }
    }
}
