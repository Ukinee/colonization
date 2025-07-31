using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private FlagPlacer _flagPlacer;
    
    private int _resourcesForNewCollector = 3;
    private int _resourcesForNewBase = 5;

   public void Add()
   {
      _score++;
      ScoreChanged?.Invoke(_score);

      if (_score == _resourcesForNewCollector)
      {
         RequirementReached?.Invoke();
         _score -= _resourcesForNewCollector;
      }
   }
}
