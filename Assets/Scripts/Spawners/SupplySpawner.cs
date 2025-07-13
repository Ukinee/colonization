using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class SupplySpawner : Spawner<SupplyBox>
{
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private SupplyBox _supplyBoxPrefab;
    [SerializeField] private Storage _storage;
    [SerializeField] private float minStartPointX;
    [SerializeField] private float maxStartPointX;
    [SerializeField] private float minStartPointZ;
    [SerializeField] private float maxStartPointZ;
    [SerializeField] private float startPointY;

    private Coroutine _supplySpawnRoutine;
    private Vector3 _spawnPoint;
    private int _maxSpawns = 5;
    private int _spawns = 0;

    private void OnEnable()
    {
        _storage.NoSuppliesLeft += StartSpawnSupply;
    }

    private void Start()
    {
        StartSpawnSupply();
    }

    private void OnDisable()
    {
        _storage.NoSuppliesLeft -= StartSpawnSupply;
    }

    private void StartSpawnSupply()
    {
        if (_supplySpawnRoutine != null)
        {
            StopCoroutine(_supplySpawnRoutine);
            _spawns = 0;
        }

        _supplySpawnRoutine = StartCoroutine(SpawnSupply());
    }

    private IEnumerator SpawnSupply()
    {
        while (enabled)
        {
            yield return null;

            if (_spawns < _maxSpawns)
            {
                GetFromPool();
                _spawns++;
            }
        }
    }

    private void GetFromPool()
    {
        SupplyBox supplyBox = Pool.Get();
        supplyBox.transform.position = GetSpawnPoint();
        supplyBox.OnDestroy += ReleaseInPool;
    }

    private Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(minStartPointX, maxStartPointX), startPointY,
            Random.Range(minStartPointZ, maxStartPointZ));
    }

    private void ReleaseInPool(SupplyBox supplyBox)
    {
        supplyBox.Rigidbody.isKinematic = false;
        supplyBox.Boxcollider.enabled = true;
        Pool.Release(supplyBox);

        supplyBox.OnDestroy -= ReleaseInPool;
    }
}