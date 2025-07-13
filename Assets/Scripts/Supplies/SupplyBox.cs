using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class SupplyBox : MonoBehaviour
{
    private float _offsetX = 2f;
    private float _offsetY = 2f;
    
    public Action<SupplyBox> OnDestroy;

    public BoxCollider Boxcollider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }


    private void Start()
    {
        Boxcollider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        transform.position = new Vector3(parent.position.x + _offsetX, parent.position.y + _offsetY, parent.position.z);
        Rigidbody.isKinematic = true;
        Boxcollider.enabled = false;
    }

    public void Destroy()
    {
        transform.SetParent(null);
        OnDestroy?.Invoke(this);
    }
}