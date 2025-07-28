using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private CollisionHandler _collisionHandler;

    private Base _base;

    private void OnEnable()
    {
        _collisionHandler.CollectorReached += SpawnBase;
    }

    private void OnDisable()
    {
        _collisionHandler.CollectorReached -= SpawnBase;
    }

    private void SpawnBase(Collector collector)
    {
        _base = Instantiate(_basePrefab, collector.transform.position, Quaternion.identity);
        _base.AddCollector(collector);
        Destroy(this);
    }
}