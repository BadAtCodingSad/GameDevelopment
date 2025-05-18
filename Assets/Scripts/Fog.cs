
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    
    public float minScale = 0.04f;
    public float maxScale = 0.073f;

    private List<Transform> clouds = new List<Transform> ();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {/*
        float scale = Random.Range(minScale, maxScale);
        Vector3 scaleVector = new Vector3(scale,transform.localScale.y,scale);   
        transform.localScale = scaleVector;*/
        for (int i = 0; i < transform.childCount; i++) 
        {
            clouds.Add (transform.GetChild(i));
        }

        foreach (Transform obj in clouds) 
        {
            Vector3 scale = new Vector3(Random.Range(minScale,maxScale),obj.localScale.y, Random.Range(minScale, maxScale));
            obj.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fog")
        {
            transform.parent.gameObject.SetActive(false);
            AudioManager.instance.PlayFogPop();
        }
    }
}
