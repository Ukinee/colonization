using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _scanRadius;
    [Header("Links")]
    [SerializeField] private DataBase _dataBase;

    private static int _supplyPlacementInLayers = 3;

    private Queue<SupplyBox> _collectableSupply;
    private Coroutine _scanRoutine;
    private float _delay = 4;
    private int _targetLayer = 1 << _supplyPlacementInLayers;

    public event Action SuppliesFounded;

    private void OnEnable()
    {
        _dataBase.NoSuppliesLeft += StartScan;
    }

    private void OnDisable()
    {
        _dataBase.NoSuppliesLeft -= StartScan;
    }

    public void StartScan()
    {
        _scanRoutine = StartCoroutine(ScanWithRate());
    }

    private Queue<SupplyBox> ScanForSupplies()
    {
        Collider[] supplies = Physics.OverlapSphere(transform.position, _scanRadius, _targetLayer);

        Queue<SupplyBox> toCollect = new();

        foreach (Collider supply in supplies)
        {
            toCollect.Enqueue(supply.GetComponent<SupplyBox>());
        }

        if (toCollect.Count > 0)
        {
            _dataBase.GetSuppliesToCollect(toCollect);
            SuppliesFounded?.Invoke();
            StopCoroutine(_scanRoutine);
        }

        return toCollect;
    }

    private IEnumerator ScanWithRate()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);
        while (enabled)
        {
            yield return wait;

            _collectableSupply = ScanForSupplies();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}