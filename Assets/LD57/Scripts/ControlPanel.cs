using UnityEngine;
using static Globals;
using Vector2 = System.Numerics.Vector2;

public class ControlPanel : MonoBehaviour
{

   public void Init()
   {
      //Example
      G.Presenter.OnMove?.Invoke(Vector2.One);
   }
   
   
}
