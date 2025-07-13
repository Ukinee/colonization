using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;

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
    }

    private void OnDisable()
    {
        _scanner.SuppliesFounded -= AssignCollector;
        _collisionHandler.CollectorReturned -= SetFreeFromTask;
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

        if (_freeCollectors.Count != 0 && _storage.AmountOfSuppliesToCollect > 0)
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