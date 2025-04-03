using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CameraController cam;
    public List<Transform> hexTiles = new List<Transform>();
    public bool ready = false;
    public HexTile townTile = null;
    public HexTile selectedTile = null;
    public List<TerrainResource> terrainResources = new List<TerrainResource>();

    //UI
    [Header("UI")]
    public GameObject infoBase;
    public TextMeshProUGUI tileMetalRes;
    public TextMeshProUGUI tileWoodRes;
    public TextMeshProUGUI tileOilRes;
    public TextMeshProUGUI tileFishRes;

    public TMP_InputField inputField;
    public TextMeshProUGUI numberOfFreeWorkersText;
    public TMP_Dropdown buildableDropDown;


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
        numberOfFreeWorkersText.text = numberOfFreeWorkers.ToString();
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
            tileMetalRes.SetText("Metal:" + selectedTile.metal.ToString());
            tileOilRes.SetText("Oil:" + selectedTile.oil.ToString());
            tileWoodRes.SetText("Wood:" + selectedTile.wood.ToString());
            tileFishRes.SetText("Fish:" + selectedTile.fish.ToString());
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
        selectedTile = hexTile;
        MoveCamToLocation(hexTile.gameObject.transform.position);
       

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
}
