using System;
using UnityEngine;

public class InSpaceObject : MonoBehaviour
{
    public float DistanceFromViewer;
    public bool Exploreded;
    
 
    private void Start()
    {
        transform.LookAt(Camera.main.transform); 
    }

    void Update()
    {
       
    }

    public void OnMouseDown()
    {
        Debug.Log("MouseDown");
    }
}
