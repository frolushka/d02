using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private string playerUnitsLayer;
    [SerializeField] private string[] enemyTargetLayers;
    
    [SerializeField] private Camera camera;

    private List<Unit> _currentSelection = new List<Unit>();
    private Collider2D[] _selectionCache = new Collider2D[5];
    
    private LayerMask _playerUnitsLayerMask;
    private LayerMask _enemyTargetLayerMask;

    private void Start()
    {
        _playerUnitsLayerMask = LayerMask.GetMask(playerUnitsLayer);
        _enemyTargetLayerMask = LayerMask.GetMask(enemyTargetLayers);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            if (Physics2D.OverlapPointNonAlloc(worldPoint, _selectionCache, _enemyTargetLayerMask) > 0)
            {
                Health target = null;
                for (var i = 0; i < _selectionCache.Length; i++)
                {
                    if (_selectionCache[i]
                        && ((1 << _selectionCache[i].gameObject.layer) & _enemyTargetLayerMask) != 0
                        && _selectionCache[i].TryGetComponent<Health>(out target))
                    {
                        break;
                    }
                }
                _currentSelection.ForEach(x => x.UpdateTarget(target));
            }
            else if (Physics2D.OverlapPointNonAlloc(worldPoint, _selectionCache, _playerUnitsLayerMask) > 0)
            {
                if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
                {
                    _currentSelection.Clear();
                }

                for (var i = 0; i < _selectionCache.Length; i++)
                {
                    if (_selectionCache[i] && _selectionCache[i].TryGetComponent<Unit>(out var unit))
                    {
                        _currentSelection.Add(unit);
                        unit.Select();
                        break;
                    }
                }
            }
            else
            {
                _currentSelection.ForEach(x =>
                {
                    if (x)
                    {
                        x.UpdateTargetPosition(worldPoint);
                    }
                });
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _currentSelection.Clear();
        }
    }
}
