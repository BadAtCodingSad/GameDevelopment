using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum ToolType
{
    View,
    Hammer,
    Pickaxe
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ExpansionManager expansionManager;
    public WorkerManager workerManager;
    public CameraController cam;
    public List<Transform> hexTiles = new List<Transform>();
    public List<HexTile> tilesWithWorkers = new List<HexTile>();
    public bool ready = false;
    public HexTile townTile = null;
    public HexTile selectedTile = null;
    public List<TerrainResource> terrainResources = new List<TerrainResource>();

    public GameObject Highlight;
    public GameObject ChangeHighlight;

    //UI
    [Header("UI")]
    public TextMeshProUGUI  tileType;
    public TMP_InputField inputField;
    public TextMeshProUGUI workerRateInfo;
    public TextMeshProUGUI populationDisplay;
    public GameObject buildInfoPrefab;
    public GameObject buildInfoContainer;
    public GameObject resourceRowPrefab;
    public GameObject workersMask;
    public Transform resourceContainer;

    public Sprite metalIcon, woodIcon, oilIcon, fishIcon;
    public Transform worldspaceCanvas;


    public GameObject fogDetect;
    private GameObject fogClearer; // Scale this
    private bool townTileSet = false;
    
    public TextMeshProUGUI pollutionAmount;
    public Slider pollutionBar;


    public float maxPollution;
    public float pollution;
    public int maxPopulation = 100;
    public int population = 100;
    public int energy;

    [Header("Resource")]
    public int numberOfFreeWorkers = 10000;
    public int numberofActions = 0;
    public int numberOfTurns = 0;
    public int populationLimitIncreasePerBuilding = 100;
    public float workerEfficiencyPerGym = 0.4f;
    public int metal;
    public int oil;
    public int wood;
    public int fish;
    public List<TileChanges> changes = new List<TileChanges>();

    [Header("Building")]
    public List<HexTile> residences = new List<HexTile>();
    public List<HexTile> gyms = new List<HexTile>();
    public List<Buildable> buildables = new List<Buildable>(); 
    public ToolType currentTool = ToolType.View; 
    public UIManager uiManager;
    public int noTowns=0;
    public int noSBuilding=0;
    public int objectivesDone=0;
    public int noReactor=0;


    Vector3 camBasePos;
    Vector3 tempVector;
    HexTile prevHex;

    // Turn Mec
    public static event Action OnTurnEnd;


    public List<HexTile> Q = new List<HexTile>();
    #region Singleton
    private void Awake()
    {
        if (instance == null)
            instance = this;
        ready = true;
    }
    #endregion

    private void Start()
    {
        //numberOfFreeWorkersText.text = numberOfFreeWorkers.ToString();
        DeleteRows();
        pollutionBar.maxValue = maxPollution;
        numberOfFreeWorkers = population;
        populationDisplay.text = population.ToString();
    }
    private void Update()
    {
        
        pollutionBar.value = pollution;
        
        pollutionAmount.text = (pollution / maxPollution * 100).ToString() + "%";
        if (townTile != null && !townTileSet)
        {
            Vector3 spawnPos = new Vector3(townTile.gameObject.transform.position.x, 0.5f, townTile.gameObject.transform.position.z);
            camBasePos = townTile.gameObject.transform.position;
            fogClearer = Instantiate(fogDetect, spawnPos, Quaternion.identity);
            townTileSet = true;
            expansionManager.fogClearer = fogClearer;
            MoveCamToLocation(camBasePos);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            MoveCamToLocation(camBasePos);
        }
        if (selectedTile != null && selectedTile != prevHex)
        {
            UpdateUI();
        }
        if (selectedTile == null)
        {
            worldspaceCanvas.transform.position = new Vector3(200, 200, 200);
            uiManager.HideChanges();
        }
        if ((pollution / maxPollution * 100) > 100)
        {
            //Game Over
            SceneManager.LoadScene("GameL");
        }
        if(Input.GetKeyDown(KeyCode.Escape))//Osama's Objective==4
        {
            SceneManager.LoadScene("GameW");
        }
        
    }
    private void UpdateUI() 
    {
        tileType.SetText(selectedTile.tileName);
        DeleteRows();
        AddRow(selectedTile.metal, metalIcon, "Metal");
        AddRow(selectedTile.wood, woodIcon, "Wood");
        AddRow(selectedTile.oil, oilIcon, "Oil");
        AddRow(selectedTile.fish, fishIcon, "Fish");
        if (selectedTile.isBuilt)
        {
            workersMask.GetComponentInChildren<TextMeshProUGUI>().text = selectedTile.GetComponent<HexTile>().buildable.info;
            workersMask.SetActive(true);
        }
        else
            workersMask.SetActive(false);
        workerRateInfo.text = selectedTile.getExtractionRate();
        foreach (Buildable buildable in buildables)
        {
            foreach (Buildable.TerrainType terrainType in buildable.buildableTerrains)
            {
                if (terrainType.ToString() == selectedTile.type.ToString())
                {
                    BuildInfo buildInfo = Instantiate(buildInfoPrefab, buildInfoContainer.transform).GetComponent<BuildInfo>();
                    buildInfo.metal.text = buildable.metalCost.ToString();
                    buildInfo.oil.text = buildable.oilCost.ToString();
                    buildInfo.wood.text = buildable.woodCost.ToString();
                    buildInfo.fish.text = buildable.fishCost.ToString();
                    buildInfo.icon.sprite = buildable.icon;
                    buildInfo.buffs.text = buildable.info;
                    buildInfo.buildingName.text = buildable.buildingName;
                    buildInfo.buildable = buildable;
                }
            }
        }
        prevHex = selectedTile;
        inputField.text = selectedTile.workersOnTile.ToString();
        foreach (TileChanges tile in changes) 
        {
            if (selectedTile == tile.affectedTile) 
            {
                if (tile.changeType == TileChanges.ChangeType.worker)
                {
                    int tmp = selectedTile.workersOnTile - selectedTile.workersOnTileLastTurn;
                    if (tmp != 0)
                    {
                        uiManager.ShowChange(tmp > 0 ? tmp + " workers will be assigned to this tile" : Mathf.Abs(tmp) + " workers will be removed from this tile");
                    }
                    else
                    {
                        uiManager.HideChanges();
                    }
                    break;
                }
                else 
                {
                    uiManager.ShowChange(tile.toBeBuilt.buildingName + " will be built on this tile");
                    break;
                }
            }
            uiManager.HideChanges();
        }
        

    }
    public void SelectedHexTile(HexTile hexTile) 
    {
        if (hexTile == null) {
            Debug.LogError("Bankai");
            return; }
        selectedTile = hexTile;

        MoveCamToLocation(hexTile.gameObject.transform.position);
        Vector3 canvasLocation = new Vector3(hexTile.gameObject.transform.position.x, 0.3f, hexTile.gameObject.transform.position.z);
        worldspaceCanvas.transform.position = canvasLocation;



    }
    public void SetTool(ToolType tool)
    {
        currentTool = tool;
    }
    
    public void MoveCamToLocation(Vector3 location) 
    {
        tempVector  = new Vector3(location.x + 3f, 4, location.z);
        cam.newPosition = tempVector;
    }
    public List<Transform> getNeighbours(Transform self) 
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in hexTiles) 
        {
             if (child == null || self == null) continue;
            if (child != self && Vector3.Distance(child.position, self.position) < 1) 
            {
                list.Add(child);
            }
        }
        return list;
    }
    public void BuildUpdate(Buildable buildable)
    {
        int n;
        //var isNumeric = int.TryParse(inputField.text, out n);
        TileChanges tileChanges = ScriptableObject.CreateInstance<TileChanges>();
        tileChanges.affectedTile = selectedTile;
        tileChanges.changeType = TileChanges.ChangeType.build;
        tileChanges.toBeBuilt = buildable;
        TileChanges changeToBeRemoved = null;
        foreach (TileChanges change in changes)
        {
            if (change.affectedTile == selectedTile) 
            {
                changeToBeRemoved = change;
            }
        }
        changes.Remove(changeToBeRemoved);
        changes.Add(tileChanges);
    }
   void AddRow(int amount, Sprite icon, string label)
    {
        if (amount <= 0) return; 
        GameObject row = Instantiate(resourceRowPrefab, resourceContainer);
        var rowUI = row.GetComponent<RowUI>();
        rowUI.SetData(icon, amount, label);
    }
    void DeleteRows(){
         foreach (Transform child in resourceContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in buildInfoContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateWorkers()
    {
        int n;
        var isNumeric = int.TryParse(inputField.text, out n);
        if (n > 6)
        {
            inputField.text = selectedTile.workersOnTile.ToString();
            return;
        }
        TileChanges tileChanges = ScriptableObject.CreateInstance<TileChanges>();
        tileChanges.affectedTile = selectedTile;
        tileChanges.changeType = TileChanges.ChangeType.worker;
        tileChanges.numberOfWorkersChanged = n - selectedTile.workersOnTile;

        TileChanges changeToBeRemoved = null;
        foreach (TileChanges change in changes)
        {
            if (change.affectedTile == tileChanges.affectedTile)
            {
                if (change.changeType == TileChanges.ChangeType.worker)
                {
                    //tileChanges.numberOfWorkersChanged += change.numberOfWorkersChanged;
                }
                else
                {
                    if (numberOfFreeWorkers - tileChanges.numberOfWorkersChanged >= 0)
                    { // confirm if user has enough workers before reversing existing build change
                        metal += change.toBeBuilt.metalCost;
                        wood += change.toBeBuilt.woodCost;
                        fish += change.toBeBuilt.fishCost;
                        oil += change.toBeBuilt.oilCost;
                        // if energy is consumed also revert it
                    }
                }
                changeToBeRemoved = change;
            }
        }

        if (numberOfFreeWorkers - tileChanges.numberOfWorkersChanged >= 0)
        {
            changes.Remove(changeToBeRemoved);
            if (changes.Count < 3)
            {
                selectedTile.workersOnTile += tileChanges.numberOfWorkersChanged;
                numberOfFreeWorkers -= tileChanges.numberOfWorkersChanged;

                int tmp = selectedTile.workersOnTile - selectedTile.workersOnTileLastTurn;
                if (tmp != 0)
                {
                    uiManager.ShowChange(tmp > 0 ? tmp + " workers will be assigned to this tile" : Mathf.Abs(tmp) + " workers will be removed from this tile");
                } else
                {
                    uiManager.HideChanges();
                }

                if (tileChanges.numberOfWorkersChanged != 0)
                {
                    changes.Add(tileChanges);
                    selectedTile.GetComponentInChildren<HighlightSystem>().ChangeHighlight.SetActive(true);
                    workerRateInfo.text = selectedTile.getExtractionRate();
                    // Highlight modified hex
                }
                else
                {
                    // Remove highlight from hex
                }
            }
            else
            {
                Debug.Log("Out of changes for this turn");
            }
        }
        else
        {
            Debug.Log("Need more workers");
        }
        selectedTile.RecalculateWorkerPositions();
        if (selectedTile.workersOnTile > 0)
        {
            tilesWithWorkers.Add(selectedTile);
        }
        else 
        {
            for(int i =0; i < tilesWithWorkers.Count; i++)
            {
                if (selectedTile == tilesWithWorkers[i]) 
                {
                    tilesWithWorkers.RemoveAt(i);
                }
            }
        }
    }

    // Button for this should be available only if selected tile is in changes.affectedTile
    public void revertChange()
    {
        foreach (TileChanges change in changes)
        {
            if (change.affectedTile == selectedTile)
            {
                if (change.changeType == TileChanges.ChangeType.worker)
                {
                    selectedTile.workersOnTile -= change.numberOfWorkersChanged;
                    numberOfFreeWorkers += change.numberOfWorkersChanged;
                }
                else
                {
                    metal += change.toBeBuilt.metalCost;
                    wood += change.toBeBuilt.woodCost;
                    fish += change.toBeBuilt.fishCost;
                    oil += change.toBeBuilt.oilCost;
                    // also revert energy if used to building
                }
            }
        }
    }
    public void BuildAction(Buildable toBeBuilt)
    {
        TileChanges tileChanges = ScriptableObject.CreateInstance<TileChanges>();
        tileChanges.affectedTile = selectedTile;
        tileChanges.changeType = TileChanges.ChangeType.build;
        tileChanges.toBeBuilt = toBeBuilt;

        int mCost = toBeBuilt.metalCost;
        int wCost = toBeBuilt.woodCost;
        int fCost = toBeBuilt.fishCost;
        int oCost = toBeBuilt.oilCost;

        TileChanges changeToBeRemoved = null;
        foreach (TileChanges change in changes)
        {
            if (change.affectedTile == tileChanges.affectedTile)
            {
                if (change.changeType == tileChanges.changeType)
                {
                    mCost -= change.toBeBuilt.metalCost;
                    wCost -= change.toBeBuilt.woodCost;
                    fCost -= change.toBeBuilt.fishCost;
                    oCost -= change.toBeBuilt.oilCost;
                }
                else
                {
                    if (metal >= mCost && wood >= wCost && fish >= fCost && oil >= oCost)
                    { // confirm enough available resources before reverting workers change
                        selectedTile.workersOnTile -= change.numberOfWorkersChanged;
                        numberOfFreeWorkers += change.numberOfWorkersChanged;
                    }
                }
                changeToBeRemoved = change;
            }

        }
        uiManager.ShowChange(toBeBuilt.buildingName + " will be built on this tile");
        if (metal >= mCost && wood >= wCost && fish >= fCost && oil >= oCost)
        {
            changes.Remove(changeToBeRemoved);

            selectedTile.GetComponentInChildren<HighlightSystem>().ChangeHighlight.SetActive(true);
        }
        else
        {
            Debug.Log("Need more resource");
            return;
        }

        if (changes.Count < 3)
        {
            if (metal >= mCost && wood >= wCost && fish >= fCost && oil >= oCost)
            {
                changes.Add(tileChanges);
                selectedTile.GetComponentInChildren<HighlightSystem>().ChangeHighlight.SetActive(true);
                // Highlight modified hex

                metal -= mCost;
                wood -= wCost;
                fish -= fCost;
                oil -= oCost;
            }
            else
            {
                Debug.Log("No enough resources");
            }
        }
        else
        {
            Debug.Log("Max Number of Changes reached!");
            // has to revert a change 
        }
    }

    public void Build(HexTile oldTile, Buildable targetBuildable)
    {
        numberOfFreeWorkers += oldTile.workersOnTile;
        Vector3 position = oldTile.transform.position;
        Quaternion rotation = oldTile.transform.rotation;
        Transform parent = oldTile.transform.parent;

        foreach (Transform tileNeighbours in oldTile.neighbours)
        {
            List<Transform> ni = tileNeighbours.gameObject.GetComponent<HexTile>().neighbours;
            for (int i = 0; i < ni.Count; i++)
            {
                if (ni[i] == oldTile.transform)
                {
                    ni.RemoveAt(i);
                }
            }
        }

        hexTiles.Remove(oldTile.transform);
        Destroy(oldTile.gameObject);
        GameObject newBuilding = Instantiate(targetBuildable.buildablePrefab, position, rotation, parent);

        hexTiles.Add(newBuilding.transform);

        // Get the HexTile component of the new building
        HexTile newTile = newBuilding.GetComponent<HexTile>();
        if (newTile == null)
        {
            newTile = newBuilding.AddComponent<HexTile>();
        }
        
        newTile.type = HexTile.TerrainType.None;

        // Adding collider from the goat
        MeshCollider meshCollider = newBuilding.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            Transform meshChild = newBuilding.transform.GetChild(0);
            if (meshChild != null)
            {
                meshCollider = meshChild.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = meshChild.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                }
            }
        }
        newTile.isBuilt = true;
        newTile.buildable = targetBuildable;
        if(targetBuildable.energyRate < 0)
            Q.Add(newTile);

        oldTile = newTile;
    }

    public void ApplyTurnChanges()
    {
        // TO DO:
        // Apply all changes -- done
        // Remove hex highlights
        // Increase Pollution levels or decrease if thats the case -- hex tiles pollute on turn end signal -- done
        // Consume food based on population -- leave for now (can be done here or residence tile can do it on signal) --done
        // Add farmed resources to inventory -- leave for now -- signal sent action done in hex tile -- done
        // Increase population based on food generated ? -- should be done centerally 
        // Consume Energy ?
        // ...smth else

        /*
        if (changes.Count < 3)
        {
            Debug.Log("Few changes left");
            return;
        }*/
        OnTurnEnd.Invoke();
        StartCoroutine(TimeDelay());


    }





    IEnumerator TimeDelay() 
    {
        yield return new WaitForSeconds(.5f);
        foreach (HexTile h in Q)
        {
            if (energy >= -h.buildable.energyRate)
            {
                energy += h.buildable.energyRate;
                if(energy < 0)
                    energy = 0;
                pollution += h.buildable.pollutionRate;
                //fish -= h.buildable.fishDepletionRate;
                h.isActive = true;
                bool addRes = true;
                foreach (HexTile res in residences)
                {
                    if (res == h) { addRes = false; break; }
                }
                if (h.gameObject.name.Contains("Town") && addRes)
                {
                    residences.Add(h); continue;
                }
                bool addGym = true;
                foreach (HexTile gym in gyms)
                {
                    if (gym == h) { addGym = false; break; }

                }
                if (h.gameObject.name.Contains("Gym") && addGym)
                {
                    gyms.Add(h);
                }
            }
            else
            {
                h.isActive = false;
                bool rmRes = false;
                foreach (HexTile res in residences)
                {
                    if (res == h) { rmRes = true; break; }
                }
                if (h.gameObject.name.Contains("Town") && rmRes)
                {
                    residences.Remove(h); continue;
                }

                bool rmGym = false;
                foreach (HexTile gym in gyms)
                {
                    if (gym == h) { rmGym = true; break; }

                }
                if (h.gameObject.name.Contains("Gym") && rmGym)
                {
                    gyms.Remove(h);
                }
                Debug.Log("Building Deactivated");
            }

        }
        if (residences.Count > 0)
            maxPopulation = 30 + populationLimitIncreasePerBuilding * residences.Count;
        else
            maxPopulation = 30;
        fish -= Mathf.Clamp(population * 4,0,fish);
        int lastTurnPopulation = population;
        population = Mathf.RoundToInt(Mathf.Clamp(fish / 4, 0, maxPopulation));
        int populationGrowth = population - lastTurnPopulation;
        Debug.Log(populationGrowth);
        if (populationGrowth > 0)
        {
            numberOfFreeWorkers += populationGrowth;
        }
        else
        {
            if (Mathf.Abs(populationGrowth) > numberOfFreeWorkers)
            {
                int workersToBeRemovedFromTiles = Mathf.Abs(populationGrowth) - numberOfFreeWorkers;
                numberOfFreeWorkers = 0;
                List<int> garbageTiles = new List<int>();
                for (int i = 0; i < tilesWithWorkers.Count; i++)
                {
                    int diff = workersToBeRemovedFromTiles - tilesWithWorkers[i].workersOnTile;
                    if (diff > 0)
                    {
                        workersToBeRemovedFromTiles -= tilesWithWorkers[i].workersOnTile;
                        tilesWithWorkers[i].workersOnTile = 0;
                        tilesWithWorkers[i].RecalculateWorkerPositions();
                        garbageTiles.Add(i);
                    }
                    else
                    {
                        tilesWithWorkers[i].workersOnTile -= workersToBeRemovedFromTiles;
                        workersToBeRemovedFromTiles = 0;
                        tilesWithWorkers[i].RecalculateWorkerPositions();
                    }
                }
                foreach (int i in garbageTiles) 
                    tilesWithWorkers.RemoveAt(i);
            }
            else 
            {
                numberOfFreeWorkers -= Mathf.Abs(populationGrowth);
            }
        }
        populationDisplay.text = population.ToString();
        expansionManager.expansionCheck(population);
        if(selectedTile)
            UpdateUI();

        foreach (TileChanges change in changes)
        {
            if (change.changeType == TileChanges.ChangeType.build)
            {
                 if (change.toBeBuilt.buildingName == "Residence")
                    {
                        noTowns += 1;
                    }
                    else if (change.toBeBuilt.buildingName == "Dam" || change.toBeBuilt.buildingName == "Wind Turbine")
                    {
                        noSBuilding += 1;
                    }
                    else if (change.toBeBuilt.buildingName == "Reactor"){
                        noReactor+=1;
                    }
                Build(change.affectedTile, change.toBeBuilt);
            } // worker changes are already done on the spot
            change.affectedTile.GetComponentInChildren<HighlightSystem>().ChangeHighlight.SetActive(false);
        }


        changes.Clear();
        uiManager.HideChanges();
    }
}
