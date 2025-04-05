using UnityEngine;
using UnityEngine.UI;

public class HotReloadTest : MonoBehaviour
{

    
    private static int testStaticInt = 9;
    private Image tst;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
[ContextMenu("Test")]
    public void TestHotReload()
    {
        var image = GetComponent<Image>();
       image.color =Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class A
{
    public void Main()
    {
        var bNew = new B();
        B.Test = 1;
    }

}

public class B
{

    public static int Test;
}