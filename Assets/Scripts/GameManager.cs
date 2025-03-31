using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform cam;
    public List<Transform> hexTiles = new List<Transform>();
    public bool ready = false;
    public HexTile townTile = null;
    public GameObject fogDetect;
    private GameObject fogClearer; // Scale this
    private bool townTileSet = false;

    #region Singleton
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else if(instance != this) 
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        ready = true;
    }
    #endregion

    private void Start()
    {
    }
    private void Update()
    {
        if (townTile != null && !townTileSet) 
        {
            Vector3 spawnPos = new Vector3(townTile.gameObject.transform.position.x, .5f, townTile.gameObject.transform.position.z);
            Vector3 camPos = new Vector3(townTile.gameObject.transform.position.x + 1, 4, townTile.gameObject.transform.position.z);
            fogClearer = Instantiate(fogDetect, spawnPos, Quaternion.identity);
            townTileSet = true;
            cam.position = camPos;
        }
    }
    public List<Transform> getNeighbours(Transform self) 
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in hexTiles) 
        {
            if (child != self && Vector3.Distance(child.position, self.position) < 1) 
            {
                list.Add(child);
            }
        }
        return list;
    } 
}
