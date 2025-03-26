using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public GameObject grid; // Reference to the HexGrid object
    private int gridWidth;
    private int gridHeight;

    void Start()
    {
        // Ensure the grid is fully generated before accessing its dimensions
        HexGrid hexGrid = grid.GetComponent<HexGrid>();

        if (hexGrid == null)
        {
            Debug.LogError("HexGrid component is not found on the grid object!");
            return;
        }

        // Debug the width and height to ensure correct values are being read
        gridWidth = hexGrid.width;
        gridHeight = hexGrid.height;

        // Check if the width and height are still zero or invalid
        Debug.Log($"HexGrid Width: {gridWidth}, Height: {gridHeight}");

        // Validate the grid size
        if (gridWidth == 0 || gridHeight == 0)
        {
            Debug.LogError("Invalid grid dimensions detected! Width and/or Height are zero.");
            return;
        }

        // Calculate the center position of the grid
        float centerX = (gridWidth - 1) * hexGrid.hexWidth * 0.75f / 2;
        float centerZ = (gridHeight - 1) * hexGrid.hexHeight / 2;

        // Debug log to check the calculated center
        Debug.Log($"Calculated Grid Center X: {centerX}, Center Z: {centerZ}");

        // Set the camera position above the center of the grid
        transform.position = new Vector3(centerX, 10f, centerZ); // Y = 10 to position the camera above
        transform.LookAt(new Vector3(centerX, 0f, centerZ)); // Look at the center of the grid
    }
}
