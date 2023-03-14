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
 
    public float heightMountain = 1.2f;
    public float heightHill = 0.6f;
    public float heightFlat = 0.0f;

    public float moistureJungle = 1f;
    public float moistureForest = 0.8f;
    public float moistureGrassLands = 0.33f;
    public float moisturePlains = 0f;
    
    public int numRows = 40;
    public int numColumns = 80;
    public int numContinents = 3;
    
    
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
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                 var tileGameObject = Object.Instantiate(_tilePrefab, new Vector3(column, 0, row), quaternion.identity, parentObject.transform);
                 tileGameObject.name = $"Tile Game Object Number: {column}, {row}";
                 var renderer = tileGameObject.GetComponent<Renderer>();
                 var tileNumber = tileGameObject.GetComponentInChildren<TMP_Text>();
                 var tile = tileGameObject.GetComponent<Tile>();
                 tile.tileName = tileGameObject.name;
                 tile.xPosition = column;
                 tile.yPosition = row;
                 tile.elevation = -0.5f;
                 _tileFinder.Tiles.Add(tile);
                 
                 tileNumber.text = $"{column}, {row}";

                 
            }
        }
        
        
    }

    public void UpdateTileVisuals()
    {
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                var tile = _tileFinder.GetTile(column, row);
                var tileGameObject = tile.gameObject;

                
                MeshRenderer mr = tileGameObject.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = tileGameObject.GetComponentInChildren<MeshFilter>();

                if (tile.elevation >= heightMountain)
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Mountains);
                    mf.mesh = _tileDataContainer.GetTileMesh(MeshType.Mountain);
                }
                else if (tile.elevation >= heightHill)
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.GrassLands);
                    mf.mesh = _tileDataContainer.GetTileMesh(MeshType.Hill);
                }
                else if (tile.elevation >= heightFlat)
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Plains);
                    mf.mesh = _tileDataContainer.GetTileMesh(MeshType.Flat);
                }
                else
                {
                    mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Ocean);
                    mf.mesh = _tileDataContainer.GetTileMesh(MeshType.Water);
                }

                if (tile.elevation >= heightFlat)
                {
                    if (tile.moisture >= moistureJungle)
                    {
                        mr.material = _tileDataContainer.GetTileMaterial(MaterialType.GrassLands);
                    }
                    else if (tile.moisture >= moistureForest)
                    {
                        mr.material = _tileDataContainer.GetTileMaterial(MaterialType.GrassLands);
                    }
                    else if (tile.moisture >= moistureGrassLands)
                    {
                        mr.material = _tileDataContainer.GetTileMaterial(MaterialType.GrassLands);
                    }
                    else if (tile.moisture >= moisturePlains)
                    {
                        mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Plains);
                    }
                    else
                    {
                        mr.material = _tileDataContainer.GetTileMaterial(MaterialType.Desert);
                    }
                }
            }
        }
    }

    public void GenerateContinentMap()
    {
        GenerateMap();
        
        //Make some kind of raised area
        int continentSpacing = numColumns / numContinents;

        for (int c = 0; c < numContinents; c++)
        {
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, numRows - range);
                int x = Random.Range(0, 10) - y / 2+(c * continentSpacing);

                if (x < 0)
                {
                    x += numColumns;
                }
            
                ElevateArea(x, y, range);
            }
        }
        
        //Add Perlin Noise for tiles
        float noiseResolution = 0.05f; //smaller number make more cherabera 
        Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        float noiseScale = 2f; //Larger Value makes more Island
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                var tile = _tileFinder.GetTile(column, row);
                var perlinNoise = Mathf.PerlinNoise(((float)column / Mathf.Max(numColumns,numRows) / noiseResolution) + noiseOffset.x,
                    ((float)row / Mathf.Max(numColumns,numRows) / noiseResolution) + noiseOffset.y )- 0.5f;
                tile.elevation += perlinNoise * noiseScale;
            }
        }
        
        //Add Perlin Noise for Moisture
        noiseResolution = 0.05f; //smaller number make more cherabera 
        noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
          
        noiseScale = 2f; //Larger Value makes more Island
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                var tile = _tileFinder.GetTile(column, row);
                var perlinNoise = Mathf.PerlinNoise(((float)column / Mathf.Max(numColumns,numRows) / noiseResolution) + noiseOffset.x,
                    ((float)row / Mathf.Max(numColumns,numRows) / noiseResolution) + noiseOffset.y )- 0.5f;
                tile.moisture = perlinNoise * noiseScale;
            }
        }

        UpdateTileVisuals();
    }

    void ElevateArea(int q, int r, int radius, float centerHeight = 0.8f)
    {
        var centerTile = _tileFinder.GetTile(q, r);
        var tiles = GetTilesWithRadiusOf(centerTile, radius);
        foreach (var tile in tiles)
        {
            // if (tile.elevation < 0)
            // {
            //     tile.elevation = 0;
            // }
            tile.elevation += centerHeight * Mathf.Lerp(1f, 0.25f,Mathf.Pow((DistanceBetweenTwoTiles(centerTile, tile) / radius),2f));
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

