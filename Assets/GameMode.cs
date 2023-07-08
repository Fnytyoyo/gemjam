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
    int currentLevelIdx;
    public Level[] Levels;
    public GameObject PlayerPrefab;

    public enum ActionType { Interaction, BuildMine, BuildSpikes, BuildCannon, BuildJumpPad };

    public ActionType currentAction { get; private set;  } = ActionType.Interaction;

    private Dictionary<string, ActionType> ActionInputMap = new Dictionary<string, ActionType>();

    void Start()
    {
        ActionInputMap.Add("0", ActionType.Interaction);
        ActionInputMap.Add("1", ActionType.BuildMine);
        ActionInputMap.Add("2", ActionType.BuildSpikes);
        ActionInputMap.Add("3", ActionType.BuildCannon);
        ActionInputMap.Add("4", ActionType.BuildJumpPad);

        currentLevelIdx = 0;
        LoadLevel(currentLevelIdx);
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

    public void LoadNextLevel()
    {
        currentLevelIdx++;
        if (currentLevelIdx < Levels.Length)
        {
            LoadLevel(currentLevelIdx);
        }
        else
        {
            //TODO: Victory screen
            UnityEngine.Debug.Log("Winner winner");
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var item in ActionInputMap)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    currentAction = item.Value;
                    break;
                }
            }
        }

        
    }

}