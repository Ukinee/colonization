using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CollectorSpawner : MonoBehaviour
{
    [Header("Links")] 
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private Collector _collectorPrefab;
    [SerializeField] private Storage _storage;
    [SerializeField] private Base _base;
    [Header("Values")] 
    [SerializeField] private float _delay;

    private Coroutine _spawnCollectorsRoutine;
    private SupplyBox _targetSupplyBox;

    // private Transform _spawnPoint;
    private float _offsetZ;
    private int _amountOfCollectorsToSpawn = 3;
    private int _indexOfCollectors = 0;

    private void OnEnable()
    {
        _base.Reassigned += SendToWork;
    }

    private void OnDisable()
    {
        _base.Reassigned -= SendToWork;
    }

    public void StartSpawnCollectors()
    {
        _spawnCollectorsRoutine = StartCoroutine(SpawnCollectors());
    }

    public SupplyBox RequestToAssignTask()
    {
        SupplyBox supply = _base.AssignTask();

        if (supply != null)
        {
            return supply;
        }

        return null;
    }

    public void ExpansionCollectorsAmount()
    {
        _amountOfCollectorsToSpawn++;

        if (_spawnCollectorsRoutine != null)
        {
            StopCoroutine(_spawnCollectorsRoutine);
        }

        _spawnCollectorsRoutine = StartCoroutine(SpawnCollectors());
    }

    public void Init(BaseFactory factory)
    {
        _baseFactory = factory;
    }

    private SpawnPoint GetSpawnPoint()
    {
        float stepBetweenSpawnPoints = -5;

        if (_offsetZ == 0)
        {
            _offsetZ += stepBetweenSpawnPoints;
            
            return _base.SpawnPoint;
        }
        else
        {
            SpawnPoint newSpawnPoint = Instantiate(_base.SpawnPoint, _base.SpawnPoint.transform.position,
                _base.SpawnPoint.transform.rotation);
            newSpawnPoint.transform.Translate(0f, 0f, _offsetZ);
            _offsetZ += stepBetweenSpawnPoints;

            return newSpawnPoint;
        }
    }

    private IEnumerator SpawnCollectors()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return wait;

            if (_indexOfCollectors < _amountOfCollectorsToSpawn)
            {
                _targetSupplyBox = RequestToAssignTask();

                if (_targetSupplyBox != null)
                {
                    SpawnPoint spawnPoint = GetSpawnPoint();
                    Collector collector = Spawn(spawnPoint);
                    collector.RecieveTargetPosition(_targetSupplyBox);
                    collector.RecieveDropOffPosition(_base.DropOff);
                    collector.RecieveSpawnPoint(spawnPoint);
                    collector.RecieveBaseFactory(_baseFactory);
                    SendToWork(collector);
                    _indexOfCollectors++;
                }
            }
            else
            {
                StopCoroutine(_spawnCollectorsRoutine);
            }
        }
    }

    private void SendToWork(Collector collector)
    {
        collector.Init();
    }

    private Collector Spawn(SpawnPoint spawnPoint)
    {
        return Instantiate(_collectorPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
}