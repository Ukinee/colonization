using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private DataBase _dataBase;
    [SerializeField] private Base _base;
    
    private int _resourcesForNewCollector = 3;
    private int _resourcesForNewBase = 5;

    public Action<SupplyBox> Delivered;

    // public Action NoSuppliesLeft;

    // public Action NoSuppliesLeft;
    public Action PriorityChanged;

    private void OnEnable()
    {
        _scoreCounter.ScoreChanged += CheckScoreMilestones;
    }

    private void OnDisable()
    {
        _scoreCounter.ScoreChanged -= CheckScoreMilestones;
    }

    public void SupplyDelivered(SupplyBox supply)
    {
        Delivered?.Invoke(supply);
        _dataBase.RemoveSuppliesFromCollection(supply);
        _scoreCounter.Add();
    }

    // private void ExpandCollectors()
    // {
    //     _collectorSpawner.ExpansionCollectorsAmount();
    // }

    // private void RemoveSuppliesFromCollection(SupplyBox supply) // метод теперь в DataBase
    // {
    //     _suppliesToDeliver.Remove(supply);
    //
    //     if (_suppliesToDeliver.Count == 0)
    //     {
    //         NoSuppliesLeft?.Invoke();
    //     }
    // }

    private void SetPriorityToBuildBase()
    {
        _base.SendBotToBuildBase();
        // PriorityChanged?.Invoke();
    }

    private void CheckScoreMilestones(int value)
    {
        //тут надо проверять достиг ли счет 3х для спавна нового бота или достиг ли 5ти для спавна новой базы
        if (_base.FlagPlacer.IsFlagSet)
        {
            if (_scoreCounter.Score == _resourcesForNewBase)
            {
                _base.SendBotToBuildBase();
                _scoreCounter.SpendScore(_resourcesForNewBase);
            }
        }
        else if (_scoreCounter.Score == _resourcesForNewCollector)
        {
            //создать нового бота
            _collectorSpawner.ExpansionCollectorsAmount();
            _scoreCounter.SpendScore(_resourcesForNewCollector);
        }
    }
}