using UnityEngine;

[CreateAssetMenu(fileName = "TileChanges", menuName = "Scriptable Objects/TileChanges")]
public class TileChanges : ScriptableObject
{
    public enum ChangeType
    {
        build,
        worker
    }
    public ChangeType changeType;
    public int numberOfWorkersChanged;
    public Buildable toBeBuilt;
    public HexTile affectedTile;
}
