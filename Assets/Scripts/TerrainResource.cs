using UnityEngine;

[CreateAssetMenu(fileName = "TerrainResource", menuName = "Scriptable Objects/TerrainResource")]
public class TerrainResource : ScriptableObject
{
    public enum TerrainType
    {
        Ocean,
        Forest,
        Quarry,
        River,
        Mountain,
        Town
    }
    public TerrainType type;
    public Vector2 metal;
    public Vector2 wood;
    public Vector2 oil;
    public Vector2 fish;

    public float metalRate;
    public float woodRate;
    public float oilRate;
    public float fishRate;

    public float pollutionRate;
}
