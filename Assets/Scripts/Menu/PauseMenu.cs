using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Continue()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameMode>().Unpause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
