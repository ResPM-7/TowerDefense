using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    //SceneChangeManager DontDestroyOnLoad(gameObject);의 싱글톤으로 부서지지않게 되어있어서 타이틀씬으로 돌아올때 onclick이 되도록 하기위해서 구현
    [SerializeField] private Button playButton;

    private void Start()
    {
        if (playButton != null && SceneChangeManager.instance != null)
        {
            playButton.onClick.RemoveAllListeners();

            playButton.onClick.AddListener(() => SceneChangeManager.instance.LoadScene(1));
        }
    }
}
