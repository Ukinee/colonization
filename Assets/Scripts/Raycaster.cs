using System;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _baseMask;

    private RaycastHit _hit;
    private Camera _camera;

    public Action OnBaseHitted;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public bool TryHitGround(Vector3 screenPosition, out Vector3 position)
    {
        position = Vector3.zero;
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out _hit, 1000f, _groundMask))
        {
            position = _hit.point;
            return true;
        }

        return false;
    }

    public void TryHitBase(Vector3 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _baseMask))
        {
            if (hit.collider.gameObject.GetComponent<Base>() != null)
            {
                OnBaseHitted?.Invoke();
            }
        }
    }
}