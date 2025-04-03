using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Factory,
    Dam,
    Residence
}
[CreateAssetMenu(fileName = "Buildable", menuName = "Scriptable Objects/Buildable")]
public class Buildable : ScriptableObject
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
    public List<TerrainType> buildableTerrains = new List<TerrainType>();
    public GameObject buildablePrefab;
    public int metalCost;
    public int oilCost;
    public int woodCost;
    public int fishCost;
    public BuildingType type;

}
