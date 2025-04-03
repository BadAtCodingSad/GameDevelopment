
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public List<Transform> neighbours = new List<Transform> ();
    public TerrainResource terrainResource;
    private GameManager gameManager;

    public int metal;
    public int wood;
    public int oil;
    public int fish;

    public int workersOnTile;

    public enum TerrainType
    {
        Ocean,
        Forest,
        Quarry,
        River,
        Mountain,
        Town
    }
    public TerrainType type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        while (GameManager.instance == null) { }
        if (gameObject.name.Contains("Hills"))
            type = TerrainType.Quarry;
        else if (gameObject.name.Contains("Forest"))
            type = TerrainType.Forest;
        else if (gameObject.name.Contains("Ocean"))
            type = TerrainType.Ocean;
        else if (gameObject.name.Contains("River"))
            type = TerrainType.River;
        else if (gameObject.name.Contains("Mountain"))
            type = TerrainType.Mountain;
        else if (gameObject.name.Contains("Town"))
            type = TerrainType.Town;
        gameManager = GameManager.instance;
        gameManager.hexTiles.Add(transform);
    }
    void Start()
    {
        while (!GameManager.instance.ready) { }
        if (type == TerrainType.Town)
            gameManager.townTile = this;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
