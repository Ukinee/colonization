using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Base _base;
    
    private int _resourcesForNewCollector = 3;
    private int _resourcesForNewBase = 5;

    public void SupplyDelivered(SupplyBox supply)
    {
        _base.DataBase.RemoveSuppliesFromCollection(supply);
        _scoreCounter.Add();

        CheckScoreMilestones();
    }

    private void CheckScoreMilestones()
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
            _base.ExpansionCollectorsAmount();
            _scoreCounter.SpendScore(_resourcesForNewCollector);
        }
    }
}