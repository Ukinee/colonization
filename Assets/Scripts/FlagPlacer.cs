using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject _flag;
    [SerializeField] private Raycaster _raycaster;

    private GameObject _pendingObject;
    private Vector3 _pos;

    private void OnEnable()
    {
        _raycaster.OnBaseHit += SetFlag;
        _storage.PriorityChanged += ChangeState;
    }

    private void Update()
    {
        if (_pendingObject != null)
        {
            UpdatePendingObjectPosition();
            ClearPendingObject();
        }

        if (_input.LeftMouseButtonDown)
        {
            _raycaster.TryHitBase(_input.MousePosition);
        }
    }

    private void OnDisable()
    {
        _raycaster.OnBaseHit -= SetFlag;
        _storage.PriorityChanged -= ChangeState;
    }

    private void UpdatePendingObjectPosition()
    {
        if (_raycaster.TryHitGround(_input.MousePosition, out Vector3 position))
        {
            _pos = position;
            _pendingObject.transform.position = _pos;
        }
    }

    private void ClearPendingObject()
    {
        if (_input.LeftMouseButtonDown)
        {
            _pendingObject = null;
        }
    }

    private void SetFlag()
    {
        if (_pendingObject == null)
        {
            CreateFlag();
        }
    }

    private void CreateFlag()
    {
        Flag = Instantiate(_flagPrefab, _pos, transform.rotation);
        _pendingObject = Flag;
        // OnFlagSet?.Invoke(Flag);
    }
}