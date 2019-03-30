using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private AInventory inventory;
    public AInventory Inventory
    {
        get { return inventory; }
        private set { }
    }

    public int level;
    public int health;
    public bool inCube;

    [HideInInspector]
    public bool isCrafting;
    [HideInInspector]
    public bool isBuilding;

    public Transform outCubePoint;
    public StoryManager storyManager;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            inCube = true;
        }
        else
        {
            inCube = false;
        }
        isCrafting = false;
        isBuilding = false;
    }

    private void Start()
    {
        inventory = GetComponent<AInventory>();
        if(InGameInformationHolder.dataSave != null)
        {
            LoadPlayer(InGameInformationHolder.dataSave.playerData);
            LoadPlayerInventory(InGameInformationHolder.dataSave.playerData);
        }
        else
        {
            level = 1;
            health = 100;
            inventory.AddElementToInventory("maple_log", 20);
            inventory.AddElementToInventory("stone", 20);
        }
        
    }

    private void LoadPlayer(PlayerData playerSave)
    {
        if(inventory == null)
        {
            inventory = GetComponent<AInventory>();
        }

        level = playerSave.level;
        health = playerSave.health;

        if (InGameInformationHolder.gettingOutFromCube)
        {
            transform.position = outCubePoint.position;
            transform.rotation = outCubePoint.rotation;
            InGameInformationHolder.gettingOutFromCube = false;
            if(InGameInformationHolder.hasGetScroll)
            {
                if(storyManager.currentStoryLevel < storyManager.storyFragments.Count)
                storyManager.currentStoryLevel++;
            }
        }
        else
        {
            StartCoroutine(LoadSavedPosition(playerSave));
        }

    }

    private IEnumerator LoadSavedPosition(PlayerData playerSave)
    {
        yield return new WaitForSeconds(0.05f);

        if(!inCube)
        {
            transform.position = new Vector3(playerSave.position[0], playerSave.position[1], playerSave.position[2]);
            transform.rotation = Quaternion.Euler(new Vector3(playerSave.rotation[0], playerSave.rotation[1], playerSave.rotation[2]));
        }
    }

    private void LoadPlayerInventory(PlayerData playerSave)
    {
        inventory.LoadInventory(playerSave.inventory);
    }


}
