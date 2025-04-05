using System;
using UnityEngine;
using static Globals;

public class Space : MonoBehaviour
{

    public Camera Camera;
    [SerializeField] private float _cameraTargetPitch;
    [SerializeField] private float _rotationSpeed;

    public void Init()
    {
        G.Presenter.OnMove.Subscribe(direction =>
        {
            
            float deltaTimeMultiplier = Time.deltaTime;
        Debug.Log(direction);
            _cameraTargetPitch += direction.y * _rotationSpeed * deltaTimeMultiplier;
            var rotationVelocity = direction.x * _rotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cameraTargetPitch = Rom.MathHelper.ClampAngle(_cameraTargetPitch, -89, 89);

            // Update camera target pitch
            Camera.transform.localRotation = Quaternion.Euler(_cameraTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            Camera.transform.Rotate(Vector3.up * rotationVelocity);
       
        });
        
        G.Presenter.OnZoom.Subscribe(zoom =>
        {
            Debug.Log(zoom);
            Camera.fieldOfView = Map(zoom,0f,1f,60f,90);
        });
        
        
        
    }
    
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
