using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPanel : MonoBehaviour
{
    public void NextLevel()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameMode>().LoadCurrentLevel();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
