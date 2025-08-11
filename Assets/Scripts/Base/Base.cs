using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    private const float OffsetZ = -5;

    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private DataBase _dataBase;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private DropOff _dropOff;

    private List<Collector> _freeCollectors;
    private bool _initialSpawnDone = false;
    private int _spawnedCount = 0;
    private int _amountOfCollectorsToSpawn = 3;

    public SpawnPoint SpawnPoint => _spawnPoint;
    public FlagPlacer FlagPlacer => _flagPlacer;
    public DataBase DataBase => _dataBase;
    public DropOff DropOff => _dropOff;

    public Action<Collector> Reassigned;

    private void Awake()
    {
        _freeCollectors = new List<Collector>();
    }

    private void OnEnable()
    {
        _collisionHandler.CollectorReached += SetFreeFromTask;

        if (_scanner != null)
        {
            _scanner.SuppliesFounded += AssignCollector;
        }
    }

    private void Start()
    {
        _scanner.SuppliesFounded += OnFirstSuppliesFound;
        _scanner.StartScan();

        if (_dataBase.SuppliesToCollect.Count > 0)
        {
            AssignCollector();
        }
    }

    private void OnDisable()
    {
        _scanner.SuppliesFounded -= AssignCollector;
        _collisionHandler.CollectorReached -= SetFreeFromTask;
    }

    public void Init(DataBase dataBase, Scanner scanner)
    {
        _dataBase = dataBase;
        _scanner = scanner;
        _initialSpawnDone = true;
        _spawnedCount = 3;
        _collectorSpawner.SpawnPointProvider.ChangeOffsetZ(OffsetZ);
        _scanner.SuppliesFounded += AssignCollector;
        _flagPlacer.UnsetFlag();
    }

    public void AddCollector(Collector collector, SpawnPoint spawnPoint, DropOff dropOff)
    {
        _freeCollectors.Add(collector);
        collector.RecieveSpawnPoint(spawnPoint);
        collector.RecieveDropOffPosition(dropOff);
    }

    public void SendBotToBuildBase()
    {
        if (_freeCollectors.Count > 0)
        {
            Collector collector = _freeCollectors[Random.Range(0, _freeCollectors.Count)];

            collector.SetTargetToFlag(_flagPlacer.Flag.transform.position);
            _freeCollectors.Remove(collector);
            _flagPlacer.UnsetFlag();
        }
    }

    private void OnFirstSuppliesFound()
    {
        _scanner.SuppliesFounded -= OnFirstSuppliesFound;

        if (!_initialSpawnDone)
        {
            SpawnCollectors();
            Debug.Log(_initialSpawnDone);
            _initialSpawnDone = true;
        }

        AssignCollector();
    }

    private void SpawnCollectors()
    {
        for (int i = _spawnedCount; i < _amountOfCollectorsToSpawn; i++)
        {
            Collector collector = _collectorSpawner.SpawnCollector(_dropOff);
            _freeCollectors.Add(collector);
            collector.ResetToSpawnPoint();
            _spawnedCount++;
        }
    }

    public void ExpansionCollectorsAmount()
    {
        _amountOfCollectorsToSpawn++;

        SpawnCollectors();
    }

    private void SetFreeFromTask(Collector collector)
    {
        _storage.SupplyDelivered(collector.TargetSupplyBox);
        collector.TargetSupplyBox.Destroy();
        collector.FreeFromTask();
        collector.ResetToSpawnPoint();
        _freeCollectors.Add(collector);

        if (_freeCollectors.Count != 0 && _dataBase.SuppliesToCollect.Count > 0)
        {
            AssignCollector();
        }
    }

    private void AssignCollector()
    {
        if (_dataBase.SuppliesToCollect.Count == 0 || _freeCollectors.Count == 0) return;

        for (int i = _freeCollectors.Count - 1; i >= 0; i--)
        {
            if (_dataBase.SuppliesToCollect.Count == 0) break;

            SupplyBox task = RequestToAssignTask();
            Collector collector = _freeCollectors[i];
            collector.RecieveTargetPosition(task);
            SendToWork(collector);
            _freeCollectors.RemoveAt(i);
        }
    }

    private void SendToWork(Collector collector)
    {
        collector.Init();
    }

    public SupplyBox RequestToAssignTask()
    {
        SupplyBox task = _dataBase.SuppliesToCollect.Dequeue();
        _dataBase.SuppliesToDeliver.Add(task);

        return task;
    }
}