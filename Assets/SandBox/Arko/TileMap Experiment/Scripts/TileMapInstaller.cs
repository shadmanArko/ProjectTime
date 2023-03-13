using System.Collections.Generic;
using SandBox.Arko.TileMap_Experiment.Scripts;
using Unity.Entities;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "TileMapInstaller", menuName = "Installers/TileMapInstaller")]
public class TileMapInstaller : ScriptableObjectInstaller<TileMapInstaller>
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private ScriptableObject tileDataContainer;
    
    public override void InstallBindings()
    {
        Container.Bind<TileMapGenerator>().AsSingle();
        Container.BindInstance(tilePrefab).WhenInjectedInto<TileMapGenerator>();
        Container.Bind<TileDataContainer>().FromScriptableObject(tileDataContainer).AsSingle();
        
        Container.Bind<TileFinder>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<TileMapController>().AsSingle().NonLazy();
    }
    
}