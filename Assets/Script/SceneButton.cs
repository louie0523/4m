using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterTheIngame : MonoBehaviour
{
    public string sceneToLoad;
    public Button start;

    void Start()
    {
        start.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnDestroy()
    {
        if (start != null)
        {
            start.onClick.RemoveListener(OnButtonClick);
        }
    }
}
