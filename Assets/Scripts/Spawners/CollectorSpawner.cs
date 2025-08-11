using UnityEngine;

public class CollectorSpawner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SpawnPointProvider _spawnPointProvider;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private Collector _collectorPrefab;

    private int _axisY = -90;
    
    public SpawnPointProvider SpawnPointProvider => _spawnPointProvider;

    public Collector SpawnCollector(DropOff dropOff)
    {
        SpawnPoint spawnPoint = _spawnPointProvider.GetSpawnPoint();
        Collector collector = Instantiate(_collectorPrefab, spawnPoint.transform.position, Quaternion.Euler(0, _axisY, 0));
        collector.RecieveDropOffPosition(dropOff);
        collector.RecieveSpawnPoint(spawnPoint);
        collector.RecieveBaseFactory(_baseFactory);

        return collector;
    }
}