using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public GameObject structuresParent;
    public GameObject chestPrefab;
    public GameObject workTablePrefab;
    public GameObject furnacePrefab;

	// Use this for initialization
	void Awake ()
    {
        if(InGameInformationHolder.dataSave != null)
        {
            LoadWorld();
        }   
    }

    private void LoadWorld()
    {
        LoadResourcePoints();
        LoadChests();
        LoadWorkTables();
        LoadFurnaces();
    }

    private void LoadResourcePoints()
    {
        SaveData save = InGameInformationHolder.dataSave;
        ResourcePoint[] rpsList = FindObjectsOfType<ResourcePoint>();
        foreach (ResourcePointData rpData in save.worldData.resourcePointsData)
        {
            foreach (ResourcePoint rp in rpsList)
            {
                if (rpData.position[0] == rp.transform.position.x &&
                    rpData.position[1] == rp.transform.position.y &&
                    rpData.position[2] == rp.transform.position.z)
                {
                    rp.LoadResourcePoint(rpData);
                    break;
                }
            }
        }
    }

    private void LoadChests()
    {
        SaveData save = InGameInformationHolder.dataSave;
        ChestData[] chests = save.worldData.structures.chests;
        foreach(ChestData chest in chests)
        {
            GameObject chestGO = Instantiate(chestPrefab);
            chestGO.transform.SetParent(structuresParent.transform, false);
            chestGO.transform.position = new Vector3(chest.position[0], chest.position[1], chest.position[2]);
            chestGO.transform.eulerAngles = new Vector3(chest.rotation[0], chest.rotation[1], chest.rotation[2]);
            chestGO.GetComponent<AInventory>().LoadInventory(chest.inventory);
        }
    }

    private void LoadWorkTables()
    {
        SaveData save = InGameInformationHolder.dataSave;
        WorkTableData[] tables = save.worldData.structures.workTables;
        foreach(WorkTableData table in tables)
        {
            GameObject tableGO = Instantiate(workTablePrefab);
            tableGO.transform.SetParent(structuresParent.transform, false);
            tableGO.transform.position = new Vector3(table.position[0], table.position[1], table.position[2]);
            tableGO.transform.eulerAngles = new Vector3(table.rotation[0], table.rotation[1], table.rotation[2]);
        }
    }
    
    private void LoadFurnaces()
    {
        SaveData save = InGameInformationHolder.dataSave;
        FurnaceData[] furnaces = save.worldData.structures.furnaces;
        foreach(FurnaceData furnaceData in furnaces)
        {
            GameObject furnaceGO = Instantiate(furnacePrefab);
            furnaceGO.transform.SetParent(structuresParent.transform, false);
            furnaceGO.transform.position = new Vector3(furnaceData.position[0], furnaceData.position[1], furnaceData.position[2]);
            furnaceGO.transform.eulerAngles = new Vector3(furnaceData.rotation[0], furnaceData.rotation[1], furnaceData.rotation[2]);
            furnaceGO.GetComponent<Furnace>().LoadFurnace(furnaceData);
        }
    }

}
