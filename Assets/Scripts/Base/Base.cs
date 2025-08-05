using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private DataBase _dataBase;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private DropOff _dropOff;

    private List<Collector> _busyCollectors;
    private List<Collector> _freeCollectors;

    public SpawnPoint SpawnPoint => _spawnPoint;
    public FlagPlacer FlagPlacer => _flagPlacer;
    public DropOff DropOff => _dropOff;

    public Action<Collector> Reassigned;

    private void Awake()
    {
        _busyCollectors = new List<Collector>();
        _freeCollectors = new List<Collector>();
    }

    private void OnEnable()
    {
        _scanner.SuppliesFounded += AssignCollector;
        _collisionHandler.CollectorReached += SetFreeFromTask;
    }

    private void Start()
    {
        SpawnCollectors();
        _scanner.StartScan();
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

        if (_scanner != null)
        {
            _scanner.SuppliesFounded += AssignCollector;
        }
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
        }
    }

    private void SpawnCollectors()
    {
        _collectorSpawner.StartSpawnCollectors();
    }

    public SupplyBox AssignTask()
    {
        SupplyBox task;

        if (_dataBase.SuppliesToCollect.Count != 0)
        {
            task = _dataBase.SuppliesToCollect.Dequeue();
            _dataBase.SuppliesToDeliver.Add(task);
        }
        else
        {
            return null;
        }

        return task;
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
        for (int i = _freeCollectors.Count - 1; i >= 0; i--)
        {
            Collector collector = _freeCollectors[i];

            if (!collector.IsBusy)
            {
                collector.RecieveTargetPosition(_collectorSpawner.RequestToAssignTask());
                Reassigned?.Invoke(collector);
                _freeCollectors.RemoveAt(i);
            }
        }
    }
}