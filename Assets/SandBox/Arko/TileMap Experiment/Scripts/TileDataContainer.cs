using System.Linq.Expressions;
using UnityEngine;

namespace SandBox.Arko.TileMap_Experiment.Scripts
{
    [CreateAssetMenu(fileName = "_tileDataContainer", menuName = "Tile/_tileDataContainer", order = 1)]
    public class TileDataContainer : ScriptableObject
    {
        [Header("Mesh")] 
        [SerializeField] private Mesh meshWater;
        [SerializeField] private Mesh meshFlat;
        [SerializeField] private Mesh meshHill;
        [SerializeField] private Mesh meshMountain;
    
        [Header("Materials")]
        [SerializeField] private Material matOcean;
        [SerializeField] private Material matPlains;
        [SerializeField] private Material matGrassLands;
        [SerializeField] private Material matMountains;

        public Mesh GetTileMesh(MeshType meshType)
        {
            switch (meshType)
            {
                case MeshType.Flat:
                    return meshFlat;
                case MeshType.Hill:
                    return meshHill;
                case MeshType.Mountain:
                    return meshMountain;
                case MeshType.Water:
                    return meshWater;
                default:
                    return meshFlat;
            }
        }

        public Material GetTileMaterial(MaterialType materialType)
        {
            switch (materialType)
            {
                case MaterialType.Mountains:
                    return matMountains;
                case MaterialType.Ocean:
                    return matOcean;
                case MaterialType.Plains:
                    return matPlains;
                case MaterialType.GrassLands:
                    return matGrassLands;
                default:
                    return matPlains;
            }
        }
    }
}