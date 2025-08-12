using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public Vector3 MousePosition => Input.mousePosition;
    public bool LeftMouseButtonDown => Input.GetMouseButtonDown(0);

    public event Action LeftMouseButtonClick;
   
    private void Update()
    {
        if (LeftMouseButtonDown)
        {
            LeftMouseButtonClick?.Invoke();
        }
    }
}