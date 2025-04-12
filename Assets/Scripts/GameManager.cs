using System.Collections.Generic;
using System.Globalization;
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
    public GameObject infoBase;
    public TextMeshProUGUI tileMetalRes;
    public TextMeshProUGUI tileWoodRes;
    public TextMeshProUGUI tileOilRes;
    public TextMeshProUGUI tileFishRes;
    public TextMeshProUGUI  tileType;

    public TMP_InputField inputField;
    public TextMeshProUGUI numberOfFreeWorkersText;
    public TMP_Dropdown buildableDropDown;
    public GameObject resourceRowPrefab;
    public Transform resourceContainer;
    public Sprite metalIcon, woodIcon, oilIcon, fishIcon;


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
            prevHex = selectedTile;
            inputField.text = selectedTile.workersOnTile.ToString();
            buildableDropDown.ClearOptions();
            foreach (Buildable buildable in buildables) 
            {
                foreach (Buildable.TerrainType terrain in buildable.buildableTerrains) 
                {
                    if (terrain.ToString() == selectedTile.type.ToString()) 
                    {
                        buildableDropDown.options.Add(new TMP_Dropdown.OptionData(buildable.name));
                    }
                }
            }
        }
    }
    public void SelectedHexTile(HexTile hexTile) 
    {
        if (hexTile == null) {
            Debug.LogError("Bankai");
            return; }
        selectedTile = hexTile;


        if (currentTool == ToolType.Hammer){ 
            if (hexTile.type == HexTile.TerrainType.River){  
                uiManager.ShowUI(uiManager.riverBuildList);}
            else if (hexTile.type == HexTile.TerrainType.Mountain || hexTile.type == HexTile.TerrainType.Quarry){
                uiManager.ShowUI(uiManager.buildList);}
            }
        else if (currentTool == ToolType.View)
            {
                uiManager.ShowUI(uiManager.viewUI);
            }
        MoveCamToLocation(hexTile.gameObject.transform.position);
       

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
        numberOfFreeWorkersText.text = numberOfFreeWorkers.ToString();
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
        Debug.Log(n);
    }

    public void BuildAction(string  type) {
        int mCost=0;
        int wCost=0;
        int fCost=0;
        int oCost=0;
        foreach (Buildable buildable in buildables) {
            if (buildable.type.ToString()==type){
                mCost= buildable.metalCost;
                wCost= buildable.woodCost;
                fCost= buildable.fishCost;
                oCost= buildable.oilCost;
            }
        }
        if (metal>=mCost && wood>=wCost && fish>=fCost && oil>= oCost){
            //adding stuff to changes 
            if (type=="Factory"){
                Debug.Log("Factory built");
                }
            else if (type=="Residence")
                Debug.Log("House built");
            else
                Debug.Log("wind built");
            
            Build(type);

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
    }

public void Build(string type)
{
    if (selectedTile == null) return;

    Buildable targetBuildable = null;
    foreach (Buildable buildable in buildables) {
        if (buildable.type.ToString() == type) {
            targetBuildable = buildable;
            break;
        }
    }

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
