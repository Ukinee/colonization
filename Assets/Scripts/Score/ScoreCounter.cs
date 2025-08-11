using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private FlagPlacer _flagPlacer;
    
    public int Score { get; private set; }

    public Action<int> ScoreChanged;

   public void Add()
   {
      Score++;
      ScoreChanged?.Invoke(Score);
   }

   public void SpendScore(int value)
   {
       Score -= value;
      ScoreChanged?.Invoke(Score);
   }
}
