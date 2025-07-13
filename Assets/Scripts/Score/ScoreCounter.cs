using System;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
   private int _score;
   
   public Action<int> ScoreChanged;

   public void Add()
   {
      _score++;
      ScoreChanged?.Invoke(_score);
   }
}
