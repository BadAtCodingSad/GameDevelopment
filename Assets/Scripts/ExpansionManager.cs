using System.Collections.Generic;
using UnityEngine;

public class ExpansionManager : MonoBehaviour
{
    public List<Vector2> sizes;
    public GameObject fogClearer;
    private int currentLevel = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                   
    }
    public void expansionCheck(int population) 
    {
        if (sizes[currentLevel].x <= population && currentLevel + 1 < sizes.Count) 
        {
            if (sizes[currentLevel + 1].y < population) 
            {
                IncrementLevel();
            }
        }
    }
    public void IncrementLevel() 
    {
        if (currentLevel >= sizes.Count)
            return;
        fogClearer.transform.localScale = new Vector3(sizes[currentLevel].y, sizes[currentLevel].y, sizes[currentLevel].y);
        currentLevel++;

    }
    
}
