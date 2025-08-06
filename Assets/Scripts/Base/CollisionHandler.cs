using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Action<Collector> CollectorReached;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Collector collector))
        {
            // Debug.Log(collector.TargetSupplyBox.name);
            CollectorReached?.Invoke(collector);
        }
    }
}