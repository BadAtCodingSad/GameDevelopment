using UnityEngine;

public class SurroundingTerrain : MonoBehaviour
{
    Vector3[] vertices;  // Fixed typo from 'verticies' to 'vertices'

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the vertices manually for now
        vertices = new Vector3[4];
        vertices[0] = new Vector3(0f, 0f, 0f);
        vertices[1] = new Vector3(1f, 0f, 0f);
        vertices[2] = new Vector3(0f, 0f, 1f);
        vertices[3] = new Vector3(1f, 0f, 1f);
    }

    private void OnDrawGizmos()
    {
        if (vertices == null || vertices.Length == 0)
            return;

        // Draw spheres at each vertex position
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}

