using UnityEngine;

public class ImputManager : MonoBehaviour
{
 public bool LeftButtonPressed {get; private set;}
 public bool LeftButtonHeld {get; private set;}
 public void Update()
{
   LeftButtonPressed = Input.GetMouseButtonDown(0);
   LeftButtonHeld = Input.GetMouseButton(0); 
}

}
