using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float dragSpeed = 2f;
    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 80f;

    [Header("Limit Settings")]
    public Vector2 panLimitX = new Vector2(-50f, 50f);
    public Vector2 panLimitZ = new Vector2(-50f, 50f);

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
        HandleClicks();
    }

    void HandleClicks()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.transform.parent.gameObject.name);
            }
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(1)) // Middle Mouse Button Pressed
        {
            dragOrigin = GetHitPoint();
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(1)) // Middle Mouse Button Released
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentPoint = GetHitPoint();
            Vector3 move = dragOrigin - currentPoint;

            // Move only when there's a valid difference
            if (move.sqrMagnitude > 0.01f)
            {
                transform.position += new Vector3(move.x, 0, move.z) * dragSpeed;
                ClampPosition();
            }
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Vector3 pos = cam.transform.localPosition;
            pos.y -= scroll * zoomSpeed;
            pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
            cam.transform.localPosition = pos;
        }
    }

    Vector3 GetHitPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.point;
        return transform.position; // Default if no hit
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        transform.position = pos;
    }
}

