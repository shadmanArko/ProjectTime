using System;
using System.Collections;
using System.Collections.Generic;
using SandBox.Arko.TileMap_Experiment.Scripts;
using TMPro;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TileMapGenerator 
{
    private readonly GameObject _tilePrefab;
    private readonly List<Material> _materials;
    private readonly TileFinder _tileFinder;
    
    public TileMapGenerator(GameObject tilePrefab, List<Material> materials, TileFinder tileFinder)
    {
        _tilePrefab = tilePrefab;
        _materials = materials;
        _tileFinder = tileFinder;
    }
    
    public void GenerateMap()
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "All Tiles";
        // Observable.Range(0, 10).SelectMany(column => Observable.Range(0, 10).Select(row => new Vector3(column, 0, row)))
        //     .Subscribe(position => Object.Instantiate(_tilePrefab, position, Quaternion.identity, parentObject.transform));
        for (int column = 0; column < 10; column++)
        {
            for (int row = 0; row < 10; row++)
            {
                 var tileGameObject = Object.Instantiate(_tilePrefab, new Vector3(column, 0, row), quaternion.identity, parentObject.transform);
                 tileGameObject.name = $"Tile Game Object Number: {column}, {row}";
                 var renderer = tileGameObject.GetComponent<Renderer>();
                 var tileNumber = tileGameObject.GetComponentInChildren<TMP_Text>();
                 var tile = tileGameObject.GetComponent<Tile>();
                 tile.name = tileGameObject.name;
                 tile.xPosition = column;
                 tile.yPosition = row;
                 _tileFinder.Tiles.Add(tile);
                 
                 tileNumber.text = $"{column}, {row}";
                 if (renderer != null && _materials.Count > 0)
                 {
                     renderer.material = _materials[Random.Range(0, _materials.Count)];
                 }
            }
        }
        
        
    }
}

