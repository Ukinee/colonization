using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private DataBase _dataBase;
    [Header("Settings")]
    [SerializeField] private float _scanRadius;

    private static int _supplyPlacementInLayers = 3;

    private Queue<SupplyBox> _collectableSupply;
    private Coroutine _scanRoutine;
    private float _delay = 1f;
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
        _scanRoutine = StartCoroutine(Scan());
    }

    private Queue<SupplyBox> ScanForSupplies()
    {
        Collider[] supplies = Physics.OverlapSphere(transform.position, _scanRadius, _targetLayer);

        Queue<SupplyBox> toCollect = new();

        foreach (Collider supply in supplies)
        {
            // toCollect.Enqueue(supply.GetComponent<SupplyBox>());
            SupplyBox supplyBox = supply.GetComponent<SupplyBox>();

            if (supplyBox != null && !toCollect.Contains(supplyBox))
            {
                toCollect.Enqueue(supplyBox);
            }
        }

        if (toCollect.Count > 0)
        {
            _dataBase.GetSuppliesToCollect(toCollect);
            SuppliesFounded?.Invoke();
            StopCoroutine(_scanRoutine);
        }

        return toCollect;
    }

    private IEnumerator Scan()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_delay);

            _collectableSupply = ScanForSupplies();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}