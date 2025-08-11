using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(DataBase))]

public class DataBase : MonoBehaviour
{
    public Queue<SupplyBox> SuppliesToCollect { get; private set; }
    public List<SupplyBox> SuppliesToDeliver { get; private set; }

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

        if (SuppliesToDeliver.Count == 0 && SuppliesToCollect.Count == 0)
        {
            NoSuppliesLeft?.Invoke();
        }
    }
}