using UnityEngine;

public class SpawnPointProvider : MonoBehaviour
{
    [SerializeField] private Base _base;

    private float _offsetZ;

    public SpawnPoint GetSpawnPoint()
    {
        float stepBetweenSpawnPoints = -5;

        if (_offsetZ == 0)
        {
            _offsetZ += stepBetweenSpawnPoints;

            return _base.SpawnPoint;
        }

        SpawnPoint newSpawnPoint = Instantiate(
            _base.SpawnPoint,
            _base.SpawnPoint.transform.position,
            _base.SpawnPoint.transform.rotation
        );

        newSpawnPoint.transform.Translate(0f, 0f, _offsetZ);
        _offsetZ += stepBetweenSpawnPoints;

        return newSpawnPoint;
    }
}