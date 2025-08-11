using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
   public Vector3 MousePosition => Input.mousePosition;
   public bool LeftMouseButtonDown => Input.GetMouseButtonDown(0);
   public bool RightMouseButtonDown => Input.GetMouseButtonDown(1);

   public event Action LeftMouseButtonClick;
   public event Action RightMouseButtonClick;
   
   private void Update()
   {
      if (LeftMouseButtonDown)
      {
         LeftMouseButtonClick?.Invoke();
      }

      if (RightMouseButtonDown)
      {
         RightMouseButtonClick?.Invoke();
      }
   }
}
