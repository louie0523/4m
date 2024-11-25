using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyStart : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
