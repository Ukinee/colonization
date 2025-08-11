using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    private const float OffsetZ = -5;

    [SerializeField] private DataBase _dataBase;
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private Scanner _scanner;
    
    [Space(10)]
    
    [SerializeField] private Storage _storage;
    [SerializeField] private SpawnPointProvider _spawnPointProvider;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private DropOff _dropOff;
    [SerializeField] private int _initialBotAmount = 3;

    private List<Collector> _freeCollectors;
    private List<Collector> _collectors = new List<Collector>();
    private bool _initialSpawnDone = false;

    public int CollectorsCount => _collectors.Count;
    
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
        if (_initialSpawnDone == false)
        {
            _initialSpawnDone = true;
            SpawnInitialsCollectors();
        }

        _scanner.StartScan();
    }

    private void OnDisable()
    {
        _scanner.SuppliesFounded -= AssignCollector;
        _collisionHandler.CollectorReached -= SetFreeFromTask;
    }

    public void Init(DataBase dataBase, Scanner scanner, CollectorSpawner collectorSpawner)
    {
        _dataBase = dataBase;
        _scanner = scanner;
        _collectorSpawner = collectorSpawner;
        _initialSpawnDone = true;
        _scanner.SuppliesFounded += AssignCollector;
    }

    public void AddCollector(Collector collector)
    {
        var spawnPoint = _spawnPointProvider.GetSpawnPoint();
        
        _freeCollectors.Add(collector);
        _collectors.Add(collector);
        collector.SetBaseInfo(DropOff, spawnPoint);
        collector.ResetToSpawnPoint();
    }

    public void SendBotToBuildBase()
    {
        if (_freeCollectors.Count > 0)
        {
            Collector collector = _freeCollectors[Random.Range(0, _freeCollectors.Count)];

            collector.SetTargetToFlag(_flagPlacer.Flag.transform.position);
            _freeCollectors.Remove(collector);
        }
    }

    private void SpawnInitialsCollectors()
    {
        for (int i = 0; i < _initialBotAmount; i++)
        {
            SpawnCollector();
        }
    }

    private void SpawnCollector()
    {
        Collector collector = _collectorSpawner.SpawnCollector();
        AddCollector(collector);
        
        collector.ResetToSpawnPoint();
    }

    public void ExpansionCollectorsAmount()
    {
        SpawnCollector();
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
        if (_dataBase.SuppliesToCollect.Count == 0 || _freeCollectors.Count == 0)
            return;

        for (int i = _freeCollectors.Count - 1; i >= 0; i--)
        {
            if (_dataBase.SuppliesToCollect.Count == 0)
                break;

            SupplyBox task = RequestToAssignTask();
            Collector collector = _freeCollectors[i];
            collector.RecieveTargetPosition(task);
            _freeCollectors.RemoveAt(i);
        }
    }

    public SupplyBox RequestToAssignTask()
    {
        SupplyBox task = _dataBase.SuppliesToCollect.Dequeue();
        _dataBase.SuppliesToDeliver.Add(task);

        return task;
    }
}
