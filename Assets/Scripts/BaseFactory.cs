using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    [SerializeField] private DataBase _dataBase;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Base _basePrefab;

    // private Base _base;

    public void CreateBase(Collector collector)
    {
         Base createdBase = Instantiate(_basePrefab, collector.transform.position, Quaternion.identity);
         createdBase.Init(_dataBase, _scanner);
         createdBase.AddCollector(collector);
        Debug.Log("new base spawned");
    }

    public void CreateStartBase(Vector3 position)
    {
        
    }
}