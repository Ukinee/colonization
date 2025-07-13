using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefabObject;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    protected ObjectPool<T> Pool;

    private void Awake()
    {
        Pool = new ObjectPool<T>
        (createFunc: () => Instantiate(_prefabObject),
            actionOnGet: GetFromPool,
            actionOnRelease: ReleaseInPool,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            _poolCapacity,
            _poolMaxSize
        );
    }

    private void GetFromPool(T prefabObject)
    {
        prefabObject.gameObject.SetActive(true);
    }

    private void ReleaseInPool(T prefabObject)
    {
        prefabObject.gameObject.SetActive(false);
    }
}