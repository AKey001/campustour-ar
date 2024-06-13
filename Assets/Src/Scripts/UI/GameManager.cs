using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

   
    public void LoadScene(int index)
    {
        StartCoroutine(Load(index));
    }

    IEnumerator Load(int index)
    {
        transition.SetTrigger("Switch");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
    }

}
