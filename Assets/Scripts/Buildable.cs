using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Factory,
    Dam,
    Residence,
    WindTurbine
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
    public string buildingName;
    public List<TerrainType> buildableTerrains = new List<TerrainType>();
    public GameObject buildablePrefab;
    public int metalCost;
    public int oilCost;
    public int woodCost;
    public int fishCost;
    public int energyCost;


    public Sprite icon;
    public BuildingType type;
    public string info;

    [Header("Per Turn Effects")]
    public int energyRate;
    public int pollutionRate;
    public int fishDepletionRate;




}
