
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{


    public List<Transform> neighbours = new List<Transform> ();
    public TerrainResource terrainResource;
    private GameManager gameManager;
    public List<GameObject> workers= new List<GameObject> ();
    public Buildable buildable;
    public bool isActive = true;

    public int metal;
    public int wood;
    public int oil;
    public int fish;

    public int workersOnTile;
    public int workersOnTileLastTurn;
    public string tileName;
    public bool isBuilt = false;
    public GameObject inactiveSprite;
    public enum TerrainType
    {
        Ocean,
        Forest,
        Quarry,
        River,
        Mountain,
        Town,
        None
    }
    public TerrainType type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        while (GameManager.instance == null) { }
        if (gameObject.name.Contains("Hills"))
        {
            type = TerrainType.Quarry;
            tileName = "Hills";
        }
        else if (gameObject.name.Contains("Forest"))
        {
            type = TerrainType.Forest;
            tileName = "Forest";
        }
        else if (gameObject.name.Contains("Ocean"))
        {
            type = TerrainType.Ocean;
            tileName = "Ocean";
        }
        else if (gameObject.name.Contains("River"))
        {
            type = TerrainType.River;
            tileName = "River";
        }
        else if (gameObject.name.Contains("Mountain"))
        {
            type = TerrainType.Mountain;
            tileName = "Mountain";
        }
        else if (gameObject.name.Contains("Town"))
        {
            type = TerrainType.Town;
            tileName = "Town";
        }
        gameManager = GameManager.instance;
        gameManager.hexTiles.Add(transform);
    }
    void Start()
    {
        while (!GameManager.instance.ready) { }
        if (type == TerrainType.Town)
            gameManager.townTile = this;
        transform.GetChild(0).gameObject.AddComponent<HighlightSystem>();
        GameObject highlight = Instantiate(gameManager.Highlight);
        GameObject changehighlight = Instantiate(gameManager.ChangeHighlight);
        highlight.transform.parent = transform;
        changehighlight.transform.parent = transform;
        Vector3 yaseen = new Vector3();
        yaseen.y = 0.12f;
        highlight.transform.localPosition = yaseen;
        changehighlight .transform.localPosition = yaseen;
        highlight.transform.localRotation = Quaternion.identity;
        changehighlight.transform.localRotation = Quaternion.identity;
        transform.GetChild(0).gameObject.GetComponent<HighlightSystem>().Highlight = highlight;
        transform.GetChild(0).gameObject.GetComponent<HighlightSystem>().ChangeHighlight = changehighlight;


        neighbours = gameManager.getNeighbours(transform);
        foreach(TerrainResource terrainResource in gameManager.terrainResources) 
        {
            if (terrainResource.type.ToString() == type.ToString()) 
            {
                metal = Mathf.FloorToInt(Random.Range(terrainResource.metal.x, terrainResource.metal.y));
                wood = Mathf.FloorToInt(Random.Range(terrainResource.wood.x, terrainResource.wood.y));
                oil = Mathf.FloorToInt(Random.Range(terrainResource.oil.x, terrainResource.oil.y));
                fish = Mathf.FloorToInt(Random.Range(terrainResource.fish.x, terrainResource.fish.y));
            }
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("energy")) 
            {
                inactiveSprite = child.gameObject;
                inactiveSprite.SetActive(false);
                return;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inactiveSprite != null)
            inactiveSprite.SetActive(!isActive);
    }
    public string getExtractionRate() 
    {
        if (isBuilt)
            return "";
        return "+ " + Mathf.RoundToInt(terrainResource.metalRate * workersOnTile + terrainResource.metalRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count) + " metal/turn" + "+ " 
            + Mathf.RoundToInt(terrainResource.oilRate * workersOnTile + terrainResource.oilRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count) + " oil/turn"
            + "+ " + Mathf.RoundToInt(terrainResource.fishRate * workersOnTile + terrainResource.fishRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count) + " fish/turn"
            + "+ " + Mathf.RoundToInt(terrainResource.woodRate * workersOnTile + terrainResource.woodRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count) + " wood/turn"; 
    }

    // Turn Mec
    private void OnEnable()
    {
        //gameManager.OnTurnEnd += TurnHandler;
        GameManager.OnTurnEnd += TurnHandler;

    }

    private void OnDisable()
    {
        //gameManager.OnTurnEnd -= TurnHandler;
        GameManager.OnTurnEnd -= TurnHandler;
    }

    private void TurnHandler()
    {
        // Increase Pollution levels or decrease if thats the case -- hex tiles pollute on turn end signal -- dummy done
        // Consume food based on population -- leave for now (can be done here or residence tile can do it on signal) - dummy done
        // Add farmed resources to inventory -- leave for now -- signal sent action done in hex tile -- done


        if (workersOnTile > 0)
        {
            gameManager.fish += Mathf.Clamp(Mathf.RoundToInt(terrainResource.fishRate * workersOnTile + terrainResource.fishRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,fish);
            gameManager.metal += Mathf.Clamp(Mathf.RoundToInt(terrainResource.metalRate * workersOnTile + terrainResource.metalRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,metal);
            gameManager.wood += Mathf.Clamp(Mathf.RoundToInt(terrainResource.woodRate * workersOnTile + terrainResource.woodRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,wood);
            gameManager.oil += Mathf.Clamp(Mathf.RoundToInt(terrainResource.oilRate * workersOnTile + terrainResource.oilRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,oil);

            // dedcut from tile resource
            fish -= Mathf.Clamp(Mathf.RoundToInt(terrainResource.fishRate * workersOnTile + terrainResource.fishRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,fish);
            metal -= Mathf.Clamp(Mathf.RoundToInt(terrainResource.metalRate * workersOnTile + terrainResource.metalRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,metal);
            wood -= Mathf.Clamp(Mathf.RoundToInt(terrainResource.woodRate * workersOnTile + terrainResource.woodRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,wood);
            oil -= Mathf.Clamp(Mathf.RoundToInt(terrainResource.oilRate * workersOnTile + terrainResource.oilRate * workersOnTile * gameManager.workerEfficiencyPerGym * gameManager.gyms.Count),0,oil);
            gameManager.pollution += workersOnTile * terrainResource.pollutionRate;
            workersOnTileLastTurn = workersOnTile;
            HexTile hexTile = null;
            foreach (Transform transform in neighbours) 
            {
                hexTile = transform.gameObject.GetComponent<HexTile>();
                hexTile.fish -= Mathf.RoundToInt(Mathf.Clamp(workersOnTile * terrainResource.pollutionRate, 0, hexTile.fish));
;               hexTile.wood -= Mathf.RoundToInt(Mathf.Clamp(workersOnTile * terrainResource.pollutionRate, 0, hexTile.wood));
;           }
        }
        if (isBuilt)
        {
            if (buildable.energyRate > 0)
            {
                gameManager.pollution += buildable.pollutionRate;
                gameManager.energy += buildable.energyRate;
            }
        }
    }

    public void RecalculateWorkerPositions() 
    {
        for (int i = 0; i < workers.Count; i++) 
        {
            Destroy(workers[i].gameObject);
        }
        workers.Clear();
        for (int i = 0; i < workersOnTile; i++) 
        {
            
            workers.Add(Instantiate(gameManager.workerManager.worker, transform));
            workers[i].transform.localPosition = gameManager.workerManager.workerPositions[i];
        }
    }

}
