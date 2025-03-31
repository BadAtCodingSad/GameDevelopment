using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public GameObject[] ObjMasked;
    public bool ready = false;
    void Start()
    {

    }
    private void Update()
    {
        if (!ready && ObjMasked.Length > 0) 
        {
            for (int i = 0; i < ObjMasked.Length; i++)
            {
                ObjMasked[i].GetComponentInChildren<MeshRenderer>().material.renderQueue = 3002;
            }
            ready = true;
        }
    }

}

