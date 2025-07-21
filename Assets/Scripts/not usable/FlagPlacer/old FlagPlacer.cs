using UnityEngine;
using UnityEngine.EventSystems;

public class oldFlagPlacer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _flagPrefab;

    // private float _offset = 0.5f;
    private GameObject _toBuild;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        // _flagPrefab = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.GetComponent<Base>() != null)
                {
                    Debug.Log("click");
                    PrepareFlag();
                    ScreenPointTracker();
                }
            }
        }
    }

    // private void SetFlagPrefab(GameObject prefab)
    // {
    //     _flagPrefab = prefab;
    //     
    // }

    private void PrepareFlag()
    {
        if (_toBuild) Destroy(_toBuild);

        _toBuild = Instantiate(_flagPrefab);
        _toBuild.SetActive(false);
    }

    private void ScreenPointTracker()
    {
        if (_flagPrefab != null) 
        {
            // if (EventSystem.current.IsPointerOverGameObject())
            // {
            //     if(_toBuild.activeSelf) _toBuild.SetActive(false);
            // }
            // else if (!_toBuild.activeSelf) _toBuild.SetActive(true);
            
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, 1000f, _layerMask))
            {
                if (!_toBuild.activeSelf) _toBuild.SetActive(true);

                _toBuild.transform.position = _hit.point;
            }
            else if (_toBuild.activeSelf) _toBuild.SetActive(false);
        }
    }

    // private void SpawnPosition()
    // {
    //     Vector3 spawnPosition = hit.point + (Vector3.up * _offset); //вставлять в место обработки рейкаста
    // }
}