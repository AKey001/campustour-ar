using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadSceneIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneName(string scene) {
        SceneManager.LoadScene(scene);
    }
}
