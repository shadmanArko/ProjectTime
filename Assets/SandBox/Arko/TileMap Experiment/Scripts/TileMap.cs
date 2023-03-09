using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class TileMap 
{
    private readonly GameObject _tilePrefab;
    public TileMap(GameObject tilePrefab)
    {
        _tilePrefab = tilePrefab;
    }
    
    public void GenerateMap()
    {
        GameObject parentObject = new GameObject();
        // Observable.Range(0, 10).SelectMany(column => Observable.Range(0, 10).Select(row => new Vector3(column, 0, row)))
        //     .Subscribe(position => Object.Instantiate(_tilePrefab, position, Quaternion.identity, parentObject.transform));
        for (int column = 0; column < 10; column++)
        {
            for (int row = 0; row < 10; row++)
            {
                Object.Instantiate(_tilePrefab, new Vector3(column, 0, row), quaternion.identity, parentObject.transform);
            }
        }
    }
}

