using UnityEngine;

//Script to reset static variables on scene load 
//necessary because unity reload domain is turn off

public class StaticResetter
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        Debug.Log("Static variables reset.");
    }
}
