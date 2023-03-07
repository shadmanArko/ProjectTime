using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class HexMap 
{
    private readonly GameObject _hexPrefab;
    
    //private readonly Transform _parentTransform;
    
    public HexMap(GameObject hexPrefab)
    {
        _hexPrefab = hexPrefab;
        //_parentTransform = parentTransform;
    }
    
    public void GenerateMap()
    {
        Observable.Range(0, 10).SelectMany(column => Observable.Range(0, 10).Select(row => new Vector3(column, 0, row)))
            .Subscribe(position => Object.Instantiate(_hexPrefab, position, Quaternion.identity));
        // for (int column = 0; column < 10; column++)
        // {
        //     for (int row = 0; row < 10; row++)
        //     {
        //         Instantiate(tile, new Vector3(column, 0, row), quaternion.identity, this.transform);
        //     }
        // }
    }
}

