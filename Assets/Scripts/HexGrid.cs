using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width, height;

    // Prefabs for different terrain types
    public GameObject oceanPrefab;
    public GameObject forestPrefab;
    public GameObject quarryPrefab;
    public GameObject riverPrefab;
    public GameObject mountainPrefab;


    private enum TerrainType { 
        Ocean, 
        Forest, 
        Quarry,
        River,
        Mountain
    }


    public float hexWidth, hexHeight;
    public GameObject[,] hexTiles;

    void Start()
    {
        // Initialize grid
        hexTiles = new GameObject[width, height];

        // Get correct hex size
        //hexWidth = forestPrefab.GetComponentInChildren<Renderer>().bounds.size.x;

        hexHeight = forestPrefab.GetComponentInChildren<Renderer>().bounds.size.z * 0.866f; // Hex height factor

        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Correctly position hexagons in a staggered grid
                float xPos = x * hexWidth * 0.75f;
                float zPos = y * hexHeight + (x % 2 == 1 ? hexHeight / 2f : 0);

                Vector3 position = new Vector3(xPos, 0, zPos);
                TerrainType type = GetTerrainType(x, y);
                GameObject hex = InstantiateTerrain(type, position);

                if (hex != null)
                {
                    Transform meshChild = hex.transform.GetChild(0); // Target the mesh child

                    // Add MeshCollider if missing
                    if (!meshChild.TryGetComponent(out MeshCollider meshCollider))
                    {
                        meshCollider = meshChild.gameObject.AddComponent<MeshCollider>();
                        meshCollider.convex = true;
                    }

                    hexTiles[x, y] = hex;
                }
            }
        }
    }

    TerrainType GetTerrainType(int x, int y)
    {
        return (TerrainType)Random.Range(0, 5);
    }

    GameObject InstantiateTerrain(TerrainType type, Vector3 position)
    {
        GameObject prefab = null;
        switch (type)
        {
            case TerrainType.Mountain:
                prefab = mountainPrefab;
                break;
            case TerrainType.Quarry:
                prefab = quarryPrefab;
                break;
            case TerrainType.Forest:
                prefab = forestPrefab;
                break;
            case TerrainType.Ocean:
                prefab = oceanPrefab;
                break;
            case TerrainType.River:
                prefab = riverPrefab;
                break;

        }

        Quaternion hexRotation = Quaternion.Euler(0, 90, 0); // Rotate for flat top

        return Instantiate(prefab, position, hexRotation, transform);
    }

    public GameObject[,] GetHexTiles()
    {
        return hexTiles;
    }
}
