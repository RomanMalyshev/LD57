using UnityEngine;
using UnityEngine.Serialization;

public class InSpaceObject : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public float DistanceFromViewer;
    public bool Exploreded;
    
    public float _defaultScale;
    public float _maxScale;



    private void Start()
    {
        _defaultScale = Sprite.transform.localScale.x;
        transform.LookAt(Camera.main.transform); 
     }



  
}
