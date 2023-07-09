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
    public bool IsInteractable => interactableBlockCounter == 0;
    private int interactableBlockCounter = 0;

    private int currentStoryIdx;
    private bool isStoryPanelDisplayed;
    private float storyTimer;

    int currentLevelIdx;
    public Level[] Levels;
    public GameObject PlayerPrefab;

    public int buildingRotation { get; private set; }
    public int buildingRotationsCount { get; private set; }

    public enum ActionType { Interaction, BuildMine, BuildSpikes, BuildCannon, BuildJumpPad };

    public ActionType currentAction { get; private set; } = ActionType.Interaction;

    private readonly Dictionary<string, ActionType> actionInputMap = new Dictionary<string, ActionType>();
    public ActionType ShortcutStringToAction(string shortcutString) => actionInputMap[shortcutString];

    private Dictionary<string, int> inventoryItemsLeft = new Dictionary<string, int>();

    public int GetItemsLeft(string contraptionName)
    {
        return inventoryItemsLeft.ContainsKey(contraptionName) ? inventoryItemsLeft[contraptionName] : 0;
    }

    public void ChangeItemCount(string contraptionName, int delta)
    {
        if (inventoryItemsLeft.ContainsKey(contraptionName) == false)
        {
            Debug.LogError($"No contraption of this type: {contraptionName}");
            return;
        }

        inventoryItemsLeft[contraptionName] += delta;

        if (inventoryItemsLeft[contraptionName] < 0)
        {
            Debug.LogError($"We've built more contraptions: {contraptionName} than we had. WHY?");
        }
    }

    void Start()
    {
        actionInputMap.Add("1", ActionType.Interaction);
        actionInputMap.Add("2", ActionType.BuildMine);
        actionInputMap.Add("3", ActionType.BuildSpikes);
        actionInputMap.Add("4", ActionType.BuildCannon);
        actionInputMap.Add("5", ActionType.BuildJumpPad);

        currentLevelIdx = 0;

        interactableBlockCounter = 0;
        LoadLevel(currentLevelIdx);

        buildingRotation = 0;
        buildingRotationsCount = 4;
    }

    void LoadLevel(int idx)
    {
        interactableBlockCounter++;

        var currLevelGO = FindObjectOfType<Level>();

        if (currLevelGO)
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

        inventoryItemsLeft.Clear();
        foreach (var item in newLevel.Inventory)
        {
            inventoryItemsLeft.Add(item.ContraptionName, item.Count);
        }

        FindObjectOfType<Inventory>().SetupInventory(newLevel.Inventory);

        FindObjectOfType<Inventory>().gameObject.SetActive(false);

        currentStoryIdx = 0;
        if (newLevel.Story.Count == 0)
        {
            interactableBlockCounter--;
            GameObject.FindObjectOfType<Inventory>(true).gameObject.SetActive(true);
        }
        else
        {
            storyTimer = newLevel.Story[0].WaitBefore;
        }
        isStoryPanelDisplayed = false;
    }

    public bool IsInBuildingMode()
    {
        return currentAction == ActionType.BuildCannon || currentAction == ActionType.BuildMine ||
               currentAction == ActionType.BuildSpikes || currentAction == ActionType.BuildJumpPad;
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
        interactableBlockCounter++;
    }

    public void LoadCurrentLevel()
    {
        interactableBlockCounter--;
        LoadLevel(currentLevelIdx);
    }

    void Update()
    {
        if (currentLevelIdx >= Levels.Length )
        {
            return;
        }

        if (currentStoryIdx < Levels[currentLevelIdx].Story.Count)
        {
            if (isStoryPanelDisplayed)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentStoryIdx++;
                    isStoryPanelDisplayed = false;

                    if (currentStoryIdx < Levels[currentLevelIdx].Story.Count)
                    {
                        storyTimer = Levels[currentLevelIdx].Story[currentStoryIdx].WaitBefore;
                    }
                    else
                    {
                        GameObject.FindObjectOfType<Inventory>(true).gameObject.SetActive(true);
                        interactableBlockCounter--;
                    }

                    GameObject.FindObjectOfType<StoryArea>(true).gameObject.SetActive(false);
                }
            }
            else
            {
                storyTimer -= Time.deltaTime;

                if (storyTimer < 0 || Input.GetKeyDown(KeyCode.Space))
                {
                    var sa = GameObject.FindObjectOfType<StoryArea>(true);
                    sa.gameObject.SetActive(true);
                    sa.SetText(Levels[currentLevelIdx].Story[currentStoryIdx].StoryText);
                    isStoryPanelDisplayed = true;
                }
            }

            return;
        }

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddToBuildingRotation(-1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddToBuildingRotation(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ResetLevel();
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
                    string contraptionName = ContraptionInteractionDispatcher.ToTileString(item.Value);
                    if (contraptionName != "") // Do not check if interaction
                    {
                        if (Levels[currentLevelIdx].Inventory.Find(e => e.ContraptionName == contraptionName).Count == 0)
                        {
                            continue;
                        }
                    }
                    currentAction = item.Value;
                    UnityEngine.Debug.Log("Action changed to: " + currentAction);
                    break;
                }
            }
        }
    }

    public void AddToBuildingRotation(int ToAdd)
    {
        buildingRotation = (buildingRotation + ToAdd) % buildingRotationsCount;
        buildingRotation = buildingRotation < 0 ? buildingRotation + buildingRotationsCount : buildingRotation;
        Debug.Log(buildingRotation);
    }

    public void Unpause()
    {
        IsPaused = false;
        interactableBlockCounter--;
        Time.timeScale = 1;
        GameObject.FindObjectOfType<PauseMenu>(true).gameObject.SetActive(false);
    }

    public void Pause()
    {
        IsPaused = true;
        interactableBlockCounter++;
        Time.timeScale = 0;
        GameObject.FindObjectOfType<PauseMenu>(true).gameObject.SetActive(true);
    }

    public bool CanBuildOn(Vector3Int gridCoords, int rotation)
    {
        var currLevel = Levels[currentLevelIdx];

        if (currLevel.contraptionTilemap.GetTile(gridCoords) == true)
        {
            return false;
        }

        if (currLevel.wallsTilemap.GetTile(gridCoords) == true)
        {
            return false;
        }

        {
            var neighbourCoords = gridCoords;

            switch (rotation)
            {
                case 0:
                    neighbourCoords.y -= 1;
                    break;
                case 1:
                    neighbourCoords.x += 1;
                    break;
                case 2:
                    neighbourCoords.y += 1;
                    break;
                case 3:
                    neighbourCoords.x += -1;
                    break;
                default:
                    Debug.Log("Unexpected rotation value.");
                    return false;
            }

            if (currLevel.wallsTilemap.GetTile(neighbourCoords) == false)
            {
                return false;
            }
        }

        return true;
    }

    void ResetLevel()
    {
        var currLevel = FindObjectOfType<Level>(); ;

        var dispatcher = currLevel.contraptionTilemap.GetComponent<ContraptionInteractionDispatcher>();
        if (dispatcher == null)
        {
            return;
        }

        dispatcher.RechargeAllContraptions();

        RespawnPlayer(currLevel.GetStartPosition());
    }
}