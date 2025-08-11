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
    private bool _isFlagDestination = false;

    public SupplyBox TargetSupplyBox { get; private set; }

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

        if (_isFlagDestination)
        {
            TryToReachFlag();
        }
    }

    public void Init(BaseFactory baseFactory)
    {
        _baseFactory = baseFactory;
    }

    public void SetBaseInfo(DropOff dropPoint, SpawnPoint spawnPoint)
    {
        _dropPoint = dropPoint;
        _spawnPoint = spawnPoint.transform;
    }

    public void ResetToSpawnPoint()
    {
        MarkAsFree();

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        transform.position = _spawnPoint.transform.position;
        transform.LookAt(_spawnPoint.transform.position);
    }

    public void RecieveTargetPosition(SupplyBox target)
    {
        TargetSupplyBox = target;
        transform.LookAt(TargetSupplyBox.transform.position);
        MoveTo(TargetSupplyBox.transform.position);
    }

    public void FreeFromTask()
    {
        TargetSupplyBox = null;
    }

    public void SetTargetToFlag(Vector3 flagPosition)
    {
        Debug.Log("flag set as target");
        _currentTarget = flagPosition;
        TargetSupplyBox = null;
        _isFlagDestination = true;

        transform.LookAt(_currentTarget);
        MoveTo(_currentTarget);
    }

    private void MarkAsFree()
    {
        _isFlagDestination = false;
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
        while (isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _currentTarget,
                _moveSpeed * Time.deltaTime
            );

            yield return null;
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
