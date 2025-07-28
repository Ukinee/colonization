using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Base _base;
    [SerializeField] private DataBase _dataBase;

    public Action<SupplyBox> Delivered;
    public Action NoSuppliesLeft;
    // public Action NoSuppliesLeft;
    public Action PriorityChanged;

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

    public void SupplyDelivered(SupplyBox supply)
    {
        Delivered?.Invoke(supply);
        _dataBase.RemoveSuppliesFromCollection(supply);
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
    private void SetPriorityToBuildBase()
    {
        _base.SendBotToBuildBase();
        PriorityChanged?.Invoke();
    }
}