using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 80f;
    public float movementSpeed = 0.5f;

    [Header("Limit Settings")]
    public Vector2 panLimitX = new Vector2(-50f, 50f);
    public Vector2 panLimitZ = new Vector2(-50f, 50f);

    public Vector3 newPosition;


    public Vector3 dragStartPostion;
    public Vector3 dragCurrentPostion;
    private Camera cam;
    Vector3 tempPosition;

    void Start()
    {
        newPosition = transform.position;
        cam = Camera.main;
    }

    void Update()
    {
        HandleClicks();
        HandleMouseInput();
        HandleZoom();
        HandleMovement();

    }

    private void HandleClicks()
    {
        if (Input.GetMouseButtonDown(0)) // Casts a ray from the camera to the mouse position
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HexTile tempHex = hit.collider.gameObject.GetComponentInParent<HexTile>();
                if (tempHex != null)
                {
                    GameManager.instance.SelectedHexTile(tempHex);
                }
            }
        }
    }

    private void HandleMouseInput() 
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry)) 
            {
                dragStartPostion = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1)) 
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPostion = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPostion - dragCurrentPostion;
            }
        }
    }
    private void HandleMovement()
    {
        tempPosition = newPosition;
        newPosition.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSpeed);
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
}

