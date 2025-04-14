using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;


public enum ToolType
{
    View,
    Hammer,
    Pickaxe
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CameraController cam;
    public List<Transform> hexTiles = new List<Transform>();
    public bool ready = false;
    public HexTile townTile = null;
    public HexTile selectedTile = null;
    public List<TerrainResource> terrainResources = new List<TerrainResource>();

    public GameObject Highlight;

    //UI
    [Header("UI")]
    public TextMeshProUGUI tileMetalRes;
    public TextMeshProUGUI tileWoodRes;
    public TextMeshProUGUI tileOilRes;
    public TextMeshProUGUI tileFishRes;
    public TextMeshProUGUI  tileType;
    public TMP_InputField inputField;
    public TextMeshProUGUI workerRateInfo;

    public GameObject buildInfoPrefab;
    public GameObject buildInfoContainer;
    public GameObject resourceRowPrefab;
    public Transform resourceContainer;
    public Sprite metalIcon, woodIcon, oilIcon, fishIcon;
    public Transform worldspaceCanvas;


    public GameObject fogDetect;
    private GameObject fogClearer; // Scale this
    private bool townTileSet = false;

    [Header("Resource")]
    public int numberOfFreeWorkers = 10000;
    public int numberofActions = 0;
    public int numberOfTurns = 0;

    public int metal;
    public int oil;
    public int wood;
    public int fish;
    public List<TileChanges> changes = new List<TileChanges>();

    [Header("Building")]
    public List<Buildable> buildables = new List<Buildable>(); 
    public ToolType currentTool = ToolType.View; 
    public UIManager uiManager;


    Vector3 camBasePos;
    Vector3 tempVector;
    HexTile prevHex;

    #region Singleton
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else if(instance != this) 
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        ready = true;
    }
    #endregion

    private void Start()
    {
        //numberOfFreeWorkersText.text = numberOfFreeWorkers.ToString();
        DeleteRows();
    }
    private void Update()
    {
        if (townTile != null && !townTileSet) 
        {
            Vector3 spawnPos = new Vector3(townTile.gameObject.transform.position.x, 0.5f, townTile.gameObject.transform.position.z);
            camBasePos = townTile.gameObject.transform.position;
            fogClearer = Instantiate(fogDetect, spawnPos, Quaternion.identity);
            townTileSet = true;
            MoveCamToLocation(camBasePos);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            MoveCamToLocation(camBasePos);
        }

        if (selectedTile != null && selectedTile != prevHex) 
        {
            /*tileMetalRes.SetText("Metal:" + selectedTile.metal.ToString());
            tileOilRes.SetText("Oil:" + selectedTile.oil.ToString());
            tileWoodRes.SetText("Wood:" + selectedTile.wood.ToString());
            tileFishRes.SetText("Fish:" + selectedTile.fish.ToString());*/
            tileType.SetText(selectedTile.type.ToString());
            DeleteRows();
            AddRow(selectedTile.metal, metalIcon, "Metal");
            AddRow(selectedTile.wood, woodIcon, "Wood");
            AddRow(selectedTile.oil, oilIcon, "Oil");
            AddRow(selectedTile.fish, fishIcon, "Fish");
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
                        buildInfo.buildingName.text = buildable.buildingName;
                        buildInfo.buildable = buildable;
                    }
                }
            }
            
            prevHex = selectedTile;
            inputField.text = selectedTile.workersOnTile.ToString();
            /*buildableDropDown.ClearOptions();
            foreach (Buildable buildable in buildables) 
            {
                foreach (Buildable.TerrainType terrain in buildable.buildableTerrains) 
                {
                    if (terrain.ToString() == selectedTile.type.ToString()) 
                    {
                        buildableDropDown.options.Add(new TMP_Dropdown.OptionData(buildable.name));
                    }
                }
            }*/
        }
    }
    public void SelectedHexTile(HexTile hexTile) 
    {
        if (hexTile == null) {
            Debug.LogError("Bankai");
            return; }
        selectedTile = hexTile;


        if (currentTool == ToolType.Hammer)
        { 
            uiManager.ShowUI(uiManager.buildList);
        }
        else if (currentTool == ToolType.View)
        {
            uiManager.ShowUI(uiManager.viewUI);
        }
        MoveCamToLocation(hexTile.gameObject.transform.position);
        Vector3 canvasLocation = new Vector3(hexTile.gameObject.transform.position.x-1, 1.2f, hexTile.gameObject.transform.position.z);
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
    public void UpdateWorkers() 
    {
        int n;
        var isNumeric = int.TryParse(inputField.text, out n);
        TileChanges tileChanges = ScriptableObject.CreateInstance<TileChanges>();
        tileChanges.affectedTile = selectedTile; 
        tileChanges.changeType = TileChanges.ChangeType.worker;
        tileChanges.numberOfWorkersChanged = n - selectedTile.workersOnTile;
        selectedTile.workersOnTile += tileChanges.numberOfWorkersChanged;
        numberOfFreeWorkers -= tileChanges.numberOfWorkersChanged;
        workerRateInfo.text = selectedTile.getExtractionRate();
        //numberOfFreeWorkersText.text = numberOfFreeWorkers.ToString();
        TileChanges changeToBeRemoved = null;
        foreach (TileChanges change in changes) 
        {
            if (change.changeType == TileChanges.ChangeType.worker && change.affectedTile == tileChanges.affectedTile) 
            {
                tileChanges.numberOfWorkersChanged += change.numberOfWorkersChanged;
                changeToBeRemoved = change;
            }
        }
        changes.Remove(changeToBeRemoved);
        if (tileChanges.numberOfWorkersChanged != 0)
            changes.Add(tileChanges);
        //Debug.Log(n);
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

    public void BuildAction(Buildable  build) {
        int mCost=0;
        int wCost=0;
        int fCost=0;
        int oCost=0;
        foreach (Buildable buildable in buildables) {
            if(buildable == build )
            mCost= buildable.metalCost;
            wCost= buildable.woodCost;
            fCost= buildable.fishCost;
            oCost= buildable.oilCost;
        }
        if (metal>=mCost && wood>=wCost && fish>=fCost && oil>= oCost){
            
            Build(build);

            metal-=mCost;
            wood-=wCost;
            fish-=fCost;
            oil-= oCost;
        }
        else{
            Debug.Log("No enough resources");
        }

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

public void Build(Buildable build)
{
    if (selectedTile == null) return;

    Buildable targetBuildable = build;


    if (targetBuildable == null) return;

    Vector3 position = selectedTile.transform.position;
    Quaternion rotation = selectedTile.transform.rotation;
    Transform parent = selectedTile.transform.parent;
    hexTiles.Remove(selectedTile.transform);
    Destroy(selectedTile.gameObject);
    GameObject newBuilding = Instantiate(targetBuildable.buildablePrefab, position, rotation, parent);
    
    hexTiles.Add(newBuilding.transform);

    // Get the HexTile component of the new building
    HexTile newTile = newBuilding.GetComponent<HexTile>();
    if (newTile == null)
    {
        newTile = newBuilding.AddComponent<HexTile>();
    }



    switch(targetBuildable.type.ToString())
    {
        case "Factory":
            newTile.type = HexTile.TerrainType.Factory;
            break;
        case "Dam":
            newTile.type = HexTile.TerrainType.Dam;
            break;
        case "Residence":
            newTile.type = HexTile.TerrainType.Town;
            break;
        case "WindTurbine":
            newTile.type = HexTile.TerrainType.WindTurbine;
            break;
    }

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
    selectedTile = newTile;
}
}
