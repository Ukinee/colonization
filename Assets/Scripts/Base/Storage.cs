using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private Base _base;
    
    private int _resourcesForNewCollector = 3;
    private int _resourcesForNewBase = 5;

    public Action<SupplyBox> Delivered;

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
        // Delivered?.Invoke(supply);
        _base.DataBase.RemoveSuppliesFromCollection(supply);
        _scoreCounter.Add();
        
        //CheckScoreMilestones here
    }

    private void CheckScoreMilestones(int value)
    {
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
            _collectorSpawner.ExpansionCollectorsAmount();
            _scoreCounter.SpendScore(_resourcesForNewCollector);
        }
    }
}