using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    public string levelToLoad = "";

    private void OnTriggerEnter(Collider other)
    {
        if (!levelToLoad.Equals(""))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
