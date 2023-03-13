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
    private readonly TileDataContainer _tileDataContainer;
    private readonly TileFinder _tileFinder;
    
    public TileMapGenerator(
        GameObject tilePrefab, 
        TileDataContainer tileDataContainer,
        TileFinder tileFinder
    )
    {
        _tilePrefab = tilePrefab;
        _tileDataContainer = tileDataContainer;
        _tileFinder = tileFinder;
    }
    
    public void GenerateMap()
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "All Tiles";
        // Observable.Range(0, 10).SelectMany(column => Observable.Range(0, 10).Select(row => new Vector3(column, 0, row)))
        //     .Subscribe(position => Object.Instantiate(_tilePrefab, position, Quaternion.identity, parentObject.transform));
        for (int column = 0; column < 100; column++)
        {
            for (int row = 0; row < 200; row++)
            {
                 var tileGameObject = Object.Instantiate(_tilePrefab, new Vector3(column, 0, row), quaternion.identity, parentObject.transform);
                 tileGameObject.name = $"Tile Game Object Number: {column}, {row}";
                 var renderer = tileGameObject.GetComponent<Renderer>();
                 var tileNumber = tileGameObject.GetComponentInChildren<TMP_Text>();
                 var tile = tileGameObject.GetComponent<Tile>();
                 tile.tileName = tileGameObject.name;
                 tile.xPosition = column;
                 tile.yPosition = row;
                 tile.elevation = -1;
                 _tileFinder.Tiles.Add(tile);
                 
                 tileNumber.text = $"{column}, {row}";

                 
            }
        }
        
        
    }

    public void UpdateTileVisuals()
    {
        for (int column = 0; column < 100; column++)
        {
            for (int row = 0; row < 200; row++)
            {
                var tile = _tileFinder.GetTile(column, row);
                var tileGameObject = tile.gameObject;

                
                MeshRenderer mr = tileGameObject.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = tileGameObject.GetComponentInChildren<MeshFilter>();

                if (tile.elevation >= 0)
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.GrassLands);
                }
                else
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Ocean);
                }
                
                mf.mesh = _tileDataContainer.GetTileMesh(MeshType.Water);
            }
        }
    }

    public void GenerateContinentMap()
    {
        GenerateMap();
        
        //Make some kind of raised area

        int numSplats = Random.Range(3, 6);
        for (int i = 0; i < numSplats; i++)
        {
            int range = Random.Range(3, 8);
            int y = Random.Range(range, 200 - range);
            int x = Random.Range(0, 12) - y / 2;
            
            ElevateArea(x, y, range);
        }
        
        ElevateArea(2,2, 2);
        ElevateArea(2,8, 3);
        ElevateArea(8,8, 2);

        UpdateTileVisuals();
    }

    void ElevateArea(int q, int r, int radius, float centerHeight = 0.5f)
    {
        var centerTile = _tileFinder.GetTile(q, r);
        var tiles = GetTilesWithRadiusOf(centerTile, radius);
        foreach (var tile in tiles)
        {
            if (tile.elevation < 0)
            {
                tile.elevation = 0;
            }
            tile.elevation += centerHeight * Mathf.Lerp(1f, 0.25f, DistanceBetweenTwoTiles(centerTile, tile) / radius);
        }
    }

    List<Tile> GetTilesWithRadiusOf(Tile centerTile, int radius)
    {
        var results = new List<Tile>();
        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                var tile = _tileFinder.GetTile(centerTile.xPosition + x,centerTile.yPosition + y);
                if (tile != null)
                {
                    results.Add(tile);
                }
            }
        }

        return results;
    }

    public float DistanceBetweenTwoTiles(Tile a, Tile b)
    {
        return
            Mathf.Max(
                Mathf.Abs(a.xPosition - b.xPosition),
                Mathf.Abs(a.yPosition - b.yPosition)
            );
    }
}

