using System.Collections;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10;

    private Coroutine _moveRoutine;
    private BaseFactory _baseFactory;
    private Transform _spawnPoint;
    private DropOff _dropPoint;
    private Vector3 _currentTarget;
    private float _distanceToInteract = 4f;
    private bool _isBusy;
    private bool IsFlagDestination = false;

    public SupplyBox TargetSupplyBox { get; private set; }
    public bool IsBusy { get; private set; }

    private void Awake()
    {
        MarkAsFree();
    }

    private void Update()
    {
        if (TargetSupplyBox != null && _moveRoutine != null && !TargetSupplyBox.Rigidbody.isKinematic)
        {
            TryToPickUp();
        }

        if (IsFlagDestination)
        {
            TryToReachFlag();
        }
    }

    public void MarkAsBusy()
    {
        _isBusy = true;
    }

    public void Init()
    {
        if (TargetSupplyBox != null && !_isBusy)
        {
            MarkAsBusy();
            transform.LookAt(TargetSupplyBox.transform.position);
            MoveTo(TargetSupplyBox.transform.position);
        }
    }

    public void ResetToSpawnPoint()
    {
        MarkAsFree();
        StopCoroutine(_moveRoutine);
        // Debug.Log(_spawnPoint.transform.position);
        transform.position = _spawnPoint.transform.position;
        transform.LookAt(_spawnPoint.transform.position);
    }

    public void RecieveBaseFactory(BaseFactory factory)
    {
        _baseFactory = factory;
    }

    public void RecieveTargetPosition(SupplyBox target)
    {
        if (TargetSupplyBox != null) TargetSupplyBox = null;

        TargetSupplyBox = target;
    }

    public void FreeFromTask()
    {
        TargetSupplyBox = null;
    }

    public void RecieveDropOffPosition(DropOff dropPoint)
    {
        _dropPoint = dropPoint;
    }

    public void RecieveSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoint = spawnPoint.transform;
    }

    public void SetTargetToFlag(Vector3 flagPosition)
    {
        _currentTarget = flagPosition;
        TargetSupplyBox = null;
        IsFlagDestination = true;

        transform.LookAt(_currentTarget);
        MoveTo(_currentTarget);
    }

    private void MarkAsFree()
    {
        _isBusy = false;
        IsFlagDestination = false;
    }

    private void MoveTo(Vector3 targetPosition)
    {
        _currentTarget = targetPosition;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(MoveToTargetRoutine());
    }

    private IEnumerator MoveToTargetRoutine()
    {
        if (_currentTarget != null)
        {
            _isBusy = true;

            while (isActiveAndEnabled)
            {
                transform.position = Vector3.MoveTowards(transform.position, _currentTarget,
                    _moveSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }

    private void TryToPickUp()
    {
        if (Vector3.Distance(transform.position, TargetSupplyBox.transform.position) <= _distanceToInteract)
        {
            TargetSupplyBox.PickUp(transform);

            if (TargetSupplyBox.Rigidbody.isKinematic)
            {
                transform.LookAt(_dropPoint.transform.position);
                MoveTo(_dropPoint.transform.position);
            }
        }
    }

    private void TryToReachFlag()
    {
        if (Vector3.Distance(transform.position, _currentTarget) <= _distanceToInteract)
        {
            _baseFactory.CreateBase(this);
            ResetToSpawnPoint();
        }
    }
}