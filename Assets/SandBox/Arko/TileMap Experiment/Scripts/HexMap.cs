using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    [SerializeField]private GameObject tile;
    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int column = 0; column < 10; column++)
        {
            for (int row = 0; row < 10; row++)
            {
                Instantiate(tile, new Vector3(column, 0, row), quaternion.identity, this.transform);
            }
        }
    }
}

