using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private Base _basePrefab;

    private Base _base;

    public Action<Collector> OnBaseFactory;

    private void OnEnable()
    {
        _collisionHandler.CollectorReached += RemoveFlag;
    }

    private void OnDisable()
    {
        _collisionHandler.CollectorReached -= RemoveFlag;
    }

    private void RemoveFlag(Collector collector)
    {
        Destroy(this);
    }
}