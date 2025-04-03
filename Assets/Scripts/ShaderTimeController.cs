using UnityEngine;

public class ShaderTimeController : MonoBehaviour
{
    public Material material;  // Assign the material using our shader

    void Update()
    {
        if (material != null)
        {
            material.SetFloat("_CustomTime", Time.time);
        }
    }
}
