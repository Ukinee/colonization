using UnityEngine;

public class CollectorSpawner : MonoBehaviour
{
    [Header("Links")] [SerializeField] private Collector _collectorPrefab;
    [SerializeField] private BaseFactory _baseFactory;

    public Collector SpawnCollector()
    {
        Collector collector = Instantiate(_collectorPrefab);
        collector.Init(_baseFactory);

        return collector;
    }
}
