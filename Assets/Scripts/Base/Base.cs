using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private DataBase _dataBase;

    private List<Collector> _busyCollectors;
    private List<Collector> _freeCollectors;

    public Action<Collector> Reassigned;

    private void Awake()
    {
        _busyCollectors = new List<Collector>();
        _freeCollectors = new List<Collector>();
    }

    private void OnEnable()
    {
        _scanner.SuppliesFounded += AssignCollector;
        _collisionHandler.CollectorReturned += SetFreeFromTask;
    }

    private void Start()
    {
        SpawnCollectors();
        _scanner.StartScan();
    }

    private void OnDisable()
    {
        _scanner.SuppliesFounded -= AssignCollector;
        collisionHandler.CollectorReached -= SetFreeFromTask;
    }

    public void AddCollector(Collector collector)
    {
        _freeCollectors.Add(collector);

        foreach (var collector1 in _freeCollectors)
        {
            Debug.Log(collector1.name);
        }
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