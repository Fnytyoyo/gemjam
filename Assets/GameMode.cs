using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static GameMode;
using Debug = UnityEngine.Debug;

public class GameMode : MonoBehaviour
{
    public bool IsPaused { get; private set; }
    public bool IsInteractable => interactableCounter == 0;
    private int interactableCounter = 0;

    int currentLevelIdx;
    public Level[] Levels;
    public GameObject PlayerPrefab;

    public enum ActionType { Interaction, BuildMine, BuildSpikes, BuildCannon, BuildJumpPad };

    public ActionType currentAction { get; private set;  } = ActionType.Interaction;

    private readonly Dictionary<string, ActionType> actionInputMap = new Dictionary<string, ActionType>();

    void Start()
    {
        actionInputMap.Add("1", ActionType.Interaction);
        actionInputMap.Add("2", ActionType.BuildMine);
        actionInputMap.Add("3", ActionType.BuildSpikes);
        actionInputMap.Add("4", ActionType.BuildCannon);
        actionInputMap.Add("5", ActionType.BuildJumpPad);
        
        currentLevelIdx = 0;
        LoadCurrentLevel();
        Unpause();

        interactableCounter = 0;
    }

    void LoadLevel(int idx)
    {
        var currLevelGO = FindObjectOfType<Level>();
        Debug.Log(currLevelGO?.name);
        
        if(currLevelGO)
        {
            GameObject.Destroy(currLevelGO.gameObject);
        }

        var obj = Instantiate(Levels[idx]);

        Level newLevel = obj.GetComponent<Level>();

        RespawnPlayer(newLevel.GetStartPosition());

        var levelBounds = newLevel.GetBounds();
        Vector3 newCameraPos = levelBounds.center;
        newCameraPos.z = -1;
        Camera.main.transform.position = newCameraPos;
    }

    void RespawnPlayer(Vector3 position)
    {
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        if (player)
        {
            Destroy(player.gameObject);
        }

        Instantiate(PlayerPrefab, position, Quaternion.identity);
    }

    public void NextLevel()
    {
        currentLevelIdx++;
        if (currentLevelIdx < Levels.Length)
        {
            GameObject.FindObjectOfType<NextLevelPanel>(true).gameObject.SetActive(true);
        }
        else
        {
            GameObject.FindObjectOfType<VictoryScreen>(true).gameObject.SetActive(true);
        }
        interactableCounter++;
    }

    public void LoadCurrentLevel()
    {
        interactableCounter--;
        LoadLevel(currentLevelIdx);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        if (!IsInteractable)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            foreach (var item in actionInputMap)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    currentAction = item.Value;
                    UnityEngine.Debug.Log("Action changed to: " + currentAction);
                    break;
                }
            }
        }
    }

    public void Unpause()
    {
        IsPaused = false;
        interactableCounter--;
        Time.timeScale = 1;
        GameObject.FindObjectOfType<PauseMenu>(true).gameObject.SetActive(false);
    }

    public void Pause()
    {
        IsPaused = true;
        interactableCounter++;
        Time.timeScale = 0;
        GameObject.FindObjectOfType<PauseMenu>(true).gameObject.SetActive(true);
    }

}