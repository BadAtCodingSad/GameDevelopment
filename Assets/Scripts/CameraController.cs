using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseMoveSpeed = 20f;
    public float shiftMultiplier = 2f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float edgeScrollThreshold = 10f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 60f;
    public bool allowRotation = true;

    [Header("Zoom Settings")]
    public float zoomSpeed = 100f;
    public float minZoom = 10f;
    public float maxZoom = 80f;

    [Header("Limit Settings")]
    public Vector2 panLimitX = new Vector2(-50f, 50f);
    public Vector2 panLimitZ = new Vector2(-50f, 50f);

    public event Action<Vector3> OnCameraMoved; // For minimap sync

    private Vector3 targetVelocity;
    private Vector3 currentVelocity;
    private Vector3 dragOrigin;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseDrag();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 inputDir = Vector3.zero;

        // Keyboard Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            inputDir += Vector3.forward;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            inputDir += Vector3.back;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            inputDir += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            inputDir += Vector3.right;

        // Edge Scroll
        if (Input.mousePosition.x >= Screen.width - edgeScrollThreshold)
            inputDir += Vector3.right;
        if (Input.mousePosition.x <= edgeScrollThreshold)
            inputDir += Vector3.left;
        if (Input.mousePosition.y >= Screen.height - edgeScrollThreshold)
            inputDir += Vector3.forward;
        if (Input.mousePosition.y <= edgeScrollThreshold)
            inputDir += Vector3.back;

        inputDir.Normalize();

        // Speed boost
        float speed = baseMoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            speed *= shiftMultiplier;

        targetVelocity = inputDir * speed;

        // Smooth Movement
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity,
            (targetVelocity.magnitude > currentVelocity.magnitude ? acceleration : deceleration) * Time.deltaTime);

        transform.position += currentVelocity * Time.deltaTime;
        ClampPosition();

        if (currentVelocity != Vector3.zero)
            OnCameraMoved?.Invoke(transform.position);
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-diff.x * baseMoveSpeed, 0, -diff.y * baseMoveSpeed);

            transform.Translate(move * Time.deltaTime, Space.World);
            dragOrigin = Input.mousePosition;
            ClampPosition();
            OnCameraMoved?.Invoke(transform.position);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Vector3 pos = cam.transform.localPosition;
            pos.y -= scroll * zoomSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
            cam.transform.localPosition = pos;
        }
    }

    void HandleRotation()
    {
        if (!allowRotation) return;

        float rotate = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotate = -1f;
        if (Input.GetKey(KeyCode.E))
            rotate = 1f;

        if (rotate != 0f)
        {
            transform.Rotate(Vector3.up, rotate * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        transform.position = pos;
    }
}

