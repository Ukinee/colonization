using UnityEngine;

public class InputReader : MonoBehaviour
{
   public Vector3 MousePosition => Input.mousePosition;
   public bool LeftMouseButtonDown => Input.GetMouseButtonDown(0);
}
