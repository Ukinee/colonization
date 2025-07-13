using System.Collections;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4;

    private Coroutine _moveRoutine;
    private Transform _spawnPoint;
    private DropOff _dropPoint;
    private Vector3 _currentTarget;
    private float _distanceToInteract = 4f;
    private bool _isBusy;

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
        transform.position = _spawnPoint.transform.position;
        transform.LookAt(_spawnPoint.transform.position);
    }

    public void RecieveTargetPosition(SupplyBox target)
    {
        if (TargetSupplyBox != null)
        {
            TargetSupplyBox = null;
        }

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

    public void RecieveSpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    private void MarkAsFree()
    {
        _isBusy = false;
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
}