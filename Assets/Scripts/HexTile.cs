
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public List<Transform> neighbours = new List<Transform> ();
    private enum TerrainType
    {
        Ocean,
        Forest,
        Quarry,
        River,
        Mountain,
        Town
    }
    private TerrainType type;
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
        GameManager.instance.hexTiles.Add(transform);
    }
    void Start()
    {
        while (!GameManager.instance.ready) { }
        if (type == TerrainType.Town)
            GameManager.instance.townTile = this;

        neighbours = GameManager.instance.getNeighbours(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
