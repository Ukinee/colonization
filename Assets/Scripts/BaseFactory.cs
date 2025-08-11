using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private DataBase _dataBase;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Base _basePrefab;

    public void CreateBase(Collector collector)
    {
        Base createdBase = Instantiate(_basePrefab, collector.transform.position, Quaternion.identity);
        createdBase.Init(_dataBase, _scanner, _collectorSpawner);
        createdBase.AddCollector(collector);
    }
}