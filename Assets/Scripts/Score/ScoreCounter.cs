using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
   private int _score;
   private int _resourcesForNewCollector = 3;
   
   public Action<int> ScoreChanged;
   public Action RequirementReached;

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
