using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void LoadSceneOptimized(int index)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(index));
    }    

    private IEnumerator LoadSceneAsyncCoroutine(int index)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(index);

        while (op != null)
        {
            yield return null;
        }
    }
}
