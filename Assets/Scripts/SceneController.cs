using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //load the next scene
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //when player press the quit button to quit game
    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
