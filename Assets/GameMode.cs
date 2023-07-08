using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static GameMode;

public class GameMode : MonoBehaviour
{
    public enum ActionType { Interaction, BuildMine, BuildSpikes, BuildCannon, BuildJumpPad };

    public ActionType currentAction { get; private set;  } = ActionType.Interaction;

    private Dictionary<string, ActionType> ActionInputMap = new Dictionary<string, ActionType>();

    void Start()
    {
        ActionInputMap.Add("1", ActionType.Interaction);
        ActionInputMap.Add("2", ActionType.BuildMine);
        ActionInputMap.Add("3", ActionType.BuildSpikes);
        ActionInputMap.Add("4", ActionType.BuildCannon);
        ActionInputMap.Add("5", ActionType.BuildJumpPad);
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
                    UnityEngine.Debug.Log("Action changed to: " + currentAction);
                    break;
                }
            }
        }
    }
}