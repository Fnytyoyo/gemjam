using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static GameMode;

public class GameMode : MonoBehaviour
{
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
    }

    void Update()
    {
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
}