using System;
using UnityEngine;

public class Prototype : MonoBehaviour
{
    public GameObject h1;
    public GameObject h2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Vector3.Distance(h1.transform.position, h2.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
