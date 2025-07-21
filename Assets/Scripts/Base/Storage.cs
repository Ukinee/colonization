using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Base _base;

    private Queue<SupplyBox> _suppliesToCollect;
    private List<SupplyBox> _suppliesToDeliver;
    public int AmountOfSuppliesToCollect => _suppliesToCollect.Count;

    public Action<SupplyBox> Delivered;
    public Action NoSuppliesLeft;

    private void Awake()
    {
        _suppliesToCollect = new Queue<SupplyBox>();
        _suppliesToDeliver = new List<SupplyBox>();
    }

    private void OnEnable()
    {
        _scoreCounter.RequirementReached += ExpandCollectors;
    }

    private void OnDisable()
    {
        _scoreCounter.RequirementReached -= ExpandCollectors;
    }

    public SupplyBox AssignTask()
    {
        SupplyBox task;

        if (_suppliesToCollect.Count != 0)
        {
            task = _suppliesToCollect.Dequeue();
            _suppliesToDeliver.Add(task);
        }
        else
        {
            return null;
        }

        return task;
    }

    public void GetSuppliesToCollect(Queue<SupplyBox> suppliesToCollect)
    {
        foreach (SupplyBox supply in suppliesToCollect)
        {
            if (!_suppliesToCollect.Contains(supply) && !_suppliesToDeliver.Contains(supply))
            {
                _suppliesToCollect.Enqueue(supply);
            }
        }
    }

    public void SupplyDelivered(SupplyBox supply)
    {
        Delivered?.Invoke(supply);
        RemoveSuppliesFromCollection(supply);
        _scoreCounter.Add();
    }

    private void ExpandCollectors()
    {
        _collectorSpawner.ExpansionCollectorsAmount();
    }

    private void RemoveSuppliesFromCollection(SupplyBox supply)
    {
        _suppliesToDeliver.Remove(supply);

        if (_suppliesToDeliver.Count == 0)
        {
            NoSuppliesLeft?.Invoke();
        }
    }
}