using System.Collections;
using UnityEngine;

public class CameraClickDetector : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public Camera mainCamera;
    public float moveSpeed = 5f;

    private HexGrid hexGrid;

    void Start()
    {
        // Ensure there's a HexGrid in the scene to update the fog of war
        hexGrid = FindObjectOfType<HexGrid>();
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid not found in the scene.");
        }
    }

    void Update()
    {
        DetectClick();
    }

    void DetectClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Get the position of the hex clicked
                Vector3 clickedPosition = hit.point;
                clickedPosition.y = 0; // Ensure we move on the XZ plane, not affecting height

                // Move player to the clicked position
                MovePlayerToHex(clickedPosition);
            }
        }
    }

    void MovePlayerToHex(Vector3 targetPosition)
    {
        if (player != null)
        {
            // Smoothly move the player to the target position
            StartCoroutine(MovePlayerCoroutine(targetPosition));

            // Update the fog of war after the player moves
            if (hexGrid != null)
            {
                hexGrid.SetPlayerPosition(targetPosition);
            }
        }
        else
        {
            Debug.LogError("Player object is null. Cannot move.");
        }
    }

    // Coroutine to smoothly move the player to the clicked hex tile
    IEnumerator MovePlayerCoroutine(Vector3 targetPosition)
    {
        Vector3 startPosition = player.transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(player.transform.position, targetPosition) > 0.05f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            player.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        // Ensure the player ends exactly at the target position
        player.transform.position = targetPosition;
    }
}
