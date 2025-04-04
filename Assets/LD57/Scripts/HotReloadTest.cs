using UnityEngine;
using UnityEngine.UI;

public class HotReloadTest : MonoBehaviour
{

    
    
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
