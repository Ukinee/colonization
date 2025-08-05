using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private Storage _storage;

    private Flag _pendingObject;
    private Vector3 _pos;

    public Flag Flag { get; private set; } = null;
    public bool IsFlagSet { get; private set; } = false;

    private void OnEnable()
    {
        _raycaster.OnBaseHit += SetFlag;
        // _storage.PriorityChanged += ChangeState;
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
        // _storage.PriorityChanged -= ChangeState;
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
            IsFlagSet = true;
            Debug.Log(IsFlagSet);
        }
    }

    private void SetFlag()
    {
        if (_pendingObject == null)
        {
            if (Flag != null)
            {
                Destroy(Flag.gameObject);
                Flag = null;
                IsFlagSet = false;
            }

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