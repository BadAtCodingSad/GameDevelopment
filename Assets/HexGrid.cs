using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width, height;

    // Prefabs for different terrain types
    public GameObject waterPrefab;
    public GameObject earthPrefab;
    public GameObject sandPrefab;
    public GameObject hexFogPrefab; // Fog object to cover hex tiles

    private enum TerrainType { Water, Earth, Sand }
    private readonly Color[] terrainColors = {
        Color.blue,                                  // Water
        new Color(0.545f, 0.271f, 0.075f),          // Earth (Brown)
        new Color(0.960f, 0.870f, 0.701f)           // Sand (Light Yellow)
    };

    public float hexWidth, hexHeight;
    public GameObject[,] hexTiles;
    public GameObject[,] fogTiles;

    private Vector3 playerPosition; // Player's position (updated based on clicks)
    private float visionRange = 5f; // Distance at which tiles become visible

    void Start()
    {
        // Initialize grid and fog
        hexTiles = new GameObject[width, height];
        fogTiles = new GameObject[width, height];

        // Get correct hex size
        hexWidth = earthPrefab.GetComponentInChildren<Renderer>().bounds.size.x;
        hexHeight = earthPrefab.GetComponentInChildren<Renderer>().bounds.size.z * 0.866f; // Hex height factor (cos(30°))

        GenerateHexGrid();

        // Set initial player position in the center (or based on click later)
        playerPosition = new Vector3(width / 2f, 0, height / 2f);
    }

    void Update()
    {
        // Update fog of war based on the player's position
        UpdateFogOfWar();
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
                    // Find the child object with the mesh (assuming it's the first child)
                    Transform meshChild = hex.transform.GetChild(0); // Target the first child with the mesh

                    // Add MeshCollider to the mesh child if it doesn't exist
                    MeshCollider meshCollider = meshChild.GetComponent<MeshCollider>();
                    if (meshCollider == null)
                    {
                        meshCollider = meshChild.gameObject.AddComponent<MeshCollider>();
                    }
                    meshCollider.convex = true; // Optional: set convex if it's not static

                    // Apply the "HexTile" tag to the mesh child
                    meshChild.gameObject.tag = "HexTile";

                    ApplyColor(hex, type);
                    hexTiles[x, y] = hex;
                }

                // Instantiate fog over each tile with the same staggered positioning and apply rotation
                GameObject fog = Instantiate(hexFogPrefab, position, Quaternion.Euler(0, 90, 0)); // Apply rotation for fog tiles
                fogTiles[x, y] = fog;
            }
        }
    }

    TerrainType GetTerrainType(int x, int y)
    {
        float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
        return noise < 0.4f ? TerrainType.Water : (noise < 0.7f ? TerrainType.Earth : TerrainType.Sand);
    }

    GameObject InstantiateTerrain(TerrainType type, Vector3 position)
    {
        GameObject prefab = type == TerrainType.Water ? waterPrefab :
                            type == TerrainType.Earth ? earthPrefab : sandPrefab;

        // Rotate the hexagon to ensure it's flat-topped
        Quaternion hexRotation = Quaternion.Euler(0, 90, 0); // Rotate by 90 degrees around the Y-axis

        return Instantiate(prefab, position, hexRotation, transform);
    }

    void ApplyColor(GameObject hex, TerrainType type)
    {
        Renderer renderer = hex.GetComponentInChildren<Renderer>();
        if (renderer) renderer.material.color = terrainColors[(int)type];
    }

    // Update the fog (physical object) based on the player's position and vision range
    void UpdateFogOfWar()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 tilePosition = hexTiles[x, y].transform.position;
                float distanceToPlayer = Vector3.Distance(playerPosition, tilePosition);

                // If the distance is within vision range, reveal the tile (remove fog object)
                if (distanceToPlayer <= visionRange)
                {
                    // Replace fog with hex tile when it's within range
                    if (fogTiles[x, y] != null)
                    {
                        Destroy(fogTiles[x, y]); // Destroy the fog object
                        fogTiles[x, y] = null; // Ensure reference is removed
                    }

                    // Optionally re-apply color or reveal a hex model (if needed)
                    hexTiles[x, y].SetActive(true); // Reveal the hex tile
                }
                else
                {
                    // Show fog (cover the hex tile)
                    if (fogTiles[x, y] == null)
                    {
                        // Re-instantiate the fog object if needed
                        fogTiles[x, y] = Instantiate(hexFogPrefab, tilePosition, Quaternion.Euler(0, 90, 0)); // Apply rotation
                    }
                    hexTiles[x, y].SetActive(false); // Hide the hex tile under fog
                }
            }
        }
    }

    // Call this method when the player moves
    public void SetPlayerPosition(Vector3 newPosition)
    {
        playerPosition = newPosition;
        UpdateFogOfWar(); // Update fog after player moves
    }

    public GameObject[,] GetHexTiles()
    {
        return hexTiles;
    }
}

