using System;
using UnityEngine;

public class test : MonoBehaviour
{
   [SerializeField] private GameObject _prefab;
   [SerializeField] private LayerMask _groundLayerMask;
   [SerializeField] private LayerMask _baseLayerMask;
   
   private GameObject _pendingObject;

   private RaycastHit _hit;
   private Vector3 _pos;
   private Camera _camera;

   private void Awake()
   {
      _camera = Camera.main;
   }

   private void Update()
   {
      if (_pendingObject != null)
      {
         _pendingObject.transform.position = _pos;

         if (Input.GetMouseButtonDown(0))
         {
            PlaceObject();
         }
      }
      else
      {
         if (Input.GetMouseButtonDown(0))
         {
            TrySelectObject();
         }
      }
   }

   private void TrySelectObject()
   {
      Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, 1000f, _baseLayerMask))
      {
         if (hit.collider.gameObject.GetComponent<Base>() != null)
         {
            SelectObject();
         }
      }
   }

   private void PlaceObject()
   {
      _pendingObject = null;
   }

   private void FixedUpdate()
   {
      Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out _hit, 1000f, _groundLayerMask))
      {
         _pos = _hit.point;
      }
   }

   private void SelectObject()
   {
      _pendingObject = Instantiate(_prefab, _pos, transform.rotation);
   }
}
