using System.Collections.Generic;
using UnityEngine;
using System;

public class DataBase : MonoBehaviour
{
    public List<SupplyBox> SuppliesToDeliver { get; private set; }
    public Queue<SupplyBox> SuppliesToCollect { get; private set; }

    public Action NoSuppliesLeft;

    private void Awake()
    {
        SuppliesToCollect = new Queue<SupplyBox>();
        SuppliesToDeliver = new List<SupplyBox>();
    }

    public void GetSuppliesToCollect(Queue<SupplyBox> suppliesToCollect)
    {
        foreach (SupplyBox supply in suppliesToCollect)
        {
            if (!SuppliesToCollect.Contains(supply) && !SuppliesToDeliver.Contains(supply))
            {
                SuppliesToCollect.Enqueue(supply);
            }
        }
    }

    public void RemoveSuppliesFromCollection(SupplyBox supply)
    {
        SuppliesToDeliver.Remove(supply);
        Debug.Log("бот удален из коллекции базы данных");

        if (SuppliesToDeliver.Count == 0)
        {
            NoSuppliesLeft?.Invoke();
        }
    }
}