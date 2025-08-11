using System;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _baseMask;

    private Camera _camera;
    private Base _currentBase;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.LeftMouseButtonClick += OnLeftMouseButtonClick;
        // _inputReader.RightMouseButtonClick += OnRightMouseButtoClick;
    }

    private void Update()
    {
        if (_currentBase != null)
        {
            if (TryHitGround(_inputReader.MousePosition, out var position))
            {
                _currentBase.FlagPlacer.SetFlag(position);
            }
        }
    }

    private void OnLeftMouseButtonClick()
    {
        if (_currentBase == null)
        {
            TryHitBase(_inputReader.MousePosition);
        }
        else
        {
            _currentBase = null;
        }
    }

    private void OnDisable()
    {
        _inputReader.LeftMouseButtonClick -= OnLeftMouseButtonClick;
        // _inputReader.RightMouseButtonClick -= OnRightMouseButtoClick;
    }

    public bool TryHitGround(Vector3 screenPosition, out Vector3 position)
    {
        position = Vector3.zero;
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, 1000f, _groundMask))
        {
            position = hit.point;

            return true;
        }

        return false;
    }

    public void TryHitBase(Vector3 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _baseMask))
        {
            _currentBase = hit.collider.GetComponent<Base>();
        }
    }
}
