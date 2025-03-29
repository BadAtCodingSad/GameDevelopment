using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public GameObject baseChunk;
    public List<GameObject> chunkList = new List<GameObject>();
    public int xSize;
    public int ySize;

    public float xScale;
    public float yScale;

    private int townPosx = 0;
    private int townPosy = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        townPosx = Mathf.FloorToInt(xSize/2);
        townPosy = Mathf.FloorToInt(ySize / 2);
        float currX = 0;
        float currY = 0;
        for(int x  = 0; x < xSize; x++)
        {
            currY = 0;
            for (int y = 0; y < ySize; y++) 
            {
                GameObject chunk = y == townPosy & x == townPosx ? Instantiate(baseChunk) : Instantiate(chunkList[Random.Range(0, chunkList.Count)]);
                chunk.transform.position = new Vector3(currX,0,currY);
                currY += yScale;
            }
            currX += xScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
